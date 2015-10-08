using UnityEngine;
using System.Collections;

public abstract class Monster : MonoBehaviour {

	public float maxSpeed;	// maksymalna predkosc potwora
	public float range;		// zasieg w jakim widzi postac gracza
	public float attackDistance;	// zasieg od ktorego rozpoczyna atak
	public int patrolRange;	// zasieg patrolu potwora

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

	// UNITY STYLE RANDOMIZER
	protected int getRandomNumber(int min,int max)
	{
		int myNumber = Random.Range (min, max);
		return myNumber;
	}
}