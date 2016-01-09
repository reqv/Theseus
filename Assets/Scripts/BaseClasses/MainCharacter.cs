using UnityEngine;
using System.Collections;

/**
 * <summary>
 * 	Klasa kontrolująca główną postać.
 * </summary>
 * <remarks>
 * 	Odpowiada za przyjmowanie sterowania jako wejścia i na tej podstawie kontrolowanie postacią główną w świecie gry.
 * </remarks>
 */
[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : Character
{
    #region Serialized Fields
    [Tooltip("Współczynnik prędkości ruchu gracza")]
    [SerializeField]
    /// <summary>
    /// Współczynnik prędkości ruchu gracza
    /// </summary>
    private float _moveVelocityFactor;
    [Tooltip("Odstęp czasu między kolejnymi atakami")]
    [SerializeField]
    /// <summary>
    /// Odstęp czasu między kolejnymi atakami
    /// </summary>
    private float _throwDelay;
    [Tooltip("Prędkość fireball'a rzucanego przez gracza")]
    [SerializeField]
    /// <summary>
    /// Prędkość fireball'a rzucanego przez gracza
    /// </summary>
    private float _fireballVelocity;
    [Tooltip("Prefab fireball'a")]
    [SerializeField]
    /// <summary>
    /// WPrefab fireball'a
    /// </summary>
	private GameObject _fireball;
	#endregion

    /// <summary>
    /// Aktualny czas który minął od ostatniego ataku
    /// </summary>
	private float _actualThrowDelay = 0;
    /// <summary>
    /// Komponent Animator postaci
    /// </summary>
	private Animator _animator;

    /// <summary>
    /// Ilość monet którą posiada gracz
    /// </summary>
    public int Coins { get; set; }

    /// <summary>
    /// 	Metoda uruchamiana podczas utworzenia obiektu
    /// </summary>
	void Start () 
	{
		_Rig2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();

        Coins = 10;
        Messenger.AddListener<Direction>(Messages.PlayerGoesThroughTheDoor, OnRoomChange);
	}

    /// <summary>
    /// 	Metoda uruchamiana podczas każdej jednostki czasu.
    /// </summary>
	void Update () 
	{
		var xAxis = Input.GetAxis ("Horizontal");
		var yAxis = Input.GetAxis ("Vertical");

		_Rig2D.velocity = new Vector2(xAxis * _moveVelocityFactor, yAxis * _moveVelocityFactor);

		UpdateAnimator(xAxis, yAxis);
		CheckThrow();
	}

    /// <summary>
    /// Metoda kontrolująca animacje postaci gracza na podstawie jej ruchu w osiach x i y
    /// </summary>
    /// <param name="xAxis">Ruch postaci w osi x</param>
    /// <param name="yAxis">Ruch postaci w osi y</param>
	private void UpdateAnimator(float xAxis, float yAxis)
	{
		if (Mathf.Abs(xAxis) > 0
			|| Mathf.Abs(yAxis) > 0)
		{
			_animator.SetBool("IsWalking", true);
		}
		else
			_animator.SetBool("IsWalking", false);

		if (_Rig2D.velocity.x > 0)
		{
			_animator.SetInteger("Direction", 1);
		}
		else if (_Rig2D.velocity.x < 0)
		{
			_animator.SetInteger("Direction", 3);
		}

		if (_Rig2D.velocity.y > 0)
		{
			_animator.SetInteger("Direction", 0);
		}
		else if (_Rig2D.velocity.y < 0)
		{
			_animator.SetInteger("Direction", 2);
		}

	}

    /// <summary>
    /// Metoda sprawdzająca czy atak może zostać wykonany i jeśli tak to wykonuje go jeśli gracz wciska jeden z klawiszy
    /// Metoda musi być wywoływana w metodzie Update()
    /// </summary>
	private void CheckThrow()
	{
		_actualThrowDelay += Time.deltaTime;
		if (_actualThrowDelay > _throwDelay)
		{
            //Fireball rzucany jest w kierunku zależnym od wciśniętego klawisza
			if (Input.GetKey(KeyCode.W))
			{
				ThrowFireball(new Vector3(0, 0.1f), new Vector2(0, _fireballVelocity)
					+ new Vector2(_Rig2D.velocity.x, 0));
				_actualThrowDelay = 0;
				return;
			}
			if (Input.GetKey(KeyCode.S))
			{
                ThrowFireball(new Vector3(0, -0.1f), new Vector2(0, -_fireballVelocity)
					+ new Vector2(_Rig2D.velocity.x, 0));
				_actualThrowDelay = 0;
				return;
			}
			if (Input.GetKey(KeyCode.A))
			{
                ThrowFireball(new Vector3(-0.1f, 0), new Vector2(-_fireballVelocity, 0)
					+ new Vector2(0, _Rig2D.velocity.y));
				_actualThrowDelay = 0;
				return;
			}
			if (Input.GetKey(KeyCode.D))
			{
                ThrowFireball(new Vector3(0.1f, 0), new Vector2(_fireballVelocity, 0)
					+ new Vector2(0, _Rig2D.velocity.y));
				_actualThrowDelay = 0;
				return;
			}

		}
	}

    /// <summary>
    /// Metoda tworząca obiekt fireball'a i nadająca mu zadaną prędkość
    /// </summary>
    /// <param name="offset">Przesunięcie pozycji w jakiej pojawi się fireball w stosunku do postaci gracza</param>
    /// <param name="velocity">Prędkość fireball'a po jego stworzeniu</param>
	private void ThrowFireball(Vector3 offset, Vector2 velocity)
	{
		var fireball = Instantiate(_fireball);
		fireball.transform.position = this.transform.position + offset;
		fireball.GetComponent<Rigidbody2D>().velocity = velocity;
        fireball.GetComponent<Projectile>().Damage = _attackPower;
	}

    private void OnRoomChange(Direction direction)
    {
        const float roomEntranceOffset = 0.4f;
        float newX, newY;
        var roomManager = FindObjectOfType<RoomManager>();

        switch(direction)
        {
            case Direction.Top:
                newX = roomManager.Columns / 2;
                newY = -1 + roomEntranceOffset + 0.1f;
                break;
            case Direction.Bottom:
                newX = roomManager.Columns / 2;
                newY = roomManager.Rows - roomEntranceOffset;
                break;
            case Direction.Right:
                newX = -1 + roomEntranceOffset;
                newY = roomManager.Rows / 2;
                break;
            case Direction.Left:
                newX = roomManager.Columns - roomEntranceOffset;
                newY = roomManager.Rows / 2;
                break;
            default:
                newX = newY = 0;
                break;
        }

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    protected override void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
