using UnityEngine;

public class ScaleAllele : Allele {

    public float NewScale = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    override public void Express() 
    {
        transform.localScale = new Vector3(NewScale, NewScale, NewScale);
    }
}
