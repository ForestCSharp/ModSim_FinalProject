using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulationQueryHelper : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateGeneticStatistics(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GenerateGeneticStatistics(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GenerateGeneticStatistics(3);
        }
	}

    void GenerateGeneticStatistics(int SID)
    {
        List<Organism> Organisms = GetOrganismsOfSpecies(SID);

        List<Gene> GeneList = Organisms[0].GetGeneList();

        string outputMsg = "SpeciesID: " + Organisms[0].SpeciesID + " Elapsed: " + Time.time + "\n";

        for (int i=0;  i<GeneList.Count; ++i)
        {
            SortedDictionary<string, int> GenotypeOccurrences = new SortedDictionary<string, int>();
            SortedDictionary<string, int> PhenotypeOccurrences = new SortedDictionary<string, int>();

            foreach (Organism o in Organisms)
            {
                string key_1 = o.GetGeneList()[i].First.AlleleName;
                string key_2 = o.GetGeneList()[i].Second.AlleleName;
                string key_pheno = ((o.GetGeneList()[i].First.Dominance >= o.GetGeneList()[i].Second.Dominance) ? key_1 : key_2);

                //FIRST ALLELE
                int curVal1;
                if (GenotypeOccurrences.TryGetValue(key_1, out curVal1))
                {
                    GenotypeOccurrences[key_1] = (curVal1 + 1);
                }
                else
                {
                    GenotypeOccurrences.Add(key_1, 1);
                }

                //SECOND ALLELE
                int curVal2;
                if (GenotypeOccurrences.TryGetValue(key_2, out curVal2))
                {
                    GenotypeOccurrences[key_2] = (curVal2 + 1);
                }
                else
                {
                    GenotypeOccurrences.Add(key_2, 1);
                }

                //EXPRESSED ALLELE
                int curValP;
                if (PhenotypeOccurrences.TryGetValue(key_pheno, out curValP))
                {
                    PhenotypeOccurrences[key_pheno] = (curValP + 1);
                }
                else
                {
                    PhenotypeOccurrences.Add(key_pheno, 1);
                }
            }

            outputMsg += "GeneName: " + GeneList[i].GeneName +"\n\n";
            outputMsg += "Genotype Occurrences \n";
            foreach (KeyValuePair<string, int> entry in GenotypeOccurrences)
            {
                // do something with entry.Value or entry.Key
                outputMsg += ("Allele: " + entry.Key + " " + entry.Value +  "\n");
            }

            outputMsg += "\n";

            outputMsg += "Phenotype Occurrences \n";
            foreach (KeyValuePair<string, int> entry in PhenotypeOccurrences)
            {
                // do something with entry.Value or entry.Key
                outputMsg += ("Allele: " + entry.Key + " " + entry.Value + "\n");
            }

        }

        Debug.Log(outputMsg);

    }

    List<Organism> GetOrganismsOfSpecies(int SID)
    {
        List<Organism> AllOrganisms = new List<Organism>();
        AllOrganisms.AddRange(FindObjectsOfType<Organism>());

        List<Organism> ReturnedOrganisms = new List<Organism>();
        //Remove all organisms lacking SpeciesID
        foreach (Organism o in AllOrganisms)
        {
            if (o.SpeciesID == SID)
            {
                ReturnedOrganisms.Add(o);
            }
        }

        return ReturnedOrganisms;
    }
}
