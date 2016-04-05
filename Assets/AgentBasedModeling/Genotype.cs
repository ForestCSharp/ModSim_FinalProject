using UnityEngine;
using System;
using System.Collections.Generic;

public class Genotype : MonoBehaviour {

    //List of all genes in this GameObject
    List<Gene> GeneList = new List<Gene>();

	// Use this for initialization
    //Queries all of this gameobjects genes to build up a cached genotype
	void Start () {

        //Cache the Genes on Start
        GeneList.AddRange(GetComponents<Gene>());

        //Sort Genes by Locus
        Comparison<Gene> Comparison = (x, y) => x.Locus.CompareTo(y.Locus);
        GeneList.Sort(Comparison);

        PrintPhenome();
        
	}

    private float elapsed = 0.0f;

	// Update is called once per frame
	void Update ()
    {

    }

    public int GetDesirability()
    {
        int Desirability = 0;
        foreach (Gene x in GeneList)
        {
            Desirability += x.Desirability;
        }

        return Desirability;
    }

    public int GetSurvivability()
    {
        int Survivability = 0;
        foreach (Gene x in GeneList)
        {
            Survivability += x.Survivability;
        }

        return Survivability;
    }

    //Iteratively compute punnett square (4 gene possibilities) for each locus and 
    // select new Gene for organism based on probability: .25 probability per gene
    public void SpawnOrganism(Genotype other)
    {
        //TODO: Punnet Square computation and organism creation

        GameObject Child = (GameObject)Instantiate(transform, transform.position + new Vector3(2, 0, 0), transform.rotation);
    }

    public void PrintPhenome()
    {

        String msg = "Printing Out Phenome (Gene Name and expressed Survivability/Desirability) \n";
        foreach (Gene x in GeneList)
        {
            msg += (x.GeneName + " [ Desirability: " + x.Desirability + ", Survivability: " + x.Survivability + "] \n");
        }

        Debug.Log(msg);
    }
}
