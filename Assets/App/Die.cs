using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die :  MonoBehaviour
{

    public Vector3 InitForce;

	// Use this for initialization
	void Start ()
	{
	    var rb = GetComponent<Rigidbody>();
        rb.AddForce(InitForce.x, InitForce.y, InitForce.z, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
