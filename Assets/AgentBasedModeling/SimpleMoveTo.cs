using UnityEngine;
using System.Collections;

public class SimpleMoveTo : MonoBehaviour {

    Vector3 startLocation;
    Vector3 endLocation;
    float startTime;
    float duration;
    bool bMove;

    public SimpleMoveTo()
    {

    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (bMove)
        {
            transform.position = Vector3.Lerp(startLocation, endLocation, ((Time.time - startTime) / duration) * (1/(endLocation-startLocation).magnitude));
            if (transform.position == endLocation)
            {
                bMove = false;
            }
        }
       
	}

    public void MoveToTarget(Vector3 target, float Speed)
    {
        startLocation = transform.position;
        startTime = Time.time;
        endLocation = target;
        duration = 1/Speed;
        Debug.Log(endLocation);
        bMove = true;
    }
}
