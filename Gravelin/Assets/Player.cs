using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.Mouse0))
	    {
	        RaycastHit hit;

	        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit))
	        {
	            var hitSurface = hit.transform;
                Debug.Log(hit.point);
	        }
	    }
	}
}
