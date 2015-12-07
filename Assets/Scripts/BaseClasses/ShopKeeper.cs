using UnityEngine;
using System.Collections;
using System;

public class ShopKeeper : Monster 
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

    private bool _attacked = false;

	void Start () 
    {
        base.Start();
        PlaceItems();
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

            si.transform.SetParent(transform.parent);

            int randomItem = UnityEngine.Random.Range(0, _availableItems.Length);
            si.Prepare(_availableItems[0].Item, _availableItems[0].Cost);
        }
    }

    bool IsTop()
    {
        //brzydko, pozycja ustalana jest w RoomManager
        return transform.position.y == 2.25f;
    }

    public override void Attack()
    {
    }

    public override void Chase()
    {
        if(_attacked)
        {
            WhereIsATarget(_targetToAttack.position);
            _Rig2D.AddForce(_axis * _realMaxSpeed);
        }
    }

    public override void Walking()
    {
    }

    public override void TakingDamage(int damage)
    {
        base.TakingDamage(damage);
        if (damage > 0)
        {
            Debug.Log("aaa");
            _attacked = true;
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
