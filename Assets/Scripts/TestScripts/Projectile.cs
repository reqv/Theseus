using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    public void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "Enemy") {
			Monster Enemy = other.gameObject.GetComponent<Monster>();
			Enemy.TakingDamage(10);	// To do zmiany, wedle uznania
		}
        Destroy(gameObject);
    }
}
