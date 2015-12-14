using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Spartan</b>
 * </summary>
 * <remarks>
 * 	Klasa Centaura dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Spartan : Monster {

	[Tooltip ("Wartość na osi X do której porusza się Spartan patrolując teren.")]
	[SerializeField]
	/// <summary>
	/// 	Wartość na osi X do której porusza się Spartan patrolując teren.
	/// </summary>
	private int _patrolX;

	[Tooltip ("Wartość na osi Y do której porusza się Spartan patrolując teren.")]
	[SerializeField]
	/// <summary>
	/// 	Wartość na osi Y do której porusza się Spartan patrolując teren.
	/// </summary>
	private int _patrolY;

	[Tooltip ("Odległość od obiektu gracza po osiągnięciu której Spartan zacznie ucieczkę.")]
	[SerializeField]
	/// <summary>
	/// 	Odległość od obiektu gracza po osiągnięciu której Spartan zacznie ucieczkę.
	/// </summary>
	private int _runAwayRange;

	[Tooltip ("Obiekt włóczni")]
	[SerializeField]
	/// <summary>
	/// 	Obiekt włóczni
	/// </summary>
	private GameObject _arrow;

	/// <summary>
	/// 	Wektor zawierający informacje o początkowym położeniu Spartan.
	/// </summary>
	private Vector2 _startingPoint;

	/// <summary>
	/// 	Wektor trzymający miejsce do którego Spartan patroluje teren.
	/// </summary>
	private Vector2 _patrolToPoint;

	/// <summary>
	/// 	Czas pomiędzy atakami
	/// </summary>
	private double timewhenattack = 0;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	public override void Start () {
		base.Start ();
		_flipRate = 0.8f;
		_startingPoint = (Vector2)transform.position;
		_freeDestination = _startingPoint;
		_patrolToPoint = new Vector2 (_startingPoint.x + _patrolX, _startingPoint.y + _patrolY);
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	/// <remarks>
	/// 	W przypadku Spartan oprócz ataku z dystansu za pomocą strzał, gdy gracz zbliży się zbyt blisko, zacznie on uciekać.
	/// </remarks>
	public override void Attack()
	{
		if (WhereIsATarget (_targetToAttack.position, true) < _runAwayRange) {
			WhereIsATarget(_targetToAttack.position);
			_Rig2D.AddForce(_axis*-1 * _realMaxSpeed);
		} else {
			if(!_facingRight && _axis.x > 0)
				Flip();
			if(_facingRight && _axis.x < 0)
				Flip();
		}
		if (timewhenattack <= 0) {
			// Hurt player
			Vector2 vector = transform.position - _targetToAttack.position;
			if(vector.x > 0)
				NewProjectile(_arrow,new Vector2(-6,0),new Vector2(-vector.x+6,-vector.y) * 2);
			else
				NewProjectile(_arrow,new Vector2(6,0),new Vector2(-vector.x-6,-vector.y) * 2);
			timewhenattack = 2;
		} else
			timewhenattack -= Time.deltaTime;
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Spartan zbliża się do gracza w celu oddania celnego rzutu.
	/// </remarks>
	public override void Chase()
	{
		Vector2 target;
		target.x = _targetToAttack.position.x;
		target.y = _targetToAttack.position.y;
		WhereIsATarget (target);
		_Rig2D.AddForce (_axis * _realMaxSpeed);
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	Spartan patroluje wskazany wcześniej teren.
	/// </remarks>
	public override void Walking()
	{
		if ((int)_freeDestination.x == (int)transform.position.x && (int)_freeDestination.y == (int)transform.position.y) 
			if (_freeDestination == _startingPoint)
				_freeDestination = _patrolToPoint;
			else
			_freeDestination = _startingPoint;
		WhereIsATarget (_freeDestination);
		_Rig2D.AddForce (_axis * (_realMaxSpeed/3));
	}
}