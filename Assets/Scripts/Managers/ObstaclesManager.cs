using UnityEngine;
using System.Collections;


/**
 * <summary>
 * 	Klasa tworząca przeszkody
 * </summary>
 * <remarks>
 *  Przetrzymuje odwołania do przeszkód i tworzy je w zadanych pomieszczeniach
 * </remarks>
 */
public class ObstaclesManager : MonoBehaviour
{
    [Tooltip("Prefaby przeszkód")]
    [SerializeField]
    /// <summary>
    /// Prefaby potworów
    /// </summary>
    private Obstacle[] _obstacles;

    /// <summary>
    /// Metoda tworząca przeszkody w danym pokoju
    /// </summary>
    /// <param name="actualRoom">Pokój w którym będą przeszkody</param>
    public void SpawnObstacles(GameObject actualRoom)
    {
        int obstaclesPerRoom = Random.Range(0, 7);
        int obstaclesCounter = 0;

        do
        {
            obstaclesCounter++;
            var toInstantiate = _obstacles[Random.Range(0, _obstacles.Length)];
            
            var obstacle = Instantiate(toInstantiate);
            obstacle.transform.parent = actualRoom.transform;
            //obstacle.transform.position = new Vector3(Random.Range(-0.5f, 4.5f), Random.Range(-0.5f, 2.5f));
            obstacle.transform.position = new Vector3(Random.Range(0f, 4f), Random.Range(0f, 2f));
        } while (obstaclesCounter <= obstaclesPerRoom);
    }
}
