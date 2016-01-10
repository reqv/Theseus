using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Tytan Lawy (Lava Titan)</b>
 * </summary>
 * <remarks>
 * 	Klasa tytana dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class LavaTitan : Monster {

	[Tooltip ("Zasięg ustalania nowego celu podróży od punktu w którym znajduje się Tytan.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa na jaką odległość może zostać obliczony nowy cel drogi Tytana.
	/// </summary>
	private int _patrolRange;

	[Tooltip ("Współczynnik prędkości pocisku potwora.")]
	[SerializeField]
	/// <summary>
	/// 	Współczynnik prędkości pocisku potwora.
	/// </summary>
	private int _projectileSpeed;

	[Tooltip ("Obiekt wyrzucany przez potwora podczas ataku.")]
	[SerializeField]
	/// <summary>
	/// 	Obiekt wyrzucany przez potwora podczas ataku.
	/// </summary>
	private GameObject _magic;

	[Tooltip ("Obiekt minionów powstałych po zabójstwie potwora.")]
	[SerializeField]
	/// <summary>
	/// 	Obiekt minionów powstałych po zabójstwie potwora.
	/// </summary>
	private GameObject _minion;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
		_freeDestination = new Vector2(_Rig2D.position.x, _Rig2D.position.y);
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas wykrycia zderzenia obiektu z innym obiektem
	/// </summary>
	/// <remarks>
	/// 	Podczas zderzenia wykonujemy odpowiednią akcję zależna od obiektu
	/// </remarks>
	/// <param name="other">Obiekt z którym odbyło się zderzenie</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		_collisionAxis = _axis;
	}

	/// <summary>
	/// 	Metoda uruchamiana po wcześniejszym wykryciu kolizji z której nie zdążyliśmy wyjść przed upłynięciem jednostki czasu
	/// </summary>
	/// <remarks>
	/// 	Podczas utrzymywania się kolizji ze ścianami, obiekt jest zmuszony do zmiany kierunku w którym się porusza
	/// </remarks>
	/// <param name="other">Obiekt z którym odbyło się zderzenie</param>
	void OnCollisionStay2D(Collision2D other)
	{
		if(_collisionAxis.y < 0){
			_freeDestination.y = _Rig2D.position.y + _patrolRange;
		}
		else{
			_freeDestination.y = _Rig2D.position.y - _patrolRange;
		}

		if(_collisionAxis.x < 0){
			_freeDestination.x = _Rig2D.position.x + _patrolRange;
		}
		else{
			_freeDestination.x = _Rig2D.position.x - _patrolRange;
		}
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	public override void Attack()
	{
		_Anim.SetBool ("Walking", false);
		Vector2 vector = transform.position - _targetToAttack.position;
		NewProjectile (_magic, new Vector2 (0, 0), new Vector2 (-vector.x, -vector.y) * _projectileSpeed);
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	W przypadku Lawowego Tytana, gdy zobaczy gracza zaczyna zmierzać w jego kierunku, a następnie miota w niego licznymi pociskami.
	/// </remarks>
	public override void Chase()
	{
		_Anim.SetBool ("Walking", true);
		WhereIsATarget (_targetToAttack.transform.position);
		_Rig2D.AddForce(_axis * _realMaxSpeed);
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku Tytana Lawy - otrzymuje on losowy cel podróży i do niego dąży (chyba że trafi na przeszkodę). 
	/// 	Następnie po dotarciu na miejsce lub wykryciu kolizji jest ona zmieniana.
	/// </remarks>
	public override void Walking()
	{
		_Anim.SetBool ("Walking", true);
		if (WhereIsATarget(_freeDestination,true) >= 1)
		{
			WhereIsATarget(_freeDestination);
			_Rig2D.AddForce(_axis * _realMaxSpeed);
		}
		else
		{
			_freeDestination = new Vector2(_Rig2D.position.x + RandomNumber(-_patrolRange,_patrolRange),_Rig2D.position.y +  RandomNumber(-_patrolRange,_patrolRange));
		}
	}

	/// <summary>
	/// 	Nadpisana metoda klasy Monster służąca do zadawania obrażeń stworowi.
	/// </summary>
	/// <remarks>
	/// 	W przypadku Tytana Lawy, gdy otrzyma wystarczającą ilość obrażeń i umiera uruchamiana jest również metoda SpawnMinions.
	/// </remarks>
	/// <param name="damage">Ilość otrzymanych obrażeń</param>
	public override void TakingDamage(int damage)
	{
		_healthPoints -= damage;
		if (_healthPoints <= 0) {
			SpawnMinions ();
			Die ();
		}
	}

	/// <summary>
	/// 	Funkcja pozwalająca potworowi na przyzwanie pomocników.
	/// </summary>
	/// <remarks>
	/// 	W przypadku Tytana po zgonie przyzywa on 4 pomniejsze potwory.
	/// </remarks>
	private void SpawnMinions()
	{
		
		if (_status == Status.Stunned)
			return;
		var M1 = Instantiate(_minion);
		M1.transform.position = new Vector2 (this.transform.position.x + 0.5f, this.transform.position.y);

		var M2 = Instantiate(_minion);
		M2.transform.position = new Vector2 (this.transform.position.x - 0.5f, this.transform.position.y);

		var M3 = Instantiate(_minion);
		M3.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y + 0.5f);

		var M4 = Instantiate(_minion);
		M4.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y - 0.5f);
	}
}