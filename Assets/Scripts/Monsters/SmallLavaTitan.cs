using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Małego Tytana Lawy (Small Lava Titan)</b>
 * </summary>
 * <remarks>
 * 	Klasa Małego Tytana Lawy dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora. Potwór powstaje po pokonaniu normalnego Tytana Lawy.
 * </remarks>
 */
public class SmallLavaTitan : Monster {

	override public void Start () {
		base.Start();
		_Anim.SetBool ("Walking", true);
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
	/// 	W przypadku Miniona Lawy - podąża on za ofiarą w celu jej złapania
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
	/// 	W przypadku Miniona Lawy dostępne opcje to ściganie gracza lub śmierć.
	/// </remarks>
	public override void Walking()
	{
		if (_targetToAttack != null)
			Chase ();
		else 
			Die ();
	}
}
