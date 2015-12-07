using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour 
{
    [SerializeField]
    private Sprite _opened;
    [SerializeField]
    private Sprite _closed;

    private SpriteRenderer _sr;

    public Direction Direction { get; set; }
    public Collider2D ClosedDoorCollider { get; set; }

    void OnEnable()
    {
        Messenger.AddListener(Messages.AllMonstersDied, Open);
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        _sr.sprite = _opened;
        ClosedDoorCollider.enabled = false;
    }

    public void Close()
    {
        _sr.sprite = _closed;
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
