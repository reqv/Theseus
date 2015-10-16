using UnityEngine;
using System.Collections;

/**
 * <summary>
 * 	Klasa potwora:<b> Pająk (Spider)</b>
 * </summary>
 * <remarks>
 * 	Klasa pająka dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Spider : Monster {

	[Tooltip ("Czas oczekiwania przed rozpoczęciem ataku(w sekundach)")]
	[SerializeField]
	/// <summary>
	/// 	Parametr oczekiwania pająka na atak po wykryciu ofiary
	/// </summary>
	private long _spiderAttackDelay;

	[Tooltip ("Minimalny czas oczekiwania po wykonaniu ataku do wykonania następnego(w sekundach), wartosc musi być wieksza od 0")]
	[SerializeField]
	/// <summary>
	/// 	Parametr opisujący czas oczekiwania po dokonanym ataku
	/// </summary>
	private long _spiderDelayAfterAttack;

	/// <summary>
	/// 	Parametr sprawdzający, czy pająk jest zdolny do ataku
	/// </summary>
	private bool _readyToAttack = false;

	/// <summary>
	/// 	Parametr trzymający czas w sekundach w którym nastąpi atak
	/// </summary>
	private double _timeToAttack;

	/// <summary>
	/// 	Parametr trzymający czas w sekundach w którym pająk znów bedzie aktywny
	/// </summary>
	private double _timeToWait;

	/// <summary>
	/// 	Wektor trzymający dany cel podróży wybrany przez pająka
	/// </summary>
	private Vector2 _spiderDestination;

	/// <summary>
	/// 	Parametr sprawdzający, czy pająk wybrał już cel swojego ataku
	/// </summary>
	private bool _hasTarget = false;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	public override void Start () {
		base.Start ();
		_Rig2D = GetComponent<Rigidbody2D>();
		_axis = new Vector2(0, 0);
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	public override void Attack()
	{
		//atak
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	W przypadku pająka - gdy zobaczy ofiarę szykuję się do skoku w jej strone
	/// </remarks>
	public override void Chase()
	{
		if (_readyToAttack) {

			if(Time.time >= _timeToAttack)
			{
				if(!_hasTarget)
				{
					_spiderDestination.x = _targetToAttack.position.x;
					_spiderDestination.y = _targetToAttack.position.y;
					_spiderDestination -= _Rig2D.position;
					_Rig2D.velocity = _spiderDestination*_maxSpeed;
					_hasTarget = true;
				}

				if(Time.time >= _timeToWait)
				{
					_hasTarget = false;
					_readyToAttack = false;
					_Rig2D.MovePosition (_Rig2D.position);
				}
			}

		} else {
			_timeToAttack = Time.time + _spiderAttackDelay;
			_timeToWait = _timeToAttack+_spiderDelayAfterAttack;
			_readyToAttack = true;

		}
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku pająka - utrzymuje się w jednym miejscu
	/// </remarks>
	public override void Walking()
	{
		_readyToAttack = false;
		_Rig2D.MovePosition (_Rig2D.position);
	}
}