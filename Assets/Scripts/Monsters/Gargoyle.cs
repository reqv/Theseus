using UnityEngine;

/**
 * <summary>
 * 	Klasa potwora:<b> Gargulec (Gargoyle)</b>
 * </summary>
 * <remarks>
 * 	Klasa gargulca dziedzicząca po klasie Monster, służy do ustalenia konkretnych zachowań potwora.
 * </remarks>
 */
public class Gargoyle : Monster {

	[Tooltip ("Czy gargulec jest żywy?")]
	[SerializeField]
	/// <summary>
	/// 	Parametr decydujący o tym, czy gargulec jest istotą żywą.
	/// </summary>
	private bool _isAlive;

	[Tooltip ("Czy gargulec w postaci kamiennej powinien patrzeć w lewo?")]
	[SerializeField]
	/// <summary>
	/// 	Parametr mówiący o tym w którą strone patrzy gargulec w postaci kamienia.
	/// </summary>
	private bool _lookAtLeft;

	/// <summary>
	/// 	Zmienna sprawdzająca, czy gargulec spostrzegł ofiarę.
	/// </summary>
	private bool _locateATarget = false;
	
	/// <summary>
	/// 	Zmienna trzymająca czas w jakim następuje uwolnienie gargulca.
	/// </summary>
	private float _timeToRelease;

	/// <summary>
	/// 	Początkowa pozycja gargulca.
	/// </summary>
	private Vector2 _startingPosition;
	
	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu, pozwala na inicjalizację zmiennych klasy.
	/// </summary>
	override public void Start () {
		base.Start();
		_timeToRelease = 2;
		_startingPosition = (Vector2)transform.position;
		if (_lookAtLeft)
			Flip ();
	}

	/// <summary>
	/// 	Zaimplementowana metoda atakujaca szukany obiekt
	/// </summary>
	public override void Attack()
	{
		//   Debug.Log("Attack!! !! ");
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na ściganie obiektu
	/// </summary>
	/// <remarks>
	/// 	Gargulec pozostaje w spoczynku do czasu w którym odnajdzie obiekt gracza. Po uwolnieniu ściga swój obiekt aż do śmierci swojej lub ofiary.
	/// </remarks>
	public override void Chase()
	{
		if (_timeToRelease < 0) {
			this.gameObject.layer = 9;
			WhereIsATarget (_targetToAttack.transform.position);
			_Rig2D.AddForce (_axis * _realMaxSpeed);
		} else {
			if(_locateATarget)
			{
				_timeToRelease -= Time.deltaTime;
			}else{
				_locateATarget = true;
			}
		}
	}
	
	/// <summary>
	/// 	Zaimplementowana metoda pozwalająca na swobodne poruszanie się gdy w pobliżu nie ma szukanego obiektu.
	/// </summary>
	/// <remarks>
	/// 	W przypadku gargulca, jeżeli został uwolniony i jest istotą żywą to wykonywany jest kod z metody Chase(), inaczej nie ma nic do roboty i pilnuje wyznaczonego miejsca.
	/// </remarks>
	public override void Walking()
	{
		if (_targetToAttack != null && _locateATarget && _isAlive)
			Chase ();
		else {
			if(_timeToRelease < 0)
			{
				_Rig2D.MovePosition(_startingPosition);
				this.gameObject.layer = 8;
				_timeToRelease = 2;
				_locateATarget = false;

				if(!_facingRight && !_lookAtLeft)
					Flip();
				if(_facingRight && _lookAtLeft)
					Flip();
			}
				else
					_Rig2D.velocity = Vector2.zero;
		}
	}
}
