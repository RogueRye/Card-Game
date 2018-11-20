using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehaviour : MonoBehaviour {


    public static CamBehaviour singleton;

    [Range(0, 10)]
    public float speed = 4;
    public Transform[] positions = new Transform[2];


    int curPosIndex = 0;
    bool inTransition = false;

    Coroutine movingCamera;
	// Use this for initialization
	void Awake () {
        if(singleton == null)
            singleton = this;

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
            TogglePosition();
        }
	}


    public void TogglePosition()
    {
        if(inTransition)
            StopCoroutine(movingCamera);
        
        curPosIndex++;
        movingCamera = StartCoroutine(ChangePosition(positions[curPosIndex % 2]));
    }
    public void SwitchToPosition(int posIndex)
    {

        curPosIndex = posIndex;
        if (inTransition)
            StopCoroutine(movingCamera);
        
        movingCamera = StartCoroutine(ChangePosition(positions[posIndex]));
    }

    IEnumerator ChangePosition(Transform targetPos)
    {
        Debug.Log("transitioning");
        inTransition = true;
        
        while (Vector3.Distance(transform.position, targetPos.position) > 0.1f)
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
