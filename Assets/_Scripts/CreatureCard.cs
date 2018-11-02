using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatureCard : CardHolder
{

    public bool canAttack;
    public TMP_Text attack;
    public TMP_Text health;

    Creature thisCardC;
    GameObject model;


    public override void CreateCard()
    {
        base.CreateCard();

        if (thisCard is Creature)
        {
            thisCardC = (Creature)thisCard;
            attack.text = thisCardC.attackValue.ToString();
            health.text = thisCardC.healthValue.ToString();

        }
    }

    public override void Cast()
    {
        
    }

    public override void Cast(Slot targetSlot)
    {
        if (!targetSlot.IsBlocked && !targetSlot.IsLocked)
        {
            
            canAttack = true;
            inSlot = true;
            transform.SetParent(targetSlot.transform);
            transform.localPosition = (Vector3.forward * .1f);
            transform.localRotation = Quaternion.Euler(Vector3.right * -180);
            targetSlot.currentCard = this;
            thisPlayer.SpendAP(thisCardC.castCost);
            thisPlayer.hand.Remove(this);
        }

    }

    public void Attack(CreatureCard target)
    {

        if( target.thisCardC.TakeDamage(thisCardC.attackValue) <= 0)
        {
            target.thisPlayer.DiscardCard(target);
        }

        canAttack = false;
    }

    public void Attack(Player target)
    {
        target.TakeDamage(thisCardC.attackValue);
        canAttack = false;
    }
}
