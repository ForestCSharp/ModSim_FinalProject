using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Encapsulates Data and behavior of an Organism
//
[RequireComponent(typeof(SimpleMoveTo))]
public class Organism : MonoBehaviour
{

    //Species of same ID can breed
    public uint SpeciesID;

    public float RoamRadius = 2;
    public int BaseDesirability = 1;
    public int BaseSurvivability = 1;

    public float HungerRate = 5.0f;
    public float BreedRate = 10.0f;
    public float RoamRate = 2.0f;
    public float LifeSpan = 30.0f;
    public float RateRandomness = 0.5f;

    public float InteractionRadius = 10.0f;

    //Type of Organism this organism likes to eat
    public int PreyID = -1;


    //Computed via genome
    private int ActualDesirability;
    private int ActualSurvivability;

    private float ActualLifeSpan;

    private float HungerMeter;
    private float BreedMeter;
    private float RoamMeter;
    private float LifespanMeter;

    private Genotype genotype;

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
        genotype = gameObject.AddComponent<Genotype>();

        StartCoroutine(DelayedStart());

        HungerMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness); 
        BreedMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
        RoamMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);

        ActualLifeSpan = LifeSpan + Random.Range(0.0f, LifeSpan / 5.0f);

    }

    // Update is called once per frame
    void Update()
    {

        HungerMeter += Time.deltaTime;
        BreedMeter += Time.deltaTime;
        RoamMeter += Time.deltaTime;
        LifespanMeter += Time.deltaTime;
           
        if (LifespanMeter >= ActualLifeSpan)
        {
            Destroy(this.gameObject);
        }

        if (HungerMeter >= HungerRate && OrganismState != OrganismStateEnum.Breeding) //called until food is actually consumed, at which point Eat() resets HungerMeter to 0.0f
        {
            Eat();
        }
        else if (BreedMeter >= BreedRate && OrganismState != OrganismStateEnum.Hunting) //called until mate is found and new organism is spawned, at which point Breed() resets MateMeter to 0.0f;
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
        RoamMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
    }

    private Organism HuntTarget;

    //Eating Function: Searches for food and eats
    void Eat()
    {
        //Check that prey is non null
        if (PreyID != -1)
        {
            if (OrganismState == OrganismStateEnum.Roaming) //Acquire Target to Hunt
            {
                //Generate List of Prey
                List<Organism> PreyList = GetOrganismsOfSpecies(PreyID);

                //Find the weakest prey and set as destination (tie breaks broken by proximity)

                Organism Tmp;
                if (PreyList.Count == 0)
                {
                    HungerMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
                    return;
                }
                Tmp= PreyList[0];
                foreach (Organism o in PreyList)
                {

                    bool IsWeaker = o.ActualSurvivability < Tmp.ActualSurvivability;

                    //Tiebreak condition setup
                    float DistanceFromTmp = (Tmp.transform.position - transform.position).magnitude;
                    float DistanceFromCur = (o.transform.position - transform.position).magnitude;
                    bool ProximityTieBreak = (o.ActualSurvivability == Tmp.ActualSurvivability) && (DistanceFromCur < DistanceFromTmp);
                    //o is new temp if survivability is 
                    if (o != null && (IsWeaker || ProximityTieBreak))
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
                    transform.localScale *= 1.1f;
                    OrganismState = OrganismStateEnum.Roaming;
                    HungerMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
                }
                else if(HuntTarget != null) //Check that prey still exists, and update destination
                {
                    Mover.MoveToTarget(HuntTarget.transform.position);
                    OrganismState = OrganismStateEnum.Hunting;
                }
                else //Hunt target null (destroyed somehow)
                {
                    OrganismState = OrganismStateEnum.Roaming;
                    HungerMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
                }
            }
        }
        else
        {
            HungerMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
            OrganismState = OrganismStateEnum.Roaming;
        }
    }

    private Organism MateTarget;

    //Breeding Function
    void Breed()
    {
        if (OrganismState == OrganismStateEnum.Roaming)
        {
            //Generate List of Mates
            List<Organism> MateList = GetOrganismsOfSpecies((int)SpeciesID);
            Organism Tmp;
            if (MateList.Count == 0)
            {
                BreedMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
                return;
            }
            Tmp = MateList[0];
            foreach (Organism o in MateList)
            {
                bool IsMoreDesirable = o.ActualDesirability > Tmp.ActualDesirability;

                //Tiebreak condition setup
                float DistanceFromTmp = (Tmp.transform.position - transform.position).magnitude;
                float DistanceFromCur = (o.transform.position - transform.position).magnitude;
                bool ProximityTieBreak = (o.ActualDesirability == Tmp.ActualDesirability) && (DistanceFromCur < DistanceFromTmp);

                if (o != null && (IsMoreDesirable || ProximityTieBreak))
                {
                    Tmp = o;
                }

            }
            MateTarget = Tmp;
            OrganismState = OrganismStateEnum.Breeding;

        }
        else if (OrganismState == OrganismStateEnum.Breeding)
        {
            if (MateTarget != null && (transform.position - MateTarget.transform.position).magnitude <= InteractionRadius) //Mate close enough
            {
                BreedMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
                Spawn(MateTarget);
                MateTarget = null;
                OrganismState = OrganismStateEnum.Roaming;
            }
            else if (MateTarget != null)
            {
                Mover.MoveToTarget(MateTarget.transform.position);
                OrganismState = OrganismStateEnum.Breeding;

            }
            else
            {
                OrganismState = OrganismStateEnum.Roaming;
                BreedMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
            }
        }

        //If mate found, mate and set MateMeter to 0.0f
        BreedMeter = 0.0f - UnityEngine.Random.Range(0.0f, RateRandomness);
    }

    void Spawn(Organism other)
    {
        genotype.SpawnOrganism(other.genotype);
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
        ActualDesirability = BaseDesirability + genotype.GetDesirability();
        ActualSurvivability = BaseSurvivability + genotype.GetSurvivability();

    }

    public List<Gene> GetGeneList()
    {
        return genotype.GetGenelist();
    }

}