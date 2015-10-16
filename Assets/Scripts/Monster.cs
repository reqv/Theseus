using UnityEngine;
using System.Collections;

/**
 * <summary>
 * 	Abstrakcyjna klasa dla potworów
 * </summary>
 * <remarks>
 * 	Klasa rodzica dla wszystkich przeciwników w grze, zawiera podstawową implementacje każdego potwora.
 * </remarks>
 */
public abstract class Monster : MonoBehaviour {

	/// <summary>
	/// 	Parametr trzymajacy aktualna pozycje gracza
	/// </summary>
	protected Transform _targetToAttack;	// Pozycja gracza

	/// <summary>
	/// 	Obiekt fizycznego ciała 'nvidia physx' obiektu.
	/// </summary>
	protected Rigidbody2D _Rig2D;

	/// <summary>
	/// 	Parametr trzymający aktualny kierunek poruszania sie potwora
	/// </summary>
	protected Vector2 _axis;

	/// <summary>
	/// 	Parametr trzymający kierunek poruszania się stwora podczas wykrycia kolizji 
	/// </summary>
	protected Vector2 _collisionAxis;

	/// <summary>
	/// 	Parametr trzumający pozycję do której zmierza stwór
	/// </summary>
	protected Vector2 _freeDestination;


	[Tooltip ("Maksymalne przyspieszenie obiektu.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa maksymalną szybkość obiektu
	/// </summary>
	protected float _maxSpeed;

	[Tooltip ("Zasięg spostrzeżenia gracza przez obiekt.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa zasięg widzenia innych obiektów przez dany obiekt
	/// </summary>
	protected float _range;

	[Tooltip ("Zasięg od którego obiekt rozpoczyna atak.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa maksymalną szybkość obiektu
	/// </summary>
	protected float _attackDistance;

	[Tooltip ("Zasięg ustalania nowego celu podróży od punktu w którym znajduje się obiekt.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa na jaką odległość może wyruszyć obiekt ( liczone od aktualnej pozycji obiektu )
	/// </summary>
	protected int _patrolRange;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu
	/// </summary>
	public virtual void Start () {
	
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas każdej jednostki czasu.
	/// </summary>
	public virtual void Update () {
		
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas każdej jednostki czasu i zawsze w stałym momencie. Służy do dokonywania obliczeń między obiektami. 
	/// </summary> 
	/// <remarks>
	/// 	Wynikiem końcowym metody jest uruchomienie jednej z trzech akcji: Attack(), Chase() lub Walking() zależnej 
	/// 	od odległości między obiektami.
	/// </remarks>
	public virtual void FixedUpdate () {
		float distance = float.MaxValue;
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			_targetToAttack = GameObject.FindGameObjectWithTag("Player").transform;
			distance = Vector2.Distance(transform.position,_targetToAttack.position);
		}
		if (distance <= _attackDistance)
			Attack();
		else if (distance <= _range)
			Chase();
		else
			Walking();
	}

	/// <summary>
	/// 	Abstrakcja ataku
	/// </summary>
	public abstract void Attack();

	/// <summary>
	/// 	Abstrakcja gonitwy za obiektem który został zauważony przez ten obiekt
	/// </summary>
	public abstract void Chase();

	/// <summary>
	/// 	Abstrakcja swobodnego zachowania obiektu, gdy szukany inny obiekt nie jest w zasięgu
	/// </summary>
	public abstract void Walking();

	/// <summary>
	/// 	Metoda pozwalająca na losowanie liczb całkowitych
	/// </summary>
	/// <param name="min">Minimalna wartość losowana</param>
	/// <param name="min">Maksymalna wartość losowana</param>
	/// <returns>Liczba całkowita z danego przedziału</returns>
	protected int getRandomNumber(int min,int max){
		int myNumber = Random.Range(min, max);
		return myNumber;
	}
}