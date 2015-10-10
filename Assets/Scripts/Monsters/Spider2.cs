using UnityEngine;
using System.Collections;

public class Spider2 : Monster {

	[Tooltip ("Czas oczekiwania przed rozpoczęciem ataku(w sekundach)")]
	[SerializeField]
	private long _spiderAttackDelay;

	[Tooltip ("Minimalny czas oczekiwania po wykonaniu ataku do wykonania następnego(w sekundach), wartosc musi być wieksza od 0")]
	[SerializeField]
	private long _spiderDelayAfterAttack;

	private bool _readyToAttack = false;
	private double _timeToAttack, _timeToWait;
	private Vector2 _spiderDestination;
	private bool _hasTarget = false;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		_Rig2D = GetComponent<Rigidbody2D>();
		_axis = new Vector2(0, 0);
	}

	public override void Attack()
	{
		//atak

	}

	public override void Chase()
	{
		if (_readyToAttack) {

			if(Time.time >= _timeToAttack)
			{
				Debug.Log ("Atak !");
				if(!_hasTarget)
				{
					_spiderDestination.x = _targetToAttack.position.x;
					_spiderDestination.y = _targetToAttack.position.y;
					_spiderDestination -= _Rig2D.position;
					_Rig2D.velocity = _spiderDestination*_maxSpeed;
					_hasTarget = true;
				}

				if(Time.time >= _timeToWait)
				{
					Debug.Log ("Stop attacking for a while !");
					_hasTarget = false;
					_readyToAttack = false;
					_Rig2D.MovePosition (_Rig2D.position);
				}
			}

		} else {
			Debug.Log ("Szykuje atak !");
			_timeToAttack = Time.time + _spiderAttackDelay;
			_timeToWait = _timeToAttack+_spiderDelayAfterAttack;
			_readyToAttack = true;

		}
	}

	public override void Walking()
	{
		_readyToAttack = false;
		_Rig2D.MovePosition (_Rig2D.position);
	}
}