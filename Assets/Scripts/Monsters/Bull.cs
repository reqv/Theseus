using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Byka (Bull)</b>
 * </summary>
 * <remarks>
 * 	Klasa byka dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Bull : Monster {

	[Tooltip ("Zmienna określająca czas po którym byk rozglada się w inna stronę ( sekundy )")]
	[SerializeField]
	/// <summary>
	/// 	Zmienna określająca czas po którym byk rozglada się w inna stronę
	/// </summary>
	private double _freeFlipTimer;

	[Tooltip ("Zmienna określająca czas w którym byk pozostaje w stanie oszołomienia")]
	[SerializeField]
	/// <summary>
	/// 	Zmienna określająca czas w którym byk pozostaje w stanie oszołomienia
	/// </summary>
	private int _timeOnStun;

	/// <summary>
	/// 	Zmienna określająca pozostały czas po którym byk zmieni stronę ( czyt. Flip() )
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
		if (_status == Status.Stunned)
			return;
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);		// ######################## chwilowe rozwiazanie ( usuwanie gracza ) ########################
		} else if (other.gameObject.tag == "Enemy") {
			Monster script = other.gameObject.GetComponent<Monster>();
			script.TakingDamage(_attackPower);
			script.SetCrowdControl(Status.Stunned,_timeOnStun);
		}else if(other.gameObject.tag != "Item" && other.gameObject.tag != "Projectile" && other.gameObject.tag != "PlayerProjectile")
			{
				SetCrowdControl(Status.Stunned,_timeOnStun);
				_Rig2D.velocity = Vector2.zero;
			}
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
	/// 	Byk goni za graczem, gdy ten pojawi się w pokoju. Gdy uderzy z rozpędu w ścianę, zostaję oszołomiony.
	/// </remarks>
	public override void Chase()
    {
		WhereIsATarget (_targetToAttack.position);
		_Rig2D.AddForce(_axis * _realMaxSpeed,ForceMode2D.Impulse);
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku byka, nieustannie goni on gracza, jeżeli gracz nie istnieje byk rozgląda się w poszukiwaniu ofiary.
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

	/// <summary>
	/// 	Reimplementowana metoda TakingDamage(Monster)
	/// </summary>
	/// <remarks>
	/// 	W przypadku byka może on zostać zaatakowany tylko, gdy jest oszołomiony.
	/// </remarks>
	public override void TakingDamage (int damage)
	{
		if(_status == Status.Stunned)
			base.TakingDamage (damage);
	}
}
