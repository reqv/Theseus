using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Cień (Shadow)</b>
 * </summary>
 * <remarks>
 * 	Klasa cienia dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Shadow : Monster {

	[Tooltip ("Obiekt magicznej kula, rzucanej podczas ataku.")]
	[SerializeField]
	/// <summary>
	/// 	Obiekt magicznej kula, rzucanej podczas ataku.
	/// </summary>
	private GameObject _magic;
	
	[Tooltip ("Prędkość magicznych kul cienia.")]
	[SerializeField]
	/// <summary>
	/// 	Prędkość magicznych kul cienia.
	/// </summary>
	private int _magicSpeed;

	[Tooltip ("Zmienna określająca czas po którym cień rozglada się w inna stronę ( sekundy )")]
	[SerializeField]
	/// <summary>
	/// 	Zmienna określająca czas po którym cień rozglada się w inna stronę
	/// </summary>
	private double _freeFlipTimer;
	
	[Tooltip ("Czas w którym cień wykonuje skok(teleport)")]
	[SerializeField]
	/// <summary>
	/// 	Czas w którym cień wykonuje skok(teleport)
	/// </summary>
	private int _timeToMove;

	/// <summary>
	/// 	Zmienna określająca pozostały czas po którym cień zmieni stronę ( czyt. Flip() )
	/// </summary>
	private double _actualFreeFlipTimer;

	/// <summary>
	/// 	Zegar identyfikujący czas w którym wykonany bedzie skok
	/// </summary>
	private double _moveTimer;

	/// <summary>
	/// 	Zmienna sprawdzająca czy cień wykonał skok
	/// </summary>
	private bool _teleported = false;
	
	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
		_actualFreeFlipTimer = _freeFlipTimer;
		_moveTimer = _timeToMove;
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	/// <remarks>
	/// 	Cień nie wykorzystuje tej metody, sterowanie przekierowane jest na funkcje Chase()
	/// </remarks>
	public override void Attack()
	{
		if (_attackDistance > 0)
			_attackDistance = 0;
		Chase ();
	}

	/// <summary>
	/// 	Zaimplementowana metoda w której cień eksploduje
	/// </summary>
	/// <remarks>
	/// 	Cień spowalnia gracza i wysysa z niego energię do czasu, gdy naładuje moc w celu wywołania eksplozji(tej funkcji).
	/// </remarks>
	public void Explode()
	{
		NewProjectile(_magic,new Vector2(0,0.4f),new Vector2(0,_magicSpeed));
		NewProjectile(_magic,new Vector2(0,-0.4f),new Vector2(0,-_magicSpeed));
		NewProjectile(_magic,new Vector2(0.4f,0),new Vector2(_magicSpeed,0));
		NewProjectile(_magic,new Vector2(-0.4f,0),new Vector2(-_magicSpeed,0));

		NewProjectile(_magic,new Vector2(0.2f,0.2f),new Vector2(_magicSpeed,_magicSpeed));
		NewProjectile(_magic,new Vector2(-0.2f,-0.2f),new Vector2(-_magicSpeed,-_magicSpeed));
		NewProjectile(_magic,new Vector2(-0.2f,0.2f),new Vector2(-_magicSpeed,_magicSpeed));
		NewProjectile(_magic,new Vector2(0.2f,-0.2f),new Vector2(_magicSpeed,-_magicSpeed));

		_moveTimer = _timeToMove;
		_teleported = false;
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Cień pojawia się w miejscu gracza i spowalnia jego ruchy.
	/// </remarks>
	public override void Chase()
	{
		//TUTAJ BEDZIE SLOW NA GRACZA
		//if(_targetToAttack!= null && WhereIsATarget(_targetToAttack.position,true) <= _range)Debug.Log ("Slow the target !");
		if (_moveTimer <= 0) {
			Explode();
			Walking ();
		} else
			_moveTimer -= Time.deltaTime;
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku cienia, teleportuje się on na miejsce gracza, jeżeli gracz nie istnieje cień rozgląda się w poszukiwaniu ofiary.
	/// </remarks>
	public override void Walking()
	{
		if (_teleported)
			Chase ();
		if (_targetToAttack != null & !_teleported) {
			_teleported = true;
			_Rig2D.MovePosition (_targetToAttack.position);
		}
		else {
			if(_actualFreeFlipTimer <= 0)
			{
				Flip();
				_actualFreeFlipTimer = _freeFlipTimer;
			}
			else
				_actualFreeFlipTimer -= Time.deltaTime;
		}
	}

	/// <summary>
	/// 	Metoda uruchamiana poodczas dotkniecia gracza.
	/// </summary>
	/// <remarks>
	/// 	W przypadku zetknięcia się z pociskiem gracza, umiera.
	/// </remarks>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "PlayerProjectile") {
			TakingDamage (100);
		}
	}
}
