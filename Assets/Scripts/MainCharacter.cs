using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour 
{
	/// <summary>
	/// Współczynnik prędkości ruchu gracza
	/// </summary>
	#region Serialized Fields
	[SerializeField]
	private float _moveVelocityFactor;
	[SerializeField]
	private float _throwDelay;
	[SerializeField]
	private float _fireballVelocity;
	[SerializeField]
	private GameObject _fireball;
	#endregion

	private float _actualThrowDelay = 0;
	private Rigidbody2D _rigidbody2D;
	private Animator _animator;

	void Start () 
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}
	
	void Update () 
	{
		var xAxis = Input.GetAxis ("Horizontal");
		var yAxis = Input.GetAxis ("Vertical");

		_rigidbody2D.velocity = new Vector2(xAxis * _moveVelocityFactor, yAxis * _moveVelocityFactor);

		UpdateAnimator(xAxis, yAxis);
		CheckThrow();
	}

	private void UpdateAnimator(float xAxis, float yAxis)
	{
		if (Mathf.Abs(xAxis) > 0
			|| Mathf.Abs(yAxis) > 0)
		{
			_animator.SetBool("IsWalking", true);
		}
		else
			_animator.SetBool("IsWalking", false);

		if (_rigidbody2D.velocity.x > 0)
		{
			_animator.SetInteger("Direction", 1);
		}
		else if (_rigidbody2D.velocity.x < 0)
		{
			_animator.SetInteger("Direction", 3);
		}

		if (_rigidbody2D.velocity.y > 0)
		{
			_animator.SetInteger("Direction", 0);
		}
		else if (_rigidbody2D.velocity.y < 0)
		{
			_animator.SetInteger("Direction", 2);
		}

	}

	private void CheckThrow()
	{
		_actualThrowDelay += Time.deltaTime;
		if (_actualThrowDelay > _throwDelay)
		{
			if (Input.GetKey(KeyCode.W))
			{
				ThrowFireball(new Vector3(0, 5), new Vector2(0, _fireballVelocity)
					+ new Vector2(_rigidbody2D.velocity.x, 0));
				_actualThrowDelay = 0;
				return;
			}
			if (Input.GetKey(KeyCode.S))
			{
				ThrowFireball(new Vector3(0, -5), new Vector2(0, -_fireballVelocity)
					+ new Vector2(_rigidbody2D.velocity.x, 0));
				_actualThrowDelay = 0;
				return;
			}
			if (Input.GetKey(KeyCode.A))
			{
				ThrowFireball(new Vector3(-5, 0), new Vector2(-_fireballVelocity, 0)
					+ new Vector2(0, _rigidbody2D.velocity.y));
				_actualThrowDelay = 0;
				return;
			}
			if (Input.GetKey(KeyCode.D))
			{
				ThrowFireball(new Vector3(5, 0), new Vector2(_fireballVelocity, 0)
					+ new Vector2(0, _rigidbody2D.velocity.y));
				_actualThrowDelay = 0;
				return;
			}

		}
	}

	private void ThrowFireball(Vector3 offset, Vector2 velocity)
	{
		var fireball = Instantiate(_fireball);
		fireball.transform.position = this.transform.position + offset;
		fireball.GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
