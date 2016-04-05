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
    public int BaseDesirability = 1;
    public int BaseSurvivability = 1;

    public float HungerRate = 0.2f;
    public float MateRate = 0.1f;



    //Computed via genome
    private int ActualDesirability;
    private int ActualSurvivability;

    private float HungerMeter = 0.0f;
    private float MateMeter = 0.0f;

    private Genotype Genotype;

    private SimpleMoveTo Mover;

    // Use this for initialization
    //Cache the genotype for passing to breed function
    void Start()
    {
        Mover = GetComponent<SimpleMoveTo>();
        Genotype = GetComponent<Genotype>();

        ActualSurvivability = BaseSurvivability + Genotype.GetSurvivability();
        ActualDesirability = BaseDesirability + Genotype.GetDesirability();
    }
	
	// Update is called once per frame
	void Update () {
        HungerMeter += (HungerRate * Time.deltaTime);
        MateMeter += (MateRate * Time.deltaTime);

        if (HungerMeter >= 1.0f) //called until food is actually consumed, at which point Eat() resets HungerMeter to 0.0f
        {
            Eat(); 
        }
        else if ( MateMeter >= 1.0f) //called until mate is found and new organism is spawned, at which point Breed() resets MateMeter to 0.0f;
        {
            Breed();
        }
        else //Only roam as last resort
        {
            Roam();
        }
	}

    //Roaming Function
    void Roam()
    {

    }

    //Eating Function: Searches for food and eats
    void Eat()
    {

        //if food found, eat and set HungerMeter to 0.0f
    }

    //Breeding Function
    void Breed()
    {
        //Find mate and move towards


        //If mate found, mate and set MateMeter to 0.0f
    }

    void Spawn(Organism other)
    {
        Genotype.SpawnOrganism(other.Genotype);
    }

}
