using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Events;
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
	    if (collision.gameObject.tag == "Spear" && collision.gameObject.GetComponent<JavelinPhysics>() != null)
        {
            
            hp--;
            if (hp <= 0)
            {
                var owner = collision.gameObject.GetComponent<JavelinPhysics>().Owner;
                KillCombatObject(owner);
            }
        }
    }

    private void KillCombatObject(GameObject sourceThatsResponsibleForKilling)
    {
        entity.isAlive = false;
        rigidbody.freezeRotation = false;
        Events.instance.Raise(new PlayerDiedEvent
        {
            PlayerKilled = gameObject,
            SourceOfDeath = sourceThatsResponsibleForKilling
        });
        sourceThatsResponsibleForKilling.GetComponent<GUI_Player>().ShowPlayerKilledText(entity.playerNumber.ToString());
        Debug.Log("Killed Player "+entity.playerNumber);
    }
}
