using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 	Klasa pocisków
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : TheseusGameObject {

	[Tooltip ("Czas po którym element zostanie zniszczony")]
	[SerializeField]
	/// <summary>
	/// 	Czas w którym cień wykonuje skok(teleport)
	/// </summary>
	protected int _timeToLive;

    [Tooltip("Obiekty o podanych tagach otrzymają obrażenia od pocisku")]
    [SerializeField]
    /// <summary>
    /// 	Obiekty o podanych tagach otrzymają obrażenia od pocisku
    /// </summary>
    protected string[] _damageTags;

    [Tooltip("Dodatkowy efekt kontroli tłumu")]
    [SerializeField]
    /// <summary>
    /// 	Dodatkowy efekt kontroli tłumu
    /// </summary>
    protected Status _additionalCCEffect;

    [Tooltip("Czas dodatkowego efektu kontroli tłumu")]
    [SerializeField]
    /// <summary>
    /// 	Czas dodatkowego efektu kontroli tłumu
    /// </summary>
    protected float _additionalCCEffectTime;

	/// <summary>
	/// 	Właściwość pozwalająca określić ilość obrażeń zadawanych przy zderzeniu
	/// </summary>
	public int Damage {get; set;}

	/// <summary>
	/// 	Metoda ustawiająca początkowe zmienne i inicjalizująca pocisk
	/// </summary>
	public void Start()
	{
		Destroy (this.gameObject, _timeToLive);
	}

	/// <summary>
	/// 	Metoda uruchamiana, gdy pocisk wejdzie w kolizję z innym obiektem
	/// </summary>
	/// <param name="other">Element obcy z którym wystąpiła kolizja</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (_damageTags.Contains(other.gameObject.tag)) 
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.TakingDamage(this.Damage);
                character.SetCrowdControl(_additionalCCEffect, _additionalCCEffectTime);
            }

            Destroy(this.gameObject);
		}
        else if(other.gameObject.tag != "Item" && other.gameObject.tag != "Projectile")
		{
			Destroy(this.gameObject);
		}
	}
}