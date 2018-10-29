using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatureCard : CardHolder
{

    public TMP_Text attack;
    public TMP_Text health;

    Creature thisCardC;

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
        gameObject.transform.position = targetSlot.transform.position + Vector3.up * .01f;
        targetSlot.currentCard = this;
    }
}
