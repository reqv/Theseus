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

	[Tooltip ("Mówi tym, czy przedmiot wypadł z potwora lub skrzyni.")]
	[SerializeField]
	/// <summary>
	/// 	Mówi tym, czy przedmiot wypadł z potwora.
	/// </summary>
	protected bool _fromMonsterOrChest;

	[Tooltip ("Siła z jaką wypada przedmiot z potwora.")]
	[SerializeField]
	/// <summary>
	/// 	Siła z jaką wypada przedmiot.
	/// </summary>
	protected byte _impact;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia obiektu
	/// </summary>
	public virtual void Start () {
		_Rig2D = GetComponent<Rigidbody2D>();
		if (_fromMonsterOrChest) {
			_Rig2D.AddForce(new Vector2(RandomNumber(-1,1),RandomNumber(-1,1)) * _impact,ForceMode2D.Impulse);
		}
	}

	/// <summary>
	/// 	Metoda uruchamiana podczas wykrycia zajścia kolizji.
	/// </summary>
	/// <remarks>
	/// 	W przypadku, gdy przedmiot podniesie gracz wykonywana jest metoda EffectOfItem zawierająca opis wpływu tego przedmiotu na grę. Sam przedmiot znika po podniesieniu.
	/// </remarks>
	public virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			EffectOfItem();
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// 	Metoda abstrakcyjna, implementowana w potomkach. Określa specyficzne działanie przedmiotu.
	/// </summary>
	public abstract void EffectOfItem();
}
