using UnityEngine;
using System.Collections;

public class Snake2 : MonoBehaviour {

    public float maxSpeed;
    public float range;
    public float attackDist;

    private Transform target;
    private Rigidbody2D rig2D;
    private Vector2 axis,freeais,freedest;
    private System.Random rand;


	void Start () {
        rig2D = GetComponent<Rigidbody2D>();
      //  rig2D.angularVelocity = 0;
        axis = new Vector2(0, 0);
        freedest = new Vector2(rig2D.position.x, rig2D.position.y);
	}
	
	void FixedUpdate () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Vector2.Distance(transform.position,target.position);

        if (distance <= attackDist)
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
        axis = target.position - transform.position;
        if (axis.x < 0)
            axis.x = -1;
        else
            axis.x = 1;
        if (axis.y < 0)
            axis.y = -1;
        else
            axis.y = 1;
        rig2D.AddForce(axis * maxSpeed);
       // transform.position = Vector2.MoveTowards(transform.position, target.position, maxSpeed*Time.deltaTime);
    }

    void Walking()
    {
        Debug.Log("dist = " + Vector2.Distance(rig2D.position, freedest));
       // Debug.Log("x = " + rig2D.position.x+" -> "+freedest.x);
      //  Debug.Log("y = " + rig2D.position.y + " -> " + freedest.y);
        if (Vector2.Distance(rig2D.position, freedest) > 1)
        {

            if (rig2D.position.x - freedest.x < 0)
                axis.x = 1;
            else
                axis.x = -1;
            if (rig2D.position.y - freedest.y < 0)
                axis.y = 1;
            else
                axis.y = -1;
            rig2D.AddForce(axis * maxSpeed);
          //  Debug.Log("Walking");
        }
            else
        {
            rand = new System.Random();
            freedest = new Vector2(rig2D.position.x + rand.Next(-5,5),rig2D.position.y +  rand.Next(-5,5));
            Debug.LogWarning("dest changed: "+freedest.x + " | "+freedest.y);
        }
    }
}
