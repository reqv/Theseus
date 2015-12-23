using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Nietoperz (Bat)</b>
 * </summary>
 * <remarks>
 * 	Klasa nietoperza dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Bat : Monster {

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	public override void Attack()
	{
		// no code here
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	W przypadku nietoperza - ściga on postać gracza przez całą planszę
	/// </remarks>
	public override void Chase()
	{
		WhereIsATarget (_targetToAttack.transform.position);
		_Rig2D.AddForce (_axis * _realMaxSpeed,ForceMode2D.Impulse);
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku nietoperza, stosowany jest kod z metody Chase()
	/// </remarks>
	public override void Walking()
	{
		if (_targetToAttack != null)
			Chase ();
		else {
			_Rig2D.velocity = Vector2.zero;
		}
	}
}
