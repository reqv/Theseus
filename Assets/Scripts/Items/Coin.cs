using UnityEngine;

public class Coin : Item {

	[SerializeField]
	protected int _coinValue;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia pieniądza
	/// </summary>
	public override void Start ()
	{
		base.Start ();
	}

	/// <summary>
	/// 	Metoda zawierająca efekt działania przedmiotu.
	/// </summary>
	/// <remarks>
	/// 	W tym przypadku dodajemy do puli punktów gracza określoną ich ilość.
	/// </remarks>
	/// <param name="other">Obiekt z którym nastąpiło zderzenie.</param>
	public override void EffectOfItem(Collision2D other)
	{
		var p = other.gameObject.GetComponent<MainCharacter>();
		p.Coins += _coinValue;
	}
}
