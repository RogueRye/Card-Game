using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcBrain : MonoBehaviour
{

    Player controller;

    bool isPickingCard = false;
    bool isPickingSlot = false;
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
        switch (controller.currentPhase)
        {
            case TurnPhase.Main:
                if (controller.selectedCard == null)
                    PickRandomCard();
                break;
            case TurnPhase.Casting:
                PickSlot();
                break;
            case TurnPhase.Combat:
                break;
            case TurnPhase.Attacking:
                break;
            case TurnPhase.End:
                break;
        }
    }

    void PickRandomCard()
    {
        if (!isPickingCard)
            StartCoroutine(AIPickCard());
    }

    IEnumerator AIPickCard()
    {
        isPickingCard = true;

        var randSeed = Random.Range(0, controller.hand.Count);
        controller.SelectCard(controller.hand[randSeed]);
        yield return null;


        controller.CastCard();
       
        isPickingCard = false;
    }

    void PickSlot()
    {
        if (!isPickingSlot)
        {

            isPickingSlot = true;

            foreach (var slot in controller.field)
            {
                if (!slot.IsBlocked)
                {
                    controller.selectedSlot = slot;
                    controller.selectedCard.ToggleVisible(true);
                }
            }

            controller.EndTurn();
            isPickingSlot = false;
            //  StartCoroutine(AIPickSlot());
        }
    }

    IEnumerator AIPickSlot()
    {
        isPickingSlot = true;
        


        foreach(var slot in controller.field)
        {
            if (!slot.IsBlocked)
            {
                controller.selectedSlot = slot;
                controller.selectedCard.ToggleVisible(true);
            }
        }
        controller.EndTurn();
        isPickingSlot = false;
        yield return null;
    }

}
