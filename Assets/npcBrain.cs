using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcBrain : MonoBehaviour {

    Player controller;


    private void Start()
    {
        controller = GetComponent<Player>();
    }

    private void Update()
    {
        PickAction();
    }


    void PickAction()
    {
        switch(controller.currentPhase)
        {
            case TurnPhase.Main:
                break;
            case TurnPhase.Casting:
                break;
            case TurnPhase.Combat:
                break;
            case TurnPhase.Attacking:
                break;
            case TurnPhase.End:
                break;
        }
    }

}
