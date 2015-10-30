using UnityEngine;

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
	private byte _spiderAttackDelay;

	[Tooltip ("Minimalny czas oczekiwania po wykonaniu ataku do wykonania następnego(w sekundach), wartosc musi być wieksza od 0")]
	[SerializeField]
	/// <summary>
	/// 	Parametr opisujący czas oczekiwania po dokonanym ataku
	/// </summary>
	private byte _spiderDelayAfterAttack;

	/// <summary>
	/// 	Parametr sprawdzający, czy pająk jest zdolny do ataku
	/// </summary>
	private bool _readyToAttack = false;

	/// <summary>
	/// 	Parametr trzymający czas w sekundach w którym nastąpi atak
	/// </summary>
	private float _timeToAttack;

	/// <summary>
	/// 	Parametr trzymający czas w sekundach w którym pająk znów bedzie aktywny
	/// </summary>
	private float _timeToWait;
	
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
		_axis = Vector2.zero;
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
	/// 	W przypadku pająka - gdy zobaczy ofiarę szykuję się do skoku w jej strone po czym odpoczywa.
	/// </remarks>
	public override void Chase()
	{
		if (_readyToAttack) {

			WhereIsATarget(_targetToAttack.transform.position);
			if(_axis.x > 0 && !_facingRight)
				Flip ();
			if(_axis.x < 0 && _facingRight)
				Flip ();

			if(_timeToAttack < 0)
			{
				if(!_hasTarget)
				{
					_axis.x = _targetToAttack.position.x;
					_axis.y = _targetToAttack.position.y;
					_axis -= _Rig2D.position;
					_Rig2D.AddForce(_axis * _maxSpeed,ForceMode2D.Impulse);
					_hasTarget = true;
				}

				if(_timeToWait < 0)
				{
					_hasTarget = false;
					_readyToAttack = false;
					_Rig2D.velocity = Vector2.zero;
				}
				else
					_timeToWait -= Time.deltaTime;
			}
			else
			{
				_timeToAttack -= Time.deltaTime;
			}

		} else {
			WhereIsATarget(_targetToAttack.transform.position);
			if(_axis.x > 0 && !_facingRight)
				Flip ();
			if(_axis.x < 0 && _facingRight)
				Flip ();

			_timeToAttack = _spiderAttackDelay;
			_timeToWait = _spiderDelayAfterAttack;
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
		_hasTarget = false;
		_Rig2D.velocity = Vector2.zero;
	}
}