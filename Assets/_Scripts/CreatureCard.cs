using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatureCard : CardHolder
{

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
       
        transform.SetParent(targetSlot.transform);
        transform.position = targetSlot.transform.position + Vector3.up * 2.51f;
        transform.localRotation = Quaternion.Euler(Vector3.right * -180);
        targetSlot.currentCard = this;
        thisPlayer.hand.Remove(this);

    }

    public void Attack(CreatureCard target)
    {
        if( target.thisCardC.TakeDamage(thisCardC.attackValue) <= 0)
        {
            target.thisPlayer.DiscardCard(target);
        }
    }

    public void Attack(Player target)
    {
        target.TakeDamage(thisCardC.attackValue);
    }
}
