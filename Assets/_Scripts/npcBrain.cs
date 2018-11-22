using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcBrain : MonoBehaviour
{

    Player controller;

    bool isPickingCard = false;
    bool isPickingSlot = false;
    int cardsPlayed = 0;
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
            case TurnPhase.Start:
                cardsPlayed = 0;
                break;
            case TurnPhase.Main:
                if (cardsPlayed >= 1)
                    controller.EndTurn();
                else if (controller.selectedCard == null)
                    PickRandomCard();
                break;
            case TurnPhase.Casting:
                if(controller.selectedCard is CreatureCard)
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
        //var randSeed = Random.Range(0, controller.hand.Count);
        //controller.SelectCard(controller.hand[randSeed]);
        //if (controller.selectedCard != null)
        //{
        //    controller.CastCard();
        //    cardsPlayed++;
        //}

        if (!isPickingCard)
            StartCoroutine(AIPickCard());
    }

    IEnumerator AIPickCard()
    {
        isPickingCard = true;

        var randSeed = Random.Range(0, controller.hand.Count);
        controller.SelectCard(controller.hand[randSeed]);
        
        controller.CastCard();
        cardsPlayed++;
        yield return null;
        isPickingCard = false;
    }

    void PickSlot()
    {
        if (!isPickingSlot)
        {
             StartCoroutine(AIPickSlot());
        }
    }

    IEnumerator AIPickSlot()
    {
        isPickingSlot = true;
       

        foreach(var slot in controller.field)
        {
            var rand = Random.Range(0, 100);
            if (!slot.IsBlocked && rand <= 40)
            {
                controller.selectedSlot = slot;
                controller.selectedCard.ToggleVisible(true);
            }
        }

        if (controller.selectedSlot == null)
            controller.DelselectCard(true);
        controller.EndTurn();
        yield return null;
        isPickingSlot = false;
      
    }

}
