using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 * 	Klasa reprezentująca przedmiot w sklepie.
 * </summary>
 * <remarks>
 *  Wyświetla przedmiot i cenę oraz reaguje na próbę zakupu
 * </remarks>
 */
[Serializable]
public class ShopItem : MonoBehaviour 
{
    [Tooltip("Sprzedawany przedmiot")]
    [SerializeField]
    ///
    public Item _item;
    [Tooltip("Koszt przedmiotu")]
    [SerializeField]
    /// <summary>
    /// Koszt przedmiotu
    /// </summary>
    public int _cost;

    /// <summary>
    /// Metoda reagująca na kolizje
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {
        var p = other.gameObject.GetComponent<MainCharacter>();

        if (p != null && p.Coins >= _cost)
        {
            p.Coins -= _cost;
            _item.EffectOfItem(other);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Metoda przygotowująca obiekt - niezbędna do wywołana po stworzeniu obiektu
    /// </summary>
    /// <param name="item">Sprzedawany przedmiot</param>
    /// <param name="cost">Koszt przedmiotu</param>
    public void Prepare(Item item, int cost)
    {
        _item = item;
        _cost = cost;
        GetComponent<SpriteRenderer>().sprite = _item.GetComponent<SpriteRenderer>().sprite;
        GetComponentInChildren<Text>().text = cost.ToString() + "G";
    }
}
