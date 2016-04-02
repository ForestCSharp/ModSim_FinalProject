using UnityEngine;
using System.Collections;

public class MetallicGlossAllele : Allele {

    public float Metallic;
    public float Glossiness;

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
            MR.materials[i].SetFloat("_Metallic", 1.0f);
            MR.materials[i].SetFloat("_Glossiness", 1.0f);
        }
    }
}
