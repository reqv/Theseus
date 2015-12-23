using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Spartan (Spartanin)</b>
 * </summary>
 * <remarks>
 * 	Klasa Spartanina dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Spartan : Monster {

	[Tooltip ("Wartość na osi X do której porusza się Spartanin patrolując teren.")]
	[SerializeField]
	/// <summary>
	/// 	Wartość na osi X do której porusza się Spartanin patrolując teren.
	/// </summary>
	private int _patrolX;

	[Tooltip ("Wartość na osi Y do której porusza się Spartanin patrolując teren.")]
	[SerializeField]
	/// <summary>
	/// 	Wartość na osi Y do której porusza się Spartanin patrolując teren.
	/// </summary>
	private int _patrolY;

	[Tooltip ("Odległość od obiektu gracza po osiągnięciu której Spartanin zacznie ucieczkę.")]
	[SerializeField]
	/// <summary>
	/// 	Odległość od obiektu gracza po osiągnięciu której Spartanin zacznie ucieczkę.
	/// </summary>
	private int _runAwayRange;

	[Tooltip ("Obiekt włóczni Spartanina")]
	[SerializeField]
	/// <summary>
	/// 	Obiekt włóczni Spartanina
	/// </summary>
	private GameObject _spear;

	/// <summary>
	/// 	Wektor zawierający informacje o początkowym położeniu Spartanina.
	/// </summary>
	private Vector2 _startingPoint;

	/// <summary>
	/// 	Wektor trzymający miejsce do którego Spartanin patroluje teren.
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
		_spear.GetComponent<EnemyProjectile>().Damage = _attackPower;
		_startingPoint = (Vector2)transform.position;
		_freeDestination = _startingPoint;
		_patrolToPoint = new Vector2 (_startingPoint.x + _patrolX, _startingPoint.y + _patrolY);
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	/// <remarks>
	/// 	W przypadku Spartanina oprócz ataku z dystansu za pomocą włóczni, gdy gracz zbliży się zbyt blisko, zacznie się on cofać.
	/// </remarks>
	public override void Attack()
	{
		_Anim.SetBool ("Walking",false);
		if (WhereIsATarget (_targetToAttack.position, true) < _runAwayRange) {
			_Anim.SetBool ("Walking",true);
			WhereIsATarget(_targetToAttack.position);
			_Rig2D.AddForce(_axis*-1 * _realMaxSpeed);
		} else {
			if(!_facingRight && _axis.x > 0)
				Flip();
			if(_facingRight && _axis.x < 0)
				Flip();
		}
		if (timewhenattack <= 0) {
			_Anim.SetTrigger("Attack");
			Vector2 vector = getRightAxis(transform.position,_targetToAttack.position);
			NewProjectile(_spear,new Vector2(0,0),new Vector2(-vector.x,-vector.y) * 2,VecToTan(transform.position,_targetToAttack.position,90f));
			timewhenattack = 2;
		} else
			timewhenattack -= Time.deltaTime;
	}

	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Spartanin zbliża się do gracza w celu oddania celnego rzutu włócznią.
	/// </remarks>
	public override void Chase()
	{
		_Anim.SetBool ("Walking", true);
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
	/// 	Spartanin patroluje wskazany wcześniej teren lub pilnuje wskazanej pozycji.
	/// </remarks>
	public override void Walking()
	{
		if(_patrolX == 0 && _patrolY == 0)
			_Anim.SetBool ("Walking", false);
		else
			_Anim.SetBool ("Walking", true);

		if ((int)_freeDestination.x == (int)transform.position.x && (int)_freeDestination.y == (int)transform.position.y) 
		if (_freeDestination == _startingPoint)
			_freeDestination = _patrolToPoint;
		else
			_freeDestination = _startingPoint;
		WhereIsATarget (_freeDestination);
		_Rig2D.AddForce (_axis * (_realMaxSpeed/3));
	}
}