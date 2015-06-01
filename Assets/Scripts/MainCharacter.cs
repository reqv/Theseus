using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour 
{
	public float WspRuchu;

	void Start () 
	{
	
	}
	
	void Update () 
	{
		var xAxis = Input.GetAxis ("Horizontal");
		var yAxis = Input.GetAxis ("Vertical");

		if (Mathf.Abs(xAxis) > 0 || Mathf.Abs(yAxis) > 0)
		{
			GetComponent<Animator>().SetBool("IsWalking", true);
		}
		else
			GetComponent<Animator>().SetBool("IsWalking", false);

		if (xAxis > 0)
		{
			GetComponent<Animator>().SetInteger("Direction", 1);
		}
		else if (xAxis < 0)
		{
			GetComponent<Animator>().SetInteger("Direction", 3);
		}

		if (yAxis > 0)
		{
			GetComponent<Animator>().SetInteger("Direction", 0);
		}
		else if (yAxis < 0)
		{
			GetComponent<Animator>().SetInteger("Direction", 2);
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2 (xAxis * WspRuchu, yAxis * WspRuchu);
	}
}
