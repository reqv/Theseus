using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Cerber (Cerber)</b>
 * </summary>
 * <remarks>
 * 	Klasa Cerber dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Cerber : Monster {

	[Tooltip ("Zmienna określająca czas po którym Cerber rozglada się w inna stronę ( sekundy )")]
	[SerializeField]
	/// <summary>
	/// 	Zmienna określająca czas po którym wojownik rozglada się w inna stronę
	/// </summary>
	private double _freeFlipTimer;

	/// <summary>
	/// 	Zmienna określająca pozostały czas po którym Cerber zmieni stronę ( czyt. Flip() )
	/// </summary>
	private double _actualFreeFlipTimer;

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
		
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Cerber goni za graczem po całym pomieszczeniu.
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
	/// 	W przypadku Cerbera, nieustannie goni on gracza, jeżeli gracz nie istnieje Cerber rozgląda się w poszukiwaniu ofiary.
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
