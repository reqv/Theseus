using UnityEngine;

/// <summary>
/// 	Klasa pocisków dla przeciwników
/// </summary>
public class EnemyProjectile : MonoBehaviour {

	[Tooltip ("Czas po którym element zostanie zniszczony")]
	[SerializeField]
	/// <summary>
	/// 	Czas w którym cień wykonuje skok(teleport)
	/// </summary>
	protected int _timeToLive;

	/// <summary>
	/// 	Zmienna określająca wielkość obrażeń nakładanych po zderzeniu z innym obiektem na ten obiekt
	/// </summary>
	private int _damage;

	/// <summary>
	/// 	Właściwość pozwalająca uzyskać dostęp do prywatnej zmiennej _damage
	/// </summary>
	public int Damage
	{
		get{ return _damage; }
		set{ _damage = value; }
	}

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
		}else if(other.gameObject.tag != "Item")
		{
			Destroy(this.gameObject);
		}
	}
}