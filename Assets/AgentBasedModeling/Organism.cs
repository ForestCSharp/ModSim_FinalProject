using UnityEngine;
using System.Collections;


//Encapsulates Data and behavior of an Organism
//
[RequireComponent (typeof(Genotype))]
[RequireComponent (typeof(SimpleMoveTo))]
public class Organism : MonoBehaviour {

    //Species of same ID can breed
    public int SpeciesID;

    public int BaseSpeed = 1000;

    private Genotype Genotype;

    private SimpleMoveTo Mover;

    // Use this for initialization
    //Cache the genotype for passing to breed function
    void Start()
    {
        Mover = GetComponent<SimpleMoveTo>();
        Genotype = GetComponent<Genotype>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Roaming Function
    void Roam()
    {

    }

    //Eating Function
    void Eat()
    {

    }

    //Breeding Function
    void Breed()
    {
        //Find mate and move towards


    }

    void Spawn(Organism other)
    {
        Genotype.SpawnOrganism(other.Genotype);
    }

}
