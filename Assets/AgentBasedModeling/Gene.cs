﻿using UnityEngine;
using System.Collections;


//A container for two alleles, representing a single Locus of an organism
//Any two Alleles could theoretically be added to a gene, and their two express functions may behave very differently
//The dominant of the two alleles is the one that is actually expressed
public class Gene : MonoBehaviour {

    public string GeneName;
    public Allele First;
    public Allele Second;

    //Represents the Genetic Locus of this gene
    //This is used when breeding to construct Punnett Squares
    public int Locus;

    private int survivability;
    public int Survivability
    {
        get { return survivability; }
        set { survivability = value; }
    }

    private int desirability;
    public int Desirability
    {
        get { return desirability; }
        set { desirability = value; }
    }

    // Use this for initialization
    void Start () {
        Express();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Chooses the higher dominance Allele and expresses it
    void Express()
    {
        if (First.Dominance >= Second.Dominance)
        {
            First.Express();
            Desirability = First.Desirability;
            Survivability = First.Survivability;
        }
        else
        {
            Second.Express();
            Desirability = Second.Desirability;
            Survivability = Second.Survivability;
        }
    }
}
