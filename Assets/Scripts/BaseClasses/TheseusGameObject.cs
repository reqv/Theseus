using UnityEngine;

/**
 * <summary>
 * 	Abstrakcyjna klasa dla obiektów gry.
 * </summary>
 * <remarks>
 * 	Klasa rodzica dla wszystkich obiektów w grze, zawiera elementy wspólne dla każdego obiektu.
 * </remarks>
 */
public abstract class TheseusGameObject : MonoBehaviour {

	/// <summary>
	/// 	Obiekt fizycznego ciała(Nvidia Physx)
	/// </summary>
	protected Rigidbody2D _Rig2D;

	/// <summary>
	/// 	Obiekt obrazka przypisanego do danego obiektu
	/// </summary>
	protected SpriteRenderer _Render2D;

	/// <summary>
	/// 	Obiekt animatora przypisanego do danego obiektu
	/// </summary>
	protected Animator _Anim;

	/// <summary>
	/// 	Metoda pozwalająca na losowanie liczb całkowitych
	/// </summary>
	/// <param name="min">Minimalna wartość losowana</param>
	/// <param name="min">Maksymalna wartość losowana</param>
	/// <returns>Liczba całkowita z danego w parametrach funkcji przedziału</returns>
	protected int RandomNumber(int min,int max){
		int myNumber = Random.Range(min, max+1);
		return myNumber;
	}

	/// <summary>
	/// 	Metoda pozwalająca na wyliczenie kąta obrotu obiektu względem 2 punktów
	/// </summary>
	/// <param name="currPos">Pozycja pierwszego obiektu (aktualnego)</param>
	/// <param name="destPos">Pozycja drugiego obiektu (do którego zmierzamy)</param>
	/// <param name="offset">Liczba dodawana do wyliczonego kąta</param>
	/// <returns>Kąt podany w stopniach</returns>
	public float VecToTan(Vector3 currPos,Vector3 destPos,float offset = 0f)
	{
		float opp = currPos.x - destPos.x;
		float adj = currPos.y - destPos.y;
		adj *= -1;
		return Mathf.Rad2Deg * Mathf.Atan2(opp,adj) + offset;
	}

	/// <summary>
	/// 	Metoda pozwalająca na szybkie wyliczenie w którą stronę powinien udać się obiekt, aby dojść do innego obiektu
	/// </summary>
	/// <param name="source">Pozycja pierwszego obiektu (aktualnego)</param>
	/// <param name="dest">Pozycja drugiego obiektu (do którego zmierzamy)</param>
	/// <returns>Obiekt Vector2 zawierający odpowiednie wyliczenia</returns>
	public Vector2 getRightAxis(Vector3 source,Vector3 dest)
	{
		return new Vector2 (dest.x - source.x, dest.y - source.y);
	}
}
