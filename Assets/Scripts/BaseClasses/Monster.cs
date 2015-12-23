using UnityEngine;


/// <summary>
/// Enumerator mówiący o statusie potworów względem efektów kontroli tłumu
/// </summary>
public enum MonsterStatus
{
	OK = 0,
	Slowed = 1,
	Blinded = 2,
	Stunned = 3
}

/**
 * <summary>
 * 	Abstrakcyjna klasa dla potworów
 * </summary>
 * <remarks>
 * 	Klasa rodzica dla wszystkich przeciwników w grze, zawiera podstawową implementacje każdego potwora.
 * </remarks>
 */
public abstract class Monster : TheseusGameObject {

	/// <summary>
	/// 	Parametr trzymajacy aktualna pozycje gracza
	/// </summary>
	protected Transform _targetToAttack;

	/// <summary>
	/// 	Parametr trzymający aktualny kierunek poruszania sie potwora
	/// </summary>
	protected Vector2 _axis;

	/// <summary>
	/// 	Parametr trzymający kierunek poruszania się stwora podczas wykrycia kolizji 
	/// </summary>
	protected Vector2 _collisionAxis;

	/// <summary>
	/// 	Parametr trzumający pozycję do której zmierza stwór
	/// </summary>
	protected Vector2 _freeDestination;

	/// <summary>
	/// 	Parametr sprawdzający, czy postać jest odwrócona w prawo
	/// </summary>
	protected bool _facingRight = true;

	/// <summary>
	/// 	Parametr pozwalający określić odpowiednią wielkość błędu w prędkości chodu przy której potwór zostanie odwrócony na drugą stronę.
	/// </summary>
	protected float _flipRate = 0.5f;

	/// <summary>
	/// 	Aktualny status "kontroli tłumu" na potworze
	/// </summary>
	protected MonsterStatus _status = MonsterStatus.OK;

	/// <summary>
	/// 	Zegar odliczający czas efektów "kontroli tłumu"
	/// </summary>
	protected double _ccEffectTimer;

	/// <summary>
	/// 	Zmienna sprawdzająca, czy obiekt otrzymuje obrażenia w czasie
	/// </summary>
	protected bool _isDamagedOverTime = false;

	/// <summary>
	/// 	Zegar odliczający czas po którym obrażenia czasowe znikają
	/// </summary>
	protected double _DOTTimer;

	/// <summary>
	/// 	Zegar odliczający czas do naliczenia obrażeń czasowych
	/// </summary>
	protected int _lastDoTTick;

	/// <summary>
	/// 	Wielkość obrażeń zadawanych w czasię ( co sekundę )
	/// </summary>
	protected int _DOTDamage;

	/// <summary>
	/// 	Flaga mówiąca o tym, czy obiekt nadal żyję.
	/// </summary>
	private bool _isAlive = true;

	[Tooltip ("Aktualne zdrowie potwora")]
	[SerializeField]
	/// <summary>
	/// 	Parametr trzymajacy aktualne zdrowie potwora
	/// </summary>
	protected int _healthPoints;
	
	[Tooltip ("Siła ataku potwora")]
	[SerializeField]
	/// <summary>
	/// 	Parametr trzymajacy siłę ataku potwora
	/// </summary>
	protected int _attackPower;

	[Tooltip ("Prędkość lotu pocisków")]
	[SerializeField]
	/// <summary>
	/// 	Parametr mówiący o prędkości wystrzeliwanych pocisków, ważny tylko dla jednostek strzelających.
	/// </summary>
	protected int _bulletVelocityFactor;

	[Tooltip ("Maksymalne przyspieszenie obiektu.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa maksymalną szybkość obiektu
	/// </summary>
	protected float _maxSpeed;

	/// <summary>
	/// 	Prawdziwe przyśpieszenie obiektu dla której podstawą jest _maxSpeed, zależna również od innych czynników 
	/// </summary>
	protected float _realMaxSpeed;

	[Tooltip ("Zasięg spostrzeżenia gracza przez obiekt.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa zasięg widzenia innych obiektów przez dany obiekt
	/// </summary>
	protected float _range;

	/// <summary>
	/// 	Prawdziwa wartość spostrzegawczości potwora dla której podstawą jest _range, zależna również od innych czynników 
	/// </summary>
	protected float _realRange;

	[Tooltip ("Zasięg od którego obiekt rozpoczyna atak.")]
	[SerializeField]
	/// <summary>
	/// 	Parametr serializowany, określa maksymalną szybkość obiektu
	/// </summary>
	protected float _attackDistance;

	/// <summary>
	/// 	Prawdziwa wartość zasięgu ataku potwora dla której wartością bazową jest _attackDistance, zależna również od innych czynników 
	/// </summary>
	protected float _realAttackDistance;

    [Tooltip("Jak trudny do pokonania jest przeciwnik?")]
    [SerializeField]
    /// <summary>
    /// 	Parametr serializowany, określa poziom trudności przeciwnika
    /// </summary>
    public float Difficulty;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu
	/// </summary>
	public virtual void Start () {
		_Rig2D = GetComponent<Rigidbody2D>();
		_Render2D = GetComponent<SpriteRenderer> ();
		_Anim = GetComponent<Animator> ();
		_axis = Vector2.zero;
		_realMaxSpeed = _maxSpeed;
		_realRange = _range;
		_realAttackDistance = _attackDistance;
		TakingDamage (0);	// Sprawdza, czy potwor ma wiecej niz 0 zycia na starcie
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas każdej jednostki czasu.
	/// </summary>
	public virtual void Update () {
		
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas każdej jednostki czasu i zawsze w stałym momencie. Służy do dokonywania obliczeń między obiektami. 
	/// </summary> 
	/// <remarks>
	/// 	Wynikiem końcowym metody jest uruchomienie jednej z trzech akcji: Attack(), Chase() lub Walking() zależnej 
	/// 	od odległości między obiektami.
	/// </remarks>
	public virtual void FixedUpdate () {
		CheckPersonalStatus ();
		if (_status == MonsterStatus.Stunned)
			return;
		float distance = float.MaxValue;
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			_targetToAttack = GameObject.FindGameObjectWithTag("Player").transform;
			distance = WhereIsATarget(_targetToAttack.position,true);
		}
		if (distance <= _realAttackDistance)
			Attack ();
		else if (distance <= _realRange)
			Chase ();
		else
			Walking();

		if (_facingRight && CheckAxis () < 0)
			Flip ();
		if (!_facingRight && CheckAxis () > 0)
			Flip ();
	}

	/// <summary>
	/// 	Abstrakcja ataku
	/// </summary>
	/// <remarks>
	/// 	Funkcja abstrakcyjna implementowana dla każdego z potworów oddzielnie. Służy do zaprogramowania akcji ataku.
	/// </remarks>
	public abstract void Attack();

	/// <summary>
	/// 	Abstrakcja gonitwy za obiektem który został zauważony przez ten obiekt
	/// </summary>
	/// <remarks>
	/// 	Funkcja abstrakcyjna implementowana dla każdego z potworów oddzielnie. Służy do zaprogramowania akcji podążania za graczem.
	/// </remarks>
	public abstract void Chase();

	/// <summary>
	/// 	Abstrakcja swobodnego zachowania obiektu, gdy szukany inny obiekt nie jest w zasięgu
	/// </summary>
	/// <remarks>
	/// 	Funkcja abstrakcyjna implementowana dla każdego z potworów oddzielnie. Służy do zaprogramowania akcji chodu podczas, gdy gracz nie został jeszcze wykryty.
	/// </remarks>
	public abstract void Walking();


	/// <summary>
	/// 	Pozwala na poznanie kierunku w którym porusza się obiekt.
	/// </summary>
	/// <param name="checkY">Flaga która mówi o tym, czy sprawdzana jest oś Y. Wstaw 1 jeżeki tak, 0  jeżeli nie (wartość domyślna)</param>
	/// <remarks>
	/// 	Funkcja pozwala na określenie w którą strone zmierza obiekt, zależnie od podanej w parametrze funkcji wartości otrzymamy wynik z osi X lub Y.
	/// </remarks>
	/// <returns>
	/// 	Odpowiedni kierunek poruszania obiektu.
	/// 	Możliwe wyjście to:
	/// 	1 - dla lewa lub dołu
	/// 	-1 - dla prawa lub góry
	/// 	0 - obiekt nie porusza się w danej osi X lub Y
	/// </returns>
	protected int CheckAxis(int checkY = 0)
	{
		float velo = 0;
		if(checkY == 0)
			velo =  _Rig2D.velocity.x;
		else
			velo =  _Rig2D.velocity.y;
		if (velo < -_flipRate)
			return -1;
		else if (velo > _flipRate)
			return 1;
		else
			return 0;
	}

	/// <summary>
	/// 	Metoda uruchamiana, gdy potwór odnosi obrażenia
	/// </summary>
	/// <param name="damage">Określa ilość obrażeń jakie zostały zadane potworowi</param>
	/// <remarks>
	/// 	Podana zmienna oznacza ilość obrażeń odejmowanych od aktualnego zdrowia potwora, jeżeli potwór posiada zerową lub ujemną ilość punktów życia uruchamiana jest metoda Die().
	/// </remarks>
	public virtual void TakingDamage(int damage)
	{
		_healthPoints -= damage;
		if (_healthPoints <= 0)
			Die ();
	}

	/// <summary>
	/// 	Metoda aktualizująca status potwora wzgledem czasu
	/// </summary>
	/// <remarks>
	/// 	Metoda sprawdza aktualny stan potwora. Jeżeli potwór wyszedł (wedle zegara efektów kontroli tłumu) z efektu kontroli tłumu to
	/// 	metoda ta wyłącza wszystkie negatywne efekty tego statusu. Metoda ta odpowiada również za naliczanie obrażeń czasowych i ich zdejmowanie.
	/// </remarks>
	protected void CheckPersonalStatus()
	{
		if (_status != MonsterStatus.OK)
		if (_ccEffectTimer <= 0) {
			_status = MonsterStatus.OK;
			_realMaxSpeed = _maxSpeed;
			_realAttackDistance = _attackDistance;
			_realRange = _range;
		}
		else
			_ccEffectTimer -= Time.deltaTime;

		if (_isDamagedOverTime)
		if (_DOTTimer <= 0) {
			_isDamagedOverTime = false;
			_Render2D.color = Color.white;
		} else {
			_DOTTimer -= Time.deltaTime;
			if((int)_DOTTimer != _lastDoTTick)
			{
				TakingDamage(_DOTDamage);
				_lastDoTTick = (int)_DOTTimer;
			}
		}
	}

	/// <summary>
	/// 	Metoda publiczna pozwalająca na ustawienie efektu kontroli tłumu dla potwora
	/// </summary>
	/// <param name="status">Status jaki zostanie nałożony na potwora(patrz 'public enum MonsterStatus').</param>
	/// <param name="time">Czas podczas którego efekt będzie aktywny(podany w sekundach).</param>
	public virtual void SetCrowdControl(MonsterStatus status,int time)
	{
		if (_status <= status) {
			switch(status)
			{
			case MonsterStatus.Slowed:
				if(_status != MonsterStatus.Slowed)
					_realMaxSpeed = _maxSpeed /2;
				break;
			case MonsterStatus.Blinded:
				_realAttackDistance = 1;
				_realRange = 3;
				break;
			default: break;
			}
			_status = status;
			_ccEffectTimer = time;
		}
	}

	/// <summary>
	/// 	Metoda publiczna pozwalająca na ustawienie obrażeń zadawanych w czasię
	/// </summary>
	/// <param name="time">Czas podczas którego efekt będzie aktywny(podany w sekundach).</param>
	/// <param name="damage">Ilość obrażeń zadawanych co sekundę.</param>
	public virtual void SetDamageOverTime(int time,int damage)
	{
		if (_isDamagedOverTime) {
			if(damage > _DOTDamage)
				_DOTDamage = damage;
		} else {
			_DOTTimer = time;
			_lastDoTTick = (int)_DOTTimer;
			_DOTDamage = damage;
			_isDamagedOverTime = true;
		}
		_Render2D.color = Color.green;
	}

	/// <summary>
	/// 	Metoda uruchamiana, gdy potwór umiera
	/// </summary>
	protected void Die()
	{
		if (_isAlive) {
			_isAlive = false;
			Messenger.Broadcast (Messages.MonsterDied);
			_Anim.SetTrigger ("Die");
			SetCrowdControl (MonsterStatus.Stunned, 3);
			Destroy (this.gameObject, 2.0f);
		}
	}

	/// <summary>
	/// 	Metoda odpowiadająca za tworzenie obiektu pocisku
	/// </summary>
	protected void NewProjectile(GameObject projectile, Vector2 offset, Vector2 velocity, float rotate = 0)
	{
		var bullet = Instantiate (projectile,new Vector2(this.transform.position.x + offset.x, this.transform.position.y + offset.y),Quaternion.Euler(new Vector3(0,0,rotate))) as GameObject;
		//bullet.GetComponent<Rigidbody2D>().velocity = velocity;
		bullet.GetComponent<Rigidbody2D>().AddForce(getRightAxis(bullet.GetComponent<Rigidbody2D>().position,_targetToAttack.position) * _bulletVelocityFactor,ForceMode2D.Impulse);
	}
	
	/// <summary>
	/// 	Pozwala na zmianę kierunku poruszania się obiektu oraz na obliczenie dystansu do punktu docelowego
	/// </summary>
	/// <param name="target">Cel podróży obiektu</param>
	/// <param name="justCalculateDistance">Parametr pozwalający na ustalenie, czy chcemy zmienić kierunek ruchu, czy też chcemy tylko obliczenia dystansu od celu</param>
	/// <remarks>
	/// 	Dzięki tej funkcji możemy ustalić w którą stronę powinien wyruszyć nasz obiekt, aby doszedł on do celu swojej podróży. Funkcja oblicza rówież odległość jaką musimy pokonać, aby znaleść się u celu. 
	/// </remarks>
	/// <returns>
	/// 	Funkcja zwraca nam dystans oddzielający nas od pożądanego celu podanego jako parametr
	/// </returns>
	protected float WhereIsATarget(Vector2 target,bool justCalculateDistance=false)
	{
		float distance = Vector2.Distance (_Rig2D.position, target);
		if (justCalculateDistance)
			return distance;

		_axis.x = target.x - transform.position.x;
		_axis.y = target.y - transform.position.y;

		if (_axis.x < 0)
			_axis.x = -1;
		else
			_axis.x = 1;
		if (_axis.y < 0)
			_axis.y = -1;
		else
			_axis.y = 1;
		return distance;
	}

	/// <summary>
	/// 	Metoda pozwalająca na obrót danej postaci.
	/// </summary>
	protected void Flip()
	{
		_facingRight = !_facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}