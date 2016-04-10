using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Encapsulates Data and behavior of an Organism
//
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

    public float InteractionRadius = 10.0f;

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

    //Current State of organism
    private enum OrganismStateEnum { Roaming, Hunting, Breeding};

    private OrganismStateEnum OrganismState = OrganismStateEnum.Roaming;

    // Use this for initialization
    //Cache the genotype for passing to breed function
    void Start()
    {

        Mover = GetComponent<SimpleMoveTo>();
        Genotype = gameObject.AddComponent<Genotype>();

        StartCoroutine(DelayedStart());

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

    private Organism HuntTarget;

    //Eating Function: Searches for food and eats
    void Eat()
    {
        //Check that prey is non null
        if (Prey != null)
        {
            if (OrganismState == OrganismStateEnum.Roaming) //Acquire Target to Hunt
            {
                //Generate List of Prey
                List<Organism> PreyList = GetOrganismsOfSpecies(Prey.SpeciesID);

                //TODO: function that weighs closeness with weakness (lower Survivability)
                //Find the weakest prey and set as destination

                Organism Tmp = PreyList[0];
                foreach (Organism o in PreyList)
                {
                    if (o.ActualSurvivability < Tmp.ActualSurvivability)
                    {
                        Tmp = o;
                    }

                }
                HuntTarget = Tmp;

                //Now hunting 
                
                OrganismState = OrganismStateEnum.Hunting;
            }
            else if (OrganismState == OrganismStateEnum.Hunting) //Currently Hunting Target
            {

                if (HuntTarget != null && (transform.position - HuntTarget.transform.position).magnitude <= InteractionRadius) //Actually consume prey when close enough and not null
                {
                    //if food found, eat and set HungerMeter to 0.0f
                    Destroy(HuntTarget.gameObject);
                    HuntTarget = null;
                    transform.localScale *= 2;
                    HungerMeter = 0.0f;
                    OrganismState = OrganismStateEnum.Roaming;
                }
                else if(HuntTarget != null) //Check that prey still exists, and update destination
                {
                    Mover.MoveToTarget(HuntTarget.transform.position);
                }
                else //Hunt target null (destroyed somehow)
                {
                    OrganismState = OrganismStateEnum.Roaming;
                    HungerMeter = 0.0f;
                }
            }


        }
        else
        {
            HungerMeter = 0.0f;
        }
    }

    private Organism MateTarget;

    //Breeding Function
    void Breed()
    {
        if (OrganismState == OrganismStateEnum.Roaming)
        {
            //Generate List of Mates
            List<Organism> MateList = GetOrganismsOfSpecies(SpeciesID);
            Organism Tmp = MateList[0];
            foreach (Organism o in MateList)
            {
                if (o.ActualDesirability > Tmp.ActualDesirability)
                {
                    Tmp = o;
                }

            }
            MateTarget = Tmp;
            OrganismState = OrganismStateEnum.Breeding;

        }
        else if (OrganismState == OrganismStateEnum.Breeding)
        {
            if (MateTarget != null && (transform.position - MateTarget.transform.position).magnitude <= InteractionRadius)
            {
                Destroy(MateTarget.gameObject);
                MateTarget = null;
                BreedMeter = 0.0f;
                Spawn(MateTarget);
                OrganismState = OrganismStateEnum.Roaming;
            }
            else if (MateTarget != null)
            {
                Mover.MoveToTarget(MateTarget.transform.position);
            }
            else
            {
                OrganismState = OrganismStateEnum.Roaming;
                BreedMeter = 0.0f;
            }
        }


        //If mate found, mate and set MateMeter to 0.0f
        BreedMeter = 0.0f;
    }

    void Spawn(Organism other)
    {
        Genotype.SpawnOrganism(other.Genotype);
    }


    List<Organism> GetOrganismsOfSpecies(int SID)
    {
        List<Organism> AllOrganisms = new List<Organism>();
        AllOrganisms.AddRange(FindObjectsOfType<Organism>());

        List<Organism> ReturnedOrganisms = new List<Organism>();
        //Remove all organisms lacking SpeciesID
        foreach (Organism o in AllOrganisms)
        {
            if (o.SpeciesID == SID && o != this.GetComponent<Organism>())
            {
                ReturnedOrganisms.Add(o);
            }
        }

        return ReturnedOrganisms;
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1.0f);
        ActualDesirability = BaseDesirability + Genotype.GetDesirability();
        ActualSurvivability = BaseSurvivability + Genotype.GetSurvivability();

        Debug.Log("ActualDes: " + ActualDesirability);
        Debug.Log("ActualSurv: " + ActualSurvivability);
    }

}