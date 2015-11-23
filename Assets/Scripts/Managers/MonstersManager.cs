using UnityEngine;
using System.Collections;

public class MonstersManager : MonoBehaviour 
{
    [SerializeField]
    private Monster[] _monsters;

    private int _actualRoomMonsters;

    void Start()
    {
        Messenger.AddListener(Messages.MonsterDied, OnMonsterDied);
    }

    public void SpawnEnemies(int level, GameObject actualRoom)
    {
        _actualRoomMonsters = 0;

        //z dupy
        float difficulty = level * 15 + 3;
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

    void OnMonsterDied()
    {
        if (--_actualRoomMonsters <= 0)
            Messenger.Broadcast(Messages.AllMonstersDied);
    }
}
