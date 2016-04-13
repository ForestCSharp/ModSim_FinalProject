using UnityEngine;
using System.Collections;

//Represents an allele, subclassing this 
public class Allele : MonoBehaviour {

    //Rank of dominance (higher outranks lower)
    public string AlleleName;
    public int Dominance = 0;

    public int Desirability = 0;
    public int Survivability = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //This function expresses the allele
    virtual public void Express()
    {
        //Base class left blank
    }
}
