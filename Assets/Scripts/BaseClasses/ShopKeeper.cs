using UnityEngine;
using System.Collections;
using System;

/**
 * <summary>
 * 	Klasa sklepikarza
 * </summary>
 * <remarks>
 *  Tworzy produkty z dostępnego zakresu, zaatakowany próbuje zabić gracza
 * </remarks>
 */
public class ShopKeeper : Monster 
{

    [Serializable]
    private struct ItemCostPair
    {
        public Item Item;
        public int Cost;
    }


    [SerializeField]
    /// <summary>
    /// Wsyzstkie dostępne przedmioty i ich ceny
    /// </summary>
    private ItemCostPair[] _availableItems;

    [SerializeField]
    /// <summary>
    /// Prefab klasy ShopItem z które zostaną utworzone przedmioty na sprzedarz
    /// </summary>
    private ShopItem _shopItem;

    /// <summary>
    /// Zmienna przechowująca czy przeciwnik zaatakował sklepikarza
    /// </summary>
    private bool _attacked = false;

    [SerializeField]
    private Projectile _projectile;

	void Start () 
    {
        base.Start();
        PlaceItems();
	}

    /// <summary>
    /// Metoda umieszczająca w nad lub pod sklepikarzem przemioty na sprzedarz
    /// </summary>
    void PlaceItems()
    {

        for (float positionX = -1.5f; positionX <= 1.5f; positionX += 1.5f)
        {
            var si = Instantiate(_shopItem) as ShopItem;
            si.transform.SetParent(transform);

            if (IsTop())
                si.transform.localPosition = new Vector3(positionX, -1, 0);
            else
                si.transform.localPosition = new Vector3(positionX, 1, 0);

            si.transform.SetParent(transform.parent);

            int randomItem = UnityEngine.Random.Range(0, _availableItems.Length);
            si.Prepare(_availableItems[0].Item, _availableItems[0].Cost);
        }
    }

    /// <summary>
    /// Metoda pomocnicza sprawdzająca czy pokój ze sklepem posiada górnego sąsiada
    /// </summary>
    /// <returns></returns>
    bool IsTop()
    {
        //brzydko, pozycja ustalana jest w RoomManager
        return transform.position.y == 2.25f;
    }

    /// <summary>
    /// 	Zaimplementowana metoda atakujaca szukany obiekt
    /// </summary>
    /// <remarks>
    /// 	W przypadku węża - podąża on za ofiarą z odpowiednio większą prędkością
    /// </remarks>
    public override void Attack()
    {;
        if(_attacked)
        {
            Vector2 vector = transform.position - _targetToAttack.position;

            NewProjectile(_projectile.gameObject, new Vector2(0, 0), new Vector2(-vector.x, -vector.y) * 0.9f);
            NewProjectile(_projectile.gameObject, new Vector2(0, 0), new Vector2(-vector.x, -vector.y));
            NewProjectile(_projectile.gameObject, new Vector2(0, 0), new Vector2(-vector.x, -vector.y) * 1.1f);
        }
    }

    /// <summary>
    /// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
    /// </summary>
    public override void Chase()
    {
    }


    /// <summary>
    /// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
    /// </summary>
    /// <remarks>
    /// 	Sklepikarz nie patroluje
    /// </remarks>
    public override void Walking()
    {
    }

    /// <summary>
    /// 	Zaimplementowana metoda odpowiedzialna za otrzymywane obrażeń.
    /// </summary>
    /// <remarks>
    /// 	W przypadku ataku zmienna _attacked jest ustawiana na true.
    /// </remarks>
    public override void TakingDamage(int damage)
    {
        base.TakingDamage(damage);
        if (damage > 0)
        {
            _attacked = true;
        }
    }
}
