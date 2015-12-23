using UnityEngine;

/// <summary>
/// 	Klasa pocisków dla przeciwników
/// </summary>
public class EnemyProjectile : TheseusGameObject {

	[Tooltip ("Czas po którym element zostanie zniszczony")]
	[SerializeField]
	/// <summary>
	/// 	Czas w którym cień wykonuje skok(teleport)
	/// </summary>
	protected int _timeToLive;

	/// <summary>
	/// 	Właściwość pozwalająca określić ilość obrażeń zadawanych przy zderzeniu
	/// </summary>
	public int Damage {get; set;}

	/// <summary>
	/// 	Metoda ustawiająca początkowe zmienne i inicjalizująca pocisk
	/// </summary>
	public void Start()
	{
		Destroy (this.gameObject, _timeToLive);
	}

	/// <summary>
	/// 	Metoda uruchamiana, gdy pocisk wejdzie w kolizję z innym obiektem
	/// </summary>
	/// <param name="other">Element obcy z którym wystąpiła kolizja</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);		// ######################## chwilowe rozwiazanie ( usuwanie gracza ) ########################
		}else if(other.gameObject.tag != "Item" && other.gameObject.tag != "Projectile")
		{
			Destroy(this.gameObject);
		}
	}
}