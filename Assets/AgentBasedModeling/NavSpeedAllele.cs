using UnityEngine;
using System.Collections;

public class NavSpeedAllele : Allele {

    public float NewSpeed = 4.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Express()
    {
        GetComponent<NavMeshAgent>().speed = NewSpeed;
    }
}
