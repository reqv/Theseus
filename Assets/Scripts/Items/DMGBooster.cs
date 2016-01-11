using UnityEngine;
using System.Collections;

public class DMGBooster : Item 
{
    public override void EffectOfItem(Collision2D other)
    {
        other.gameObject.GetComponent<MainCharacter>().AttackPower++;
        FindObjectOfType<GUIManager>().UpdateItems(GetComponent<SpriteRenderer>().sprite);
    }
}
