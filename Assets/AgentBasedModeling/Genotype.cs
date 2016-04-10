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
        GeneList.AddRange(this.GetComponents<Gene>());

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
        int Tmp = 0;
        foreach (Gene x in GeneList)
        {
            Tmp += x.GetDesirability();
        }
        Debug.Log("Desirability: " + Tmp);
        return Tmp;
    }

    public int GetSurvivability()
    {
        int Tmp = 0;
        foreach (Gene x in GeneList)
        {
            Tmp += x.GetSurvivability();
        }
        Debug.Log("Survivability: " + Tmp);
        return Tmp;
    }

    //Iteratively compute punnett square (4 gene possibilities) for each locus and 
    // select new Gene for organism based on probability: .25 probability per gene
    public void SpawnOrganism(Genotype other)
    {
        //TODO: Punnet Square computation and organism creation
        if (CheckSameGenes(other))
        {
            GameObject Child = (GameObject)Instantiate(transform, transform.position + new Vector3(2, 0, 0), transform.rotation);
        }
    }

    bool CheckSameGenes(Genotype other)
    {
        for (int i = 0; i < GeneList.Count; ++i)
        {
            if (GeneList[i].Locus != other.GeneList[i].Locus)
            {
                return false;
            }
        }
        return true;
    }

    public void PrintPhenome()
    {

        for (int i=0; i < GeneList.Count; ++i)
        {
            String msg = (GeneList[i].GeneName + " [ Desirability: " + GeneList[i].GetDesirability() + ", Survivability: " + GeneList[i].GetSurvivability() + "] \n");
            Debug.Log(msg);
        }
        
    }
}
