using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Wojownik (Warrior)</b>
 * </summary>
 * <remarks>
 * 	Klasa wojownika dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class SmallBloob : Monster {

	override public void Start () {
		base.Start();
	}
		
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			Destroy(other.gameObject);		// ######################## chwilowe rozwiazanie ( usuwanie gracza ) ########################
		}
		_collisionAxis = _axis;
	}


	public override void Attack()
	{
		
	}


	public override void Chase()
	{
		WhereIsATarget (_targetToAttack.transform.position);
		_Rig2D.AddForce (_axis * _realMaxSpeed);
	}

	public override void Walking()
	{
		if (_targetToAttack != null)
			Chase ();
		else 
			Die ();
	}
}
