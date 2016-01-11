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

    [Tooltip("Czas przez jaki gracz będzie nietykalny po otrzymaniu obrażeń")]
    [SerializeField]
    /// <summary>
    /// Czas przez jaki gracz będzie nietykalny po otrzymaniu obrażeń
    /// </summary>
    private float _afterAttackInviolabilityTime;

    [Tooltip("Maksymalne zdrowie postaci - musi być")]
    [SerializeField]
    /// <summary>
    /// Maksymalne zdrowie postaci
    /// </summary>
    private int _maxHealthPoints;

    [Tooltip("Magiczna pochodnia której aktualnie używa gracz")]
    [SerializeField]
    /// <summary>
    /// Magiczna pochodnia której aktualnie używa gracz
    /// </summary>
    private MagicTorch _torch;
	#endregion

    /// <summary>
    /// Licznik czasu nietykalności;
    /// </summary>
    private float _InviolabilityTimer = 0;
    /// <summary>
    /// Własność sprawdzająca czy postać gracza jest nietykalna
    /// </summary>
    private bool _isInviolable { get { return _InviolabilityTimer >= 0; } }
    /// <summary>
    /// Aktualny czas który minął od ostatniego ataku
    /// </summary>
	private float _actualThrowDelay = 0;
    /// <summary>
    /// Komponent Animator postaci
    /// </summary>
	private Animator _animator;

    private int _coins;

    [HideInInspector]
    /// <summary>
    /// Ilość monet którą posiada gracz
    /// </summary>
    public int Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            _coins = value;
            Messenger.Broadcast<int>(Messages.PlayerCoinsChanged, _coins);
        }
    }
    
    [HideInInspector]
    /// <summary>
    /// Zdrowie Gracza
    /// </summary>
    public int HealthPoints
    {
        get
        {
            return _healthPoints;
        }

        set
        {
            _healthPoints = value;
            if (_healthPoints > _maxHealthPoints)
                _healthPoints = _maxHealthPoints;

            Messenger.Broadcast<int,int>(Messages.PlayerHealthChanged, _healthPoints, _maxHealthPoints);
        }
    }
    
    public GameObject Torch
    {
        get
        {
            return _fireball;
        }
        set
        {
            _fireball = value;
            FindObjectOfType<GUIManager>().UpdateTorch(_fireball.GetComponent<SpriteRenderer>().sprite);
        }
    }

    /// <summary>
    /// 	Metoda uruchamiana podczas utworzenia obiektu
    /// </summary>
	void Start () 
	{
		_Rig2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
        _Render2D = GetComponent<SpriteRenderer>();

        Coins = 10;
        Messenger.AddListener<Direction>(Messages.PlayerGoesThroughTheDoor, OnRoomChange);
        Messenger.Broadcast<int, int>(Messages.PlayerHealthChanged, _healthPoints, _maxHealthPoints);
        Messenger.Broadcast<int>(Messages.PlayerCoinsChanged, Coins);
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
        UpdateInviolabilityTimer();
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

    /// <summary>
    /// Metoda wywoływana co klatkę, aktualizuje timer nietykalności postaci
    /// </summary>
    private void UpdateInviolabilityTimer()
    {
        if (_isInviolable)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            _InviolabilityTimer -= Time.deltaTime;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    /// <summary>
    /// Sprawia, że postać przez czas <see cref="_afterAttackInviolabilityTime"/> jest nietykalna
    /// </summary>
    private void SetInviolable()
    {
        _InviolabilityTimer = _afterAttackInviolabilityTime;
    }

    /// <summary>
    /// Metoda reagująca na zdarzenie przejścia do innego pokoju. Ustawia gracza w odpowiednim miejscu w nowym pokoju
    /// </summary>
    /// <param name="direction">Kierunek w którym zostałą dokonana zmiana pokoju</param>
    private void OnRoomChange(Direction direction)
    {
        const float roomEntranceOffset = 0.4f;
        float newX, newY;
        var roomManager = FindObjectOfType<GameManager>().roomManager;

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

        SetInviolable();
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    /// <summary>
    /// Przeciążona metoda śmierci gracza
    /// </summary>
    protected override void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    /// <summary>
    /// Przeciążona metoda otrzymywania obrażeń, reaguje na nietykalność gracza
    /// </summary>
    /// <param name="damage"></param>
    public override void TakingDamage(int damage)
    {
        if (!_isInviolable)
        {
            base.TakingDamage(damage);
            if (damage > 0)
            {
                SetInviolable();
                Messenger.Broadcast<int, int>(Messages.PlayerHealthChanged, _healthPoints, _maxHealthPoints);
            }
        }
    }

    /// <summary>
    /// Metoda reagująca na zdarzenie kolizji z innymi obiektami, jeśli gracz zderzy się z przeciwnikiem otrzymuje obrażenia
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakingDamage(1);
        }
    }
}
