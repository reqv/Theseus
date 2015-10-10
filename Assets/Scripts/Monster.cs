using UnityEngine;
using System.Collections;

public abstract class Monster : MonoBehaviour {

	[Tooltip ("Maksymalne przyspieszenie obiektu.")]
	[SerializeField]
	protected float _maxSpeed;

	[Tooltip ("Zasięg spostrzeżenia gracza przez obiekt.")]
	[SerializeField]
	protected float _range;

	[Tooltip ("Zasięg od którego obiekt rozpoczyna atak.")]
	[SerializeField]
	protected float _attackDistance;	// zasieg od ktorego rozpoczyna atak

	[Tooltip ("Zasięg ustalania nowego celu podróży od punktu w którym znajduje się obiekt.")]
	[SerializeField]
	protected int _patrolRange;	// zasieg patrolu potwora

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

	// UNITY STYLE RANDOMIZER
	protected int getRandomNumber(int min,int max){
		int myNumber = Random.Range(min, max);
		return myNumber;
	}
}