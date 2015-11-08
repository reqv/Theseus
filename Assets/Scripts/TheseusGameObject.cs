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
	/// 	Metoda pozwalająca na losowanie liczb całkowitych
	/// </summary>
	/// <param name="min">Minimalna wartość losowana</param>
	/// <param name="min">Maksymalna wartość losowana</param>
	/// <returns>Liczba całkowita z danego w parametrach funkcji przedziału</returns>
	protected int RandomNumber(int min,int max){
		if (min < 0 && max > 0)
			max++;
		int myNumber = Random.Range(min, max);
		return myNumber;
	}
}
