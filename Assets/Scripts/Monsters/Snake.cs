using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Wąż (Snake)</b>
 * </summary>
 * <remarks>
 * 	Klasa węża dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Snake : Monster {

	[Tooltip ("Prędkość dodawana przy spostrzeżeniu gracza.")]
	[SerializeField]
	/// <summary>
	/// 	Dodatkowe przyśpieszenie przy spostrzeżeniu gracza
	/// </summary>
	private int _chaseAdditionalSpeed;

	[Tooltip ("Zasięg ustalania nowego celu podróży od punktu w którym znajduje się Wąż.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa na jaką odległość może zostać obliczona nowy cel drogi węża.
	/// </summary>
	private int _patrolRange;

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
	/// 	Podczas zderzenia wykonujemy odpowiednią akcję zależna od obiektu, gdy jest nim gracz to rozpoczynamy atak
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
     //   Debug.Log("Attakck!! !! ");
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	W przypadku węża - podąża on za ofiarą z odpowiednio większą prędkością
	/// </remarks>
	public override void Chase()
    {
		if (_realMaxSpeed == _maxSpeed)
			_realMaxSpeed += _chaseAdditionalSpeed;
		if (_realMaxSpeed == _maxSpeed/2)
			_realMaxSpeed += _chaseAdditionalSpeed/2;
		WhereIsATarget (_targetToAttack.transform.position);
        _Rig2D.AddForce(_axis * _realMaxSpeed);
    }

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku węża - otrzymuje on cel podróży i do niego dąży (chyba że trafi na przeszkode). 
	/// 	Następnie po dotarciu na miejsce lub wykryciu kolizji jest ona zmieniana.
	/// </remarks>
	public override void Walking()
    {
		if (_maxSpeed != _realMaxSpeed && _status == Status.OK)
			_realMaxSpeed = _maxSpeed;
		if (_maxSpeed/2 != _realMaxSpeed && _status == Status.Slowed)
			_realMaxSpeed = _maxSpeed/2;
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
}
