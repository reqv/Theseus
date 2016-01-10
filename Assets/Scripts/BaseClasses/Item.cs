using UnityEngine;

/**
 * <summary>
 * 	Abstrakcyjna klasa dla przedmiotów.
 * </summary>
 * <remarks>
 * 	Klasa rodzica dla wszystkich przedmiotów w grze, zawiera podstawową implementacje każdego przedmiotu.
 * </remarks>
 */
public abstract class Item : TheseusGameObject {
	[Tooltip ("Siła z jaką wypada przedmiot po pojawieniu sie.")]
	[SerializeField]
	/// <summary>
	/// 	Siła z jaką wypada przedmiot po pojawieniu sie.
	/// </summary>
	protected byte _impact;

	[Tooltip ("Destruction delay")]
	[SerializeField]
	/// <summary>
	/// 	Siła z jaką wypada przedmiot po pojawieniu sie.
	/// </summary>
	protected byte _cleanDelay;

	[Tooltip ("Czy wypada ze skrzyni ?")]
	[SerializeField]
	/// <summary>
	/// 	Czy wypada ze skrzyni ?
	/// </summary>
	protected bool _fromChest;

	/// <summary>
	/// 	Opóźnienie przed możliwością podniesienia
	/// </summary>
	private float _pickUpDelay = 0.5f;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu
	/// </summary>
	public virtual void Start () {
		_Rig2D = GetComponent<Rigidbody2D>();
		if (_fromChest)
			_Rig2D.AddForce (new Vector2 (RandomNumber (-1, 1), RandomNumber (-1, 1)) * _impact, ForceMode2D.Impulse);
		
		else
			_pickUpDelay = 0;
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas wykrycia zajścia kolizji.
	/// </summary>
	/// <remarks>
	/// 	W przypadku, gdy przedmiot podniesie gracz wykonywana jest metoda EffectOfItem zawierająca opis wpływu tego przedmiotu na grę. Sam przedmiot znika po podniesieniu.
	/// </remarks>
	public virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player" && _pickUpDelay <= 0) {
			EffectOfItem(other);
			Destroy(gameObject,(float)_cleanDelay);
		}
	}

	public virtual void FixedUpdate () {
		if (_pickUpDelay > 0)
			_pickUpDelay -= Time.deltaTime;
	}


	/// <summary>
	/// 	Metoda abstrakcyjna, implementowana w potomkach. Określa specyficzne działanie przedmiotu.
	/// </summary>
	public abstract void EffectOfItem(Collision2D other);
}
