using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Encapsulates Data and behavior of an Organism
//
[RequireComponent(typeof(Genotype))]
[RequireComponent(typeof(SimpleMoveTo))]
public class Organism : MonoBehaviour
{

    //Species of same ID can breed
    public int SpeciesID;

    public float RoamRadius = 2;
    public int BaseSpeed = 1000;
    public int BaseDesirability = 1;
    public int BaseSurvivability = 1;

    public float HungerRate = 5.0f;
    public float BreedRate = 10.0f;
    public float RoamRate = 2.0f;

    //Type of Organism this organism likes to eat
    public Organism Prey;


    //Computed via genome
    private int ActualDesirability;
    private int ActualSurvivability;

    private float HungerMeter = 0.0f;
    private float BreedMeter = 0.0f;
    private float RoamMeter = 0.0f;

    private Genotype Genotype;

    private SimpleMoveTo Mover;

    private Vector3 RoamTarget;

    // Use this for initialization
    //Cache the genotype for passing to breed function
    void Start()
    {

        Mover = GetComponent<SimpleMoveTo>();
        Genotype = GetComponent<Genotype>();

        ActualSurvivability = BaseSurvivability + Genotype.GetSurvivability();
        ActualDesirability = BaseDesirability + Genotype.GetDesirability();

        RoamTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HungerMeter += Time.deltaTime;
        BreedMeter += Time.deltaTime;
        RoamMeter += Time.deltaTime;

        if (HungerMeter >= HungerRate) //called until food is actually consumed, at which point Eat() resets HungerMeter to 0.0f
        {
            Eat();
        }
        else if (BreedMeter >= BreedRate) //called until mate is found and new organism is spawned, at which point Breed() resets MateMeter to 0.0f;
        {
            Breed();
        }
        else if (RoamMeter >= RoamRate) //Only roam as last resort
        {
            Roam();
        }
    }

    //Roaming Function
    void Roam()
    {
        RoamTarget = transform.position + (Random.insideUnitSphere * RoamRadius);
        Mover.MoveToTarget(RoamTarget);
        RoamMeter = 0.0f;
    }

    //Eating Function: Searches for food and eats
    void Eat()
    {
        List<GameObject> PreyList = GetOrganismsOfSpecies(Prey.SpeciesID);

        //if food found, eat and set HungerMeter to 0.0f
        HungerMeter = 0.0f;
    }

    //Breeding Function
    void Breed()
    {
        //Find mate and move towards
        List<GameObject> MateList = GetOrganismsOfSpecies(SpeciesID);


        //If mate found, mate and set MateMeter to 0.0f
        BreedMeter = 0.0f;
    }

    void Spawn(Organism other)
    {
        Genotype.SpawnOrganism(other.Genotype);
    }


    List<GameObject> GetOrganismsOfSpecies(int SpeciesID)
    {
        //TODO:: IMPLEMENT
        return new List<GameObject>();
    }

}