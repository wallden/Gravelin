using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Combat : MonoBehaviour
{

    public int hp = 1;

    private Player entity;
    private Rigidbody rigidbody;
	// Use this for initialization
	void Start ()
	{
	    entity = GetComponent<Player>();
	    rigidbody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Spear")
        {
            hp--;
            if (hp <= 0)
            {
                
                KillCombatObject();
            }
        }
    }

    private void KillCombatObject()
    {
        entity.isAlive = false;
        rigidbody.freezeRotation = false;

        Debug.Log("Killed Player "+entity.playerNumber);
    }
}
