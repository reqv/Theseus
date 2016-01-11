using UnityEngine;
using System.Collections;

public class AttackSpeedBooster : Item 
{
    public override void EffectOfItem(Collision2D other)
    {
        other.gameObject.GetComponent<MainCharacter>().AttackDelay -= 0.05f;
        FindObjectOfType<GUIManager>().UpdateItems(GetComponent<SpriteRenderer>().sprite);
    }
}
