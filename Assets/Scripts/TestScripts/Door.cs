using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
    private bool _closed;

    public Direction Direction { get; set; }
    public Collider2D ClosedDoorCollider { get; set; }

    public void Open()
    {
        _closed = false;
        ClosedDoorCollider.enabled = false;
    }

    public void Close()
    {
        _closed = true;
        ClosedDoorCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Messenger.Broadcast<Direction>(Messages.PlayerGoesThroughTheDoor, Direction);
        }
    }
}
