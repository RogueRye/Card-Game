﻿using System.Collections;
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
                if (cardsPlayed >= 1 || controller.GetAP() < 1)
                    controller.EndTurn();
                else if (controller.selectedCard == null)
                    PickRandomCard();
                break;
            case TurnPhase.Casting:
                if(controller.selectedCard is CreatureCard)
                    PickSlot();
                break;
            case TurnPhase.Combat:
                PickAttacker();
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

        int i = 1;
        foreach(var slot in controller.field)
        {
            var rand = Random.Range(0, 100);            
            if (!slot.IsBlocked && rand <= 10 * i)
            {
                controller.selectedSlot = slot;
                controller.selectedCard.ToggleVisible(true);
                break;
            }
            i++;
        }

        if (controller.selectedSlot == null)
            controller.DelselectCard(true);

       // controller.EndTurn();
        yield return null;
        isPickingSlot = false;
      
    }

    void PickAttacker()
    {
        if (!isPickingCard)
            StartCoroutine(AIPickAttacker());
    }

    IEnumerator AIPickAttacker()
    {
        isPickingCard = true;

        foreach(var potential in controller.creaturesOnField)
        {
            if (!potential.canAttack)
            {
                continue;
            }

        }

        yield return null;

        isPickingCard = false;
    }

}