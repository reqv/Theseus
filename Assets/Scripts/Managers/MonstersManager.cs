using UnityEngine;
using System.Collections;


/**
 * <summary>
 * 	Klasa tworząca przeciwników
 * </summary>
 * <remarks>
 *  Przetrzymuje odwołania do potworów i tworzy je w zadanych pomieszczeniach
 * </remarks>
 */
public class MonstersManager : MonoBehaviour 
{
    [Tooltip("Prefaby potworów")]
    [SerializeField]
    /// <summary>
    /// Prefaby potworów
    /// </summary>
    private Monster[] _monsters;

    /// <summary>
    /// Liczba potworów aktualnie znajdującyh się w pokoju (do otwierania drzwi)
    /// </summary>
    private int _actualRoomMonsters;

    void OnLevelWasLoaded(int level)
    {
        Messenger.AddListener(Messages.MonsterDied, OnMonsterDied);
    }

    /// <summary>
    /// Metoda tworząca przeciwników w danym pokoju
    /// </summary>
    /// <param name="level">Nr poziomu</param>
    /// <param name="actualRoom">Pokój w którym będą przeciwnicy</param>
    public void SpawnEnemies(int level, GameObject actualRoom)
    {
        _actualRoomMonsters = 0;

        //z dupy
        float difficulty = level * 1;
        float actualdifficulty = 0;
        do
        {
            _actualRoomMonsters++;
            var toInstantiate = _monsters[Random.Range(0, _monsters.Length)];
            actualdifficulty += toInstantiate.Difficulty;
            var monster = Instantiate(toInstantiate);
            monster.transform.parent = actualRoom.transform;
            monster.transform.position = new Vector3(Random.Range(-0.5f, 4.5f), Random.Range(-0.5f, 2.5f));
        } while (actualdifficulty < difficulty);
    }

    /// <summary>
    /// Metoda wywoływana w trakcie śmierci potwora, wysyła wiadomość gdy wszystkie potwory są martwe
    /// </summary>
    void OnMonsterDied()
    {
        if (--_actualRoomMonsters <= 0)
            Messenger.Broadcast(Messages.AllMonstersDied);
    }
}
