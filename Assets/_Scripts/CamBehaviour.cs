using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehaviour : MonoBehaviour {

    [Range(0, 10)]
    public float speed = 4;
    public Transform[] positions = new Transform[2];


    int curPosIndex = 0;
    bool inTransition = false;
	// Use this for initialization
	void Awake () {
        GetComponent<Camera>().transparencySortMode = TransparencySortMode.CustomAxis;
        GetComponent<Camera>().transparencySortAxis = Vector3.forward;
    }

    private void Start()
    {
        curPosIndex = 0;
        ChangePosition(positions[curPosIndex]);
    }

    // Update is called once per frame
    void Update () {


        if (Input.GetKeyDown(KeyCode.P) && !inTransition)
        {
            curPosIndex++;
            StartCoroutine( ChangePosition(positions[curPosIndex % 2]));
        }
	}


    IEnumerator ChangePosition(Transform targetPos)
    {
        Debug.Log("transitioning");
        inTransition = true;
        
        while (Vector3.Distance(transform.position, targetPos.position) > 0.05f)
        {
            var curPos = transform.position;
            var curRot = transform.rotation;

            var newPos = Vector3.Lerp(curPos, targetPos.position, Time.deltaTime * speed);
            transform.position = newPos;
            var newRot = Quaternion.Lerp(curRot, targetPos.rotation, Time.deltaTime * speed);
            transform.rotation = newRot;
            yield return null;
        }

        transform.position = targetPos.position;
        transform.rotation = targetPos.rotation;

        inTransition = false;
        
    }
}
