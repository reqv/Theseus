using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShopItem : MonoBehaviour 
{
    [SerializeField]
    public Item _item;
    [SerializeField]
    public int _cost;

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        var p = other.gameObject.GetComponent<MainCharacter>();

        if (p != null && p.Coins >= _cost)
        {
            p.Coins -= _cost;
            var boughtItem = Instantiate(_item, transform.position, Quaternion.identity) as Item;
            //boughtItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 3.0f));

            Destroy(gameObject);
        }
    }

    public void Prepare(Item item, int cost)
    {
        _item = item;
        _cost = cost;
        GetComponent<SpriteRenderer>().sprite = _item.GetComponent<SpriteRenderer>().sprite;
        GetComponentInChildren<Text>().text = cost.ToString();
    }
}
