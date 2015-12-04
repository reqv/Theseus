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
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
		_actualFreeFlipTimer = _freeFlipTimer;
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas wykrycia zderzenia obiektu z innym obiektem
	/// </summary>
	/// <remarks>
	/// 	Podczas zderzenia wykonujemy odpowiednią akcję zależna od obiektu, gdy jest nim gracz to rozpoczynamy atak
	/// </remarks>
	/// <param name="other">Obiekt z którym odbyło się zderzenie</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			Destroy(other.gameObject);		// ######################## chwilowe rozwiazanie ( usuwanie gracza ) ########################
		}
		_collisionAxis = _axis;
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
    public override void Attack()
    {
     //   Debug.Log("Attakck!! !! ");
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Wojownik goni za graczem po całym pomieszczeniu.
	/// </remarks>
	public override void Chase()
    {
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
