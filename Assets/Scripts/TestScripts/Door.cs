using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
    public Direction Direction { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Messenger.Broadcast<Direction>(Messages.PlayerGoesThroughTheDoor, Direction);
        }
    }
}
