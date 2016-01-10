using UnityEngine;

public class Chest : Item {
	[Tooltip("Maksymalna ilość przedmiotów.")]
	[SerializeField]
	/// <summary>
	/// Maksymalna ilość przedmiotów.
	/// </summary>
	private int _maxItems;

	[Tooltip("Prefaby lepszych przedmiotów.")]
	[SerializeField]
	/// <summary>
	/// Prefaby lepszych przedmiotów.
	/// </summary>
	private Item[] _rareLoot;

	[Tooltip("Prefaby gorszych przedmiotów.")]
	[SerializeField]
	/// <summary>
	/// Prefaby gorszych przedmiotów.
	/// </summary>
	private Item[] _loot;

	private bool _opened = false;

	/// <summary>
	/// 	Metoda uruchamiana podczas utworzenia pieniądza
	/// </summary>
	public override void Start ()
	{
		base.Start ();
		_Anim = GetComponent<Animator> ();
	}

	/// <summary>
	/// 	Metoda zawierająca efekt działania przedmiotu.
	/// </summary>
	/// <remarks>
	/// 	
	/// </remarks>
	public override void EffectOfItem(Collision2D other)
	{
		if (_opened)
			return;
		_Anim.SetTrigger ("Open");
		_opened = true;
		int rarity = 0;
		Vector3 pozycja = Vector2.zero;
		object item;
		int howMany = RandomNumber (1, _maxItems);

		for (int i = 0; i < howMany; i++) {
			rarity = RandomNumber (0, 100);
			if (rarity < 81) {
				var initLoot = _loot [RandomNumber(0, _loot.Length-1)];

				item = Instantiate(initLoot,transform.position - pozycja,Quaternion.Euler(Vector3.zero));
			} else {
				var initLoot = _rareLoot [RandomNumber(0, _rareLoot.Length-1)];
				item = Instantiate(initLoot,transform.position - pozycja,Quaternion.Euler(Vector3.zero));
			}
		}
	}
}