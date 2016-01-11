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

    #region
    [SerializeField]
    private Projectile[] _magicProjectiles;
    #endregion
    private MinotaurAction _actualAction;
    private GameObject _player;

    private Vector3 _meleeOffsetR = new Vector3(0.4f, 0.35f, 0);
    private Vector3 _meleeOffsetL = new Vector3(-0.4f, 0.35f, 0);
    private bool _readyToAttack = false;
    private bool _meleePerformed = false;
    private float _actionTimer = 0;

    private float _magicDelayTimer = 0;
    private bool _magicDirecion;
    [SerializeField]
    private float _magicDelay;

	void Start () 
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Player");
        _targetToAttack = _player.transform;
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
            _actualAction = (MinotaurAction)RandomNumber(1, 2);
                _actionTimer = 0;
        }

        switch(_actualAction)
        {
            case MinotaurAction.Melee:
                Chase();
                break;
            case MinotaurAction.Magic:
                MagicAttack();
                break;
            case MinotaurAction.Rocks:
                break;
            case MinotaurAction.Minions:
                break;
        }
    }

    private void UpdateTimers()
    {
        _actionTimer += Time.deltaTime;
        _magicDelayTimer += Time.deltaTime;
    }

    public override void Walking()
    {
    }

    public override void SetCrowdControl(Status status, float time)
    {
        return;
    }

    #region Action Melee
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
        if(_actionTimer > 7.0f)
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
            distanceR = WhereIsATarget(_targetToAttack.position + _meleeOffsetR, true);
            distanceL = WhereIsATarget(_targetToAttack.position + _meleeOffsetL, true);
            if (distanceL < distanceR)
            {
                WhereIsATarget2(_targetToAttack.position + _meleeOffsetL);
                distance = distanceL;
            }
            else
            {
                WhereIsATarget2(_targetToAttack.position + _meleeOffsetR);
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
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
    #endregion

    #region Action Magic
    private void MagicAttack()
    {
        if (_actionTimer > 4)
        {
            _actualAction = MinotaurAction.Idle;
        }

        if (_magicDelayTimer > _magicDelay)
        {
            _magicDelayTimer = 0;
            if (_magicDirecion)
            {
                NewProjectile2(_magicProjectiles[0].gameObject, new Vector2(0, 0.2f), Vector2.up * _bulletVelocityFactor);
                NewProjectile2(_magicProjectiles[1].gameObject, new Vector2(0.2f, 0), Vector2.right * _bulletVelocityFactor);
                NewProjectile2(_magicProjectiles[2].gameObject, new Vector2(-0.2f, 0), Vector2.left * _bulletVelocityFactor);
                NewProjectile2(_magicProjectiles[3].gameObject, new Vector2(0, -0.2f), Vector2.down * _bulletVelocityFactor);
            }
            else
            {
                NewProjectile2(_magicProjectiles[0].gameObject, new Vector2(0, 0.2f), (Vector2.up + Vector2.right) * _bulletVelocityFactor);
                NewProjectile2(_magicProjectiles[1].gameObject, new Vector2(0.2f, 0), (Vector2.down + Vector2.right) * _bulletVelocityFactor);
                NewProjectile2(_magicProjectiles[2].gameObject, new Vector2(-0.2f, 0), (Vector2.up + Vector2.left) * _bulletVelocityFactor);
                NewProjectile2(_magicProjectiles[3].gameObject, new Vector2(0, -0.2f), (Vector2.down + Vector2.left) * _bulletVelocityFactor);
            }

            _magicDirecion = !_magicDirecion;
        }
    }

    protected void NewProjectile2(GameObject projectile, Vector2 offset, Vector2 velocity, float rotate = 0)
    {
        var bullet = Instantiate(projectile, new Vector2(this.transform.position.x + offset.x, this.transform.position.y + offset.y), Quaternion.Euler(new Vector3(0, 0, rotate))) as GameObject;
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
        bullet.GetComponent<Projectile>().Damage = _attackPower;
    }
    #endregion
}
