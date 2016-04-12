using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent) ) ]
public class SimpleMoveTo : MonoBehaviour {

    public GameObject Target;

    // Use this for initialization
    void Start () {
        //StartCoroutine(MyCoroutine());
    }

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(2);
        MoveToTarget(Target.transform.position);
    }

    // Update is called once per frame
    void Update () {
	}

    public void MoveToTarget(Vector3 target)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.destination = target;
        }
    }
}
