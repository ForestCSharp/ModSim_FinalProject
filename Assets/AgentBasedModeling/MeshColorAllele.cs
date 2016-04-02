using UnityEngine;
using System.Collections;

public class MeshColorAllele : Allele {

    public Color MeshColor;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void Express()
    {
        MeshRenderer MR = GetComponent<MeshRenderer>();
        for (int i = 0; i < MR.materials.Length; ++i)
        {
            MR.materials[i].color = MeshColor;
        }
    }
}
