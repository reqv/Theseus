using UnityEngine;
using System.Collections;

public class MovementSpeedBooster : Item
{
    public override void EffectOfItem(Collision2D other)
    {
        other.gameObject.GetComponent<MainCharacter>().MovementSpeed += 0.5f;
        FindObjectOfType<GUIManager>().UpdateItems(GetComponent<SpriteRenderer>().sprite);
    }
}
