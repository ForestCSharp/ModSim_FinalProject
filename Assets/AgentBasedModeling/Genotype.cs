using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

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

        StartCoroutine(DelayedStart());
	}

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
        //Debug.Log("Desirability: " + Tmp);
        return Tmp;
    }

    public int GetSurvivability()
    {
        int Tmp = 0;
        foreach (Gene x in GeneList)
        {
            Tmp += x.GetSurvivability();
        }
        //Debug.Log("Survivability: " + Tmp);
        return Tmp;
    }

    //Iteratively compute punnett square (4 gene possibilities) for each locus and 
    // select new Gene for organism based on probability: .25 probability per gene
    public void SpawnOrganism(Genotype other)
    {
        //TODO: Punnet Square computation and organism creation
        if (CheckSameGenes(other))
        {
            //Spawn the child
            GameObject Child = new GameObject();
            Child.transform.position = transform.position + new Vector3(2,0,0);
            MeshFilter ChildMesh = Child.AddComponent<MeshFilter>();
            ChildMesh.mesh = GetComponent<MeshFilter>().mesh;
            MeshRenderer ChildMeshRenderer = Child.AddComponent<MeshRenderer>();
            ChildMeshRenderer.material = new Material(Shader.Find("Standard"));

            CopyComponent(GetComponent<BoxCollider>(), Child);
            
            //Compute child's punnett squares
            for (int i = 0; i < GeneList.Count; ++i)
            {
                Component A, B, NewGene;


                float RandomValue = UnityEngine.Random.Range(1, 5);
                if (RandomValue == 1) //Top Left Punnett Square
                {
                    A = CopyComponent(GeneList[i].First, Child);
                    B = CopyComponent(other.GeneList[i].First, Child);
                    NewGene = CopyComponent(GeneList[i], Child);
                }
                else if (RandomValue == 2) //Top Right Punnett Square
                {
                    A = CopyComponent(GeneList[i].First, Child);
                    B = CopyComponent(other.GeneList[i].Second, Child);
                    NewGene = CopyComponent(GeneList[i], Child);
                } 
                else if (RandomValue == 3) //Bottom Left Punnett Square
                {
                    A = CopyComponent(GeneList[i].Second, Child);
                    B = CopyComponent(other.GeneList[i].First, Child);
                    NewGene = CopyComponent(GeneList[i], Child);
                }
                else //Bottom Right Punnett Square
                {
                    A = CopyComponent(GeneList[i].Second, Child);
                    B = CopyComponent(other.GeneList[i].Second, Child);
                    NewGene = CopyComponent(GeneList[i], Child);
                }

                ((Gene)NewGene).First = (Allele)A;
                ((Gene)NewGene).Second = (Allele)B;

            }

            CopyComponent(GetComponent<Organism>(), Child);
            NavMeshAgent MyNav = GetComponent<NavMeshAgent>();
            NavMeshAgent ChildNav = Child.GetComponent<NavMeshAgent>();
            ChildNav.radius = MyNav.radius;
            ChildNav.height = MyNav.height;
            ChildNav.baseOffset = MyNav.baseOffset;
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

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1.0f);

        //PrintPhenome();
    }

    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

}
