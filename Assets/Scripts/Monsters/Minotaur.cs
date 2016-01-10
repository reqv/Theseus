using UnityEngine;
using System.Collections;

public class Minotaur : Monster 
{
    private enum MinotaurAction
    {
        Idle = 0,
        Melee,
        Magic,
        Rocks,
        Minions
    }

    private MinotaurAction _actualAction;
    private GameObject _player;

    private Vector3 _meleeOffset = new Vector3(0.4f, 0.35f, 0);
    private bool _readyToAttack = false;
    private bool _meleePerformed = false;
    private float _chaseTimer = 0;

	void Start () 
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Player");
	}

    public override void FixedUpdate()
    {
        UpdateTimers();
        CheckPersonalStatus();
        if (_status == Status.Stunned)
            return;
        else
            _Anim.SetBool("Stunned", false);

        if(_actualAction == MinotaurAction.Idle)
        {
            _actualAction = (MinotaurAction)RandomNumber(1, 4);

            if (_actualAction == MinotaurAction.Melee)
                _chaseTimer = 0;
        }

        switch(_actualAction)
        {
            case MinotaurAction.Melee:
                Chase();
                break;
            case MinotaurAction.Magic:
                break;
            case MinotaurAction.Rocks:
                break;
            case MinotaurAction.Minions:
                break;
        }
    }

    private void UpdateTimers()
    {
        _chaseTimer += Time.deltaTime;
    }

    public override void Attack()
    {
        _Anim.SetBool("Attacking", true);
        if(_meleePerformed)
        {
            _Anim.SetBool("Stunned", true);
            _Anim.SetBool("Walking", false);
            _Anim.SetBool("Attacking", false);
            _readyToAttack = false;
            _status = Status.Stunned;
            _ccEffectTimer = 1.5f;
            _actualAction = MinotaurAction.Idle;
        }
    }

    public override void Chase()
    {
        Debug.Log(_chaseTimer);
        if(_chaseTimer > 7.0f)
        {
            _actualAction = MinotaurAction.Idle;
            _Anim.SetBool("Walking", false);
            _Rig2D.velocity = Vector2.zero;
            return;
        }

        float distanceR;
        float distanceL;
        float distance = float.MaxValue;

        if (_player != null)
        {
            _targetToAttack = _player.transform;
            distanceR = WhereIsATarget(_targetToAttack.position + _meleeOffset, true);
            distanceL = WhereIsATarget(_targetToAttack.position - _meleeOffset, true);
            if (distanceL < distanceR)
            {
                WhereIsATarget2(_targetToAttack.position - _meleeOffset);
                distance = distanceL;
            }
            else
            {
                WhereIsATarget2(_targetToAttack.position - _meleeOffset);
                distance = distanceR;
            }
        }

        if (_readyToAttack)
        {
            Attack();
            _Rig2D.velocity = Vector2.zero;
        }
        else
        {
            if (_Rig2D.velocity.x > 0 && _axis.x < 0)
                _Rig2D.velocity = Vector2.zero;
            if (_Rig2D.velocity.x < 0 && _axis.x > 0)
                _Rig2D.velocity = Vector2.zero;

            if (_Rig2D.velocity.y > 0 && _axis.y < 0)
                _Rig2D.velocity = Vector2.zero;
            if (_Rig2D.velocity.y < 0 && _axis.y > 0)
                _Rig2D.velocity = Vector2.zero;

            _Rig2D.AddForce(_axis * _realMaxSpeed, ForceMode2D.Impulse);
            _Anim.SetBool("Walking", true);
        }

        if (_facingRight && CheckAxis() < 0)
            Flip();
        if (!_facingRight && CheckAxis() > 0)
            Flip();
    }

    public override void Walking()
    {
    }

    public override void SetCrowdControl(Status status, float time)
    {
        return;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            _readyToAttack = true;
            if (_meleePerformed)
            {
                other.gameObject.GetComponent<Character>().TakingDamage(_attackPower);
            }
        }
    }

    public void MeleePerformed()
    {
        _meleePerformed = true;
    }

    protected void WhereIsATarget2(Vector2 target)
    {
        _axis.x = target.x - transform.position.x;
        _axis.y = target.y - transform.position.y;

        _axis.Normalize();
    }
}
