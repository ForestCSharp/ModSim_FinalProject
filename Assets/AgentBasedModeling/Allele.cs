using UnityEngine;
using System.Collections;

//Represents an allele, subclassing this 
public class Allele : MonoBehaviour {

    //Rank of dominance (higher outranks lower)
    public int Dominance;

    public int Desirability;
    public int Survivability;

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
