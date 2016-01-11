using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Wojownik (Warrior)</b>
 * </summary>
 * <remarks>
 * 	Klasa wojownika dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Warrior : Monster {

	[Tooltip ("Zmienna określająca czas po którym wojownik rozglada się w inna stronę ( sekundy )")]
	[SerializeField]
	/// <summary>
	/// 	Zmienna określająca czas po którym wojownik rozglada się w inna stronę
	/// </summary>
	private double _freeFlipTimer;

	/// <summary>
	/// 	Zmienna określająca pozostały czas po którym wojownik zmieni stronę ( czyt. Flip() )
	/// </summary>
	private double _actualFreeFlipTimer;


	/// <summary>
	/// 	Timer pozwalający na poprawę animacji ataku.
	/// </summary>
	private float _attackAnimationTimer = 0f;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
		_actualFreeFlipTimer = _freeFlipTimer;
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
    public override void Attack()
    {
		if (_attackAnimationTimer <= 0) {
			_Anim.SetTrigger ("Attack");
			_attackAnimationTimer = 0.3f;
		} else
			_attackAnimationTimer -= Time.deltaTime;
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Wojownik goni za graczem po całym pomieszczeniu.
	/// </remarks>
	public override void Chase()
    {
		_Anim.SetBool ("Walking", true);
		WhereIsATarget (_targetToAttack.transform.position);
		_Rig2D.AddForce (_axis * _realMaxSpeed);
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku wojownika, nieustannie goni on gracza, jeżeli gracz nie istnieje wojownik rozgląda się w poszukiwaniu ofiary.
	/// </remarks>
	public override void Walking()
    {
		if (_targetToAttack != null)
			Chase ();
		else {
			_Anim.SetBool ("Walking", false);
			if(_actualFreeFlipTimer <= 0)
			{
				Flip();
				_actualFreeFlipTimer = _freeFlipTimer;
			}
			else
				_actualFreeFlipTimer -= Time.deltaTime;
		}
    }
}
