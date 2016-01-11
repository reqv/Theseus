using UnityEngine;
using System.Collections;

public class MaxHealthBooster : Item 
{
    public override void EffectOfItem(Collision2D other)
    {
        other.gameObject.GetComponent<MainCharacter>().MaxHealthPoints++;
        FindObjectOfType<GUIManager>().UpdateItems(GetComponent<SpriteRenderer>().sprite);
    }
}
