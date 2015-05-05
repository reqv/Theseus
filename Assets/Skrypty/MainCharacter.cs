using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour 
{
	public float MoveVelocityFactor;
	public GameObject Fireball;

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

		GetComponent<Rigidbody2D>().velocity = new Vector2(xAxis * MoveVelocityFactor, yAxis * MoveVelocityFactor);

		if(Input.GetKeyDown(KeyCode.W))
		{
			ThrowFireball(new Vector3(0, 5), new Vector2(0, 100));
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			ThrowFireball(new Vector3(0, -5), new Vector2(0, -100));
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			ThrowFireball(new Vector3(-5, 0), new Vector2(-100, 0));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			ThrowFireball(new Vector3(5, 0), new Vector2(100, 0));
		}
	}

	private void ThrowFireball(Vector3 offset, Vector2 velocity)
	{
		var fireball = Instantiate(Fireball);
		fireball.transform.position = this.transform.position + offset;
		fireball.GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
