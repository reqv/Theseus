using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 	Klasa min
/// </summary>
public class Mines : TheseusGameObject {

	[Tooltip ("Czas po którym element zostanie zniszczony")]
	[SerializeField]
	/// <summary>
	/// 	Czas po którym element zostanie zniszczony
	/// </summary>
	protected int _timeToLive;

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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			var playerScript = other.gameObject.GetComponent<MainCharacter> ();
			playerScript.SetCrowdControl (_additionalCCEffect, _additionalCCEffectTime);
			Destroy (this.gameObject);
		}
	}
}