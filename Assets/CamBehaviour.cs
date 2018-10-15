using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehaviour : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<Camera>().transparencySortMode = TransparencySortMode.CustomAxis;
        GetComponent<Camera>().transparencySortAxis = Vector3.forward;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
