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


	/// <summary>
	/// 	Flaga sprawdzająca, czy skrzynia została otwarta.
	/// </summary>
	private bool _opened = false;

	/// <summary>
	/// 	Nadpisana metoda uruchamiana podczas utworzenia skrzyni. Pozwala na pobranie Animatora.
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
	/// 	Skrzynia po dotknięciu przez gracza wyrzuca z siebie losową liczbę itemów o różnej wartości. (20% szans na elitarny przedmiot )
	/// </remarks>
	/// <param name="other">Obiekt z którym nastąpiło zderzenie.</param>
	public override void EffectOfItem(Collision2D other)
	{
		if (_opened)
			return;
		_Anim.SetTrigger ("Open");
		_opened = true;
		int rarity = 0;
		Vector3 pozycja = Vector2.zero;
		GameObject item;
		int howMany = RandomNumber (1, _maxItems);
        var gameManager = FindObjectOfType<GameManager>();

		for (int i = 0; i < howMany; i++) {
			rarity = RandomNumber (0, 100);
			if (rarity < 81) {
				var initLoot = _loot [RandomNumber(0, _loot.Length-1)];

                item = Instantiate(initLoot, transform.position - pozycja, Quaternion.Euler(Vector3.zero)) as GameObject;
                try { item.transform.SetParent(gameManager.ActualRoom.transform);}
                catch { };
			} else {
				var initLoot = _rareLoot [RandomNumber(0, _rareLoot.Length-1)];
                item = Instantiate(initLoot, transform.position - pozycja, Quaternion.Euler(Vector3.zero)) as GameObject;
                try { item.transform.SetParent(gameManager.ActualRoom.transform); }
                catch { };
			}
		}
	}
}