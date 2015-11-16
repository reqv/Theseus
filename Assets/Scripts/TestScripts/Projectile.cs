using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
