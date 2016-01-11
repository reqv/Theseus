using UnityEngine;
using System.Collections;

public class NextLevelLadder : MonoBehaviour 
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().NextLevel();
        }
    }
}
