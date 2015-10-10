using UnityEngine;
using System.Collections;

public abstract class Monster : MonoBehaviour {

	protected Transform _targetToAttack;	// Pozycja gracza
	protected Rigidbody2D _Rig2D;
	protected Vector2 _axis,_collisionAxis,_freeDestination;	//zmienne odpowiedzialne za pamietanie kierunku poruszania i celu podróży obiektu

	[Tooltip ("Maksymalne przyspieszenie obiektu.")]
	[SerializeField]
	protected float _maxSpeed;

	[Tooltip ("Zasięg spostrzeżenia gracza przez obiekt.")]
	[SerializeField]
	protected float _range;

	[Tooltip ("Zasięg od którego obiekt rozpoczyna atak.")]
	[SerializeField]
	protected float _attackDistance;

	[Tooltip ("Zasięg ustalania nowego celu podróży od punktu w którym znajduje się obiekt.")]
	[SerializeField]
	protected int _patrolRange;
	
	public virtual void Start () {
	
	}
	
	public virtual void Update () {
		
	}

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

	public abstract void Attack();
	public abstract void Chase();
	public abstract void Walking();

	// UNITY STYLE RANDOMIZER
	protected int getRandomNumber(int min,int max){
		int myNumber = Random.Range(min, max);
		return myNumber;
	}
}