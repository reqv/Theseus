using UnityEngine;
using System.Collections;

public class Snake : Monster {

    private Transform target;
    private Rigidbody2D rig2D;
	private Vector2 axis,collisionAxis,freeais,freedest;
	private float personalMaxSpeed;		// podmiana dla maxSpeed / ze wzgledu na dodawanie predkosci
	public int chaseAdditionalSpeed;	// dodajemy do predkosci poruszania jak zobaczy gracza

	override public void Start () {
		base.Start();
		personalMaxSpeed = maxSpeed;
        rig2D = GetComponent<Rigidbody2D>();
        axis = new Vector2(0, 0);
        freedest = new Vector2(rig2D.position.x, rig2D.position.y);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			Destroy(other.gameObject);		// ######################## chwilowe rozwiazanie ( usuwanie gracza ) ########################
		}
		collisionAxis = axis;
	}

	void OnCollisionStay2D(Collision2D other)
	{
		if(collisionAxis.y < 0){
			freedest.y = rig2D.position.y + patrolRange;
		}
		else{
			freedest.y = rig2D.position.y - patrolRange;
		}
		
		if(collisionAxis.x < 0){
			freedest.x = rig2D.position.x + patrolRange;
		}
		else{
			freedest.x = rig2D.position.x - patrolRange;
		}
	}

	void FixedUpdate () {
		float distance = float.MaxValue;
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			target = GameObject.FindGameObjectWithTag("Player").transform;
			distance = Vector2.Distance(transform.position,target.position);
		}
        if (distance <= attackDistance)
            Attack();
        else if (distance <= range)
            Chase();
        else
            Walking();
	}

    void Attack()
    {
     //   Debug.Log("Attakck!! !! ");
    }

    void Chase()
    {
		if (maxSpeed == personalMaxSpeed)
			personalMaxSpeed += chaseAdditionalSpeed;
        axis = target.position - transform.position;
        if (axis.x < 0)
            axis.x = -1;
        else
            axis.x = 1;
        if (axis.y < 0)
            axis.y = -1;
        else
            axis.y = 1;
        rig2D.AddForce(axis * personalMaxSpeed);
    }

    void Walking()
    {
		if (maxSpeed != personalMaxSpeed)
			personalMaxSpeed = maxSpeed;
        if (Vector2.Distance(rig2D.position, freedest) >= 1)
        {
            if (rig2D.position.x - freedest.x < 0)
                axis.x = 1;
            else
                axis.x = -1;
            if (rig2D.position.y - freedest.y < 0)
                axis.y = 1;
            else
                axis.y = -1;
            rig2D.AddForce(axis * personalMaxSpeed);
        }
            else
        {
			freedest = new Vector2(rig2D.position.x + getRandomNumber(-patrolRange,patrolRange),rig2D.position.y +  getRandomNumber(-patrolRange,patrolRange));
        }
    }
}
