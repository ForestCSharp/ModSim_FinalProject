using UnityEngine;
using System.Collections;

public class AppendageAllele : Allele {

    public Mesh Appendage = null;
    public Vector3 AppendageScale = new Vector3(1, 1, 1);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void Express()
    {
        GameObject NewObj = new GameObject();
        NewObj.AddComponent<MeshFilter>().mesh = Appendage;
        NewObj.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        NewObj.transform.parent = transform;
        NewObj.transform.position = transform.position;
        NewObj.transform.localScale = AppendageScale;

    }
}
