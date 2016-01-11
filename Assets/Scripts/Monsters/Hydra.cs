using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Hydra (Hydra)</b>
 * </summary>
 * <remarks>
 * 	Klasa Hydry dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Hydra : Monster {

	/// <summary>
	/// 	Parametr określający, czy Hydra znajduje w stanie większej mocy.
	/// </summary>
	private bool _strongerMode = false;

	/// <summary>
	/// 	Parametr trzymający początkowy stan życia Hydry.
	/// </summary>
	private int _health;

	[Tooltip ("Zegar sprawdzający, czy Hydra powinna użyć leczenia. ( co ile sekund leczy się Hydra)")]
	[SerializeField]
	/// <summary>
	/// 	Zegar sprawdzający, czy Hydra powinna użyć leczenia.
	/// </summary>
	private byte _timeToHeal;

	/// <summary>
	/// 	Parametr określający, czy Hydra znajduje w stanie większej mocy.
	/// </summary>
	private float _healTimer;

	[Tooltip ("Wartość odnawianego co jakiś czas zdrowia.")]
	[SerializeField]
	/// <summary>
	/// 	Wartość odnawianego co jakiś czas zdrowia.
	/// </summary>
	private byte _healOverTime;


	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
		_health = _healthPoints;
	}


	/// <summary>
	/// 	Metoda sprawdzająca, czy gracz wszedł na Hydrę. W stanie większej mocy spowalnia gracza.
	/// </summary>
	/// <param name="other">Obiekt z którym odbyło się zderzenie</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "player" && _strongerMode) {
			var skrypt = other.gameObject.GetComponent<MainCharacter> ();
			skrypt.SetCrowdControl (Status.Slowed, 2);
		}
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
	/// 	Hydra goni za graczem po całym pomieszczeniu.
	/// </remarks>
	public override void Chase()
    {
		_Anim.SetBool ("Walking",true);
		WhereIsATarget (_targetToAttack.transform.position);
		_Rig2D.AddForce (_axis * _realMaxSpeed);
    }

	/// <summary>
	/// 	Nadpisana metoda FixedUpdate()
	/// </summary>
	/// <remarks>
	/// 	Hydra regeneruje się co określoną jednostkę czasu. Leczenie jest mocniejsze w stanie większej mocy.
	/// </remarks>
	public override void FixedUpdate ()
	{
		base.FixedUpdate ();
		if (_healTimer < 0) {
			if(_healthPoints < _health)
				_healthPoints += _healOverTime;
			_healTimer = _timeToHeal;
		} else
			_healTimer -= Time.deltaTime;
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku Hydry, nieustannie goni ona ofiarę.
	/// </remarks>
	public override void Walking()
    {
		if (_targetToAttack != null)
			Chase ();
		else
			_Anim.SetBool ("Walking",false);
	}


	/// <summary>
	/// 	Nadpisana metoda TakingDamage(int damage)
	/// </summary>
	/// <remarks>
	/// 	Hydra po pierwszym pokonaniu nie umiera lecz zyskuje nowe i większe pokłady sił.
	/// </remarks>
	/// <param name="damage">Wartość otrzymanych obrażeń</param>
	public override void TakingDamage(int damage)
	{
		_healthPoints -= damage;
		if (_healthPoints <= 0)
		if (_strongerMode)
			Die ();
		else {
			_health *= 2;
			_healOverTime *= 2;
			_healthPoints = _health;
			SetCrowdControl (Status.Stunned, 1);
			_Anim.SetTrigger ("Stronger");
			_strongerMode = true;
		}
	}
}