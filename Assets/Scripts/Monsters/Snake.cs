using UnityEngine;
using System.Collections;

public class Snake : Monster {

    private Transform _targetToAttack;
    private Rigidbody2D _snakeRig2D;
	private Vector2 _axis,_collisionAxis,_freeDestination;	//zmienne odpowiedzialne za pamietanie kierunku poruszania i celu podróży obiektu
	private float _personalMaxSpeed;		// podmiana dla _maxSpeed / ze wzgledu na dodawanie predkosci
	
	[Tooltip ("Prędkość dodawana przy spostrzeżeniu gracza.")]
	[SerializeField]
	private int _chaseAdditionalSpeed;

	override public void Start () {
		base.Start();
		_personalMaxSpeed = _maxSpeed;
        _snakeRig2D = GetComponent<Rigidbody2D>();
        _axis = new Vector2(0, 0);
        _freeDestination = new Vector2(_snakeRig2D.position.x, _snakeRig2D.position.y);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			Destroy(other.gameObject);		// ######################## chwilowe rozwiazanie ( usuwanie gracza ) ########################
		}
		_collisionAxis = _axis;
	}

	void OnCollisionStay2D(Collision2D other)
	{
		if(_collisionAxis.y < 0){
			_freeDestination.y = _snakeRig2D.position.y + _patrolRange;
		}
		else{
			_freeDestination.y = _snakeRig2D.position.y - _patrolRange;
		}
		
		if(_collisionAxis.x < 0){
			_freeDestination.x = _snakeRig2D.position.x + _patrolRange;
		}
		else{
			_freeDestination.x = _snakeRig2D.position.x - _patrolRange;
		}
	}

	void FixedUpdate () {
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

    void Attack()
    {
     //   Debug.Log("Attakck!! !! ");
    }

    void Chase()
    {
		if (_maxSpeed == _personalMaxSpeed)
			_personalMaxSpeed += _chaseAdditionalSpeed;
        _axis = _targetToAttack.position - transform.position;
        if (_axis.x < 0)
            _axis.x = -1;
        else
            _axis.x = 1;
        if (_axis.y < 0)
            _axis.y = -1;
        else
            _axis.y = 1;
        _snakeRig2D.AddForce(_axis * _personalMaxSpeed);
    }

    void Walking()
    {
		if (_maxSpeed != _personalMaxSpeed)
			_personalMaxSpeed = _maxSpeed;
        if (Vector2.Distance(_snakeRig2D.position, _freeDestination) >= 1)
        {
            if (_snakeRig2D.position.x - _freeDestination.x < 0)
                _axis.x = 1;
            else
                _axis.x = -1;
            if (_snakeRig2D.position.y - _freeDestination.y < 0)
                _axis.y = 1;
            else
                _axis.y = -1;
            _snakeRig2D.AddForce(_axis * _personalMaxSpeed);
        }
            else
        {
			_freeDestination = new Vector2(_snakeRig2D.position.x + getRandomNumber(-_patrolRange,_patrolRange),_snakeRig2D.position.y +  getRandomNumber(-_patrolRange,_patrolRange));
        }
    }
}
