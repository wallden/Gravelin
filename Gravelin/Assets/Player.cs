using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Camera = UnityEngine.Camera;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Mouse0))
	    {
	        RaycastHit hit;

	        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit))
	        {
	            var hitSurface = hit.transform;
                Debug.Log(hit.point);
	            var joint = hitSurface.transform.gameObject.AddComponent<ConfigurableJoint>();
	            ConfigureJoint(joint);
	            joint.connectedBody = transform.gameObject.GetComponent<Rigidbody>();
	        }
	    }
	}

    ConfigurableJoint ConfigureJoint(ConfigurableJoint joint)
    {
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        return joint;
    }
}
