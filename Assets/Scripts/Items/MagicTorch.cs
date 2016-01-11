using UnityEngine;
using System.Collections;

public class MagicTorch : Item 
{
    public MagicTorch Prefab;
    public Projectile TorchProjectile;

    public override void EffectOfItem(Collision2D other)
    {
        other.gameObject.GetComponent<MainCharacter>().Torch = TorchProjectile.gameObject;
    }
}
