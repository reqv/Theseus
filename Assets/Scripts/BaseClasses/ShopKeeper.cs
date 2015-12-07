using UnityEngine;
using System.Collections;
using System;

public class ShopKeeper : MonoBehaviour 
{
    [Serializable]
    private struct ItemCostPair
    {
        public Item Item;
        public int Cost;
    }

    [SerializeField]
    private ItemCostPair[] _availableItems;

    [SerializeField]
    private ShopItem _shopItem;

	void Start () 
    {
        PlaceItems();
	}
	
	void Update () 
    {
	
	}

    void PlaceItems()
    {

        for (int positionX = -8; positionX <= 8; positionX += 8)
        {
            var si = Instantiate(_shopItem) as ShopItem;
            si.transform.SetParent(transform);

            if (IsTop())
                si.transform.localPosition = new Vector3(positionX, -4, 0);
            else
                si.transform.localPosition = new Vector3(positionX, 4, 0);

            int randomItem = UnityEngine.Random.Range(0, _availableItems.Length);
            si.Prepare(_availableItems[0].Item, _availableItems[0].Cost);
        }
    }

    bool IsTop()
    {
        //brzydko, pozycja ustalana jest w RoomManager
        return transform.position.y == 2.25f;
    }
}
