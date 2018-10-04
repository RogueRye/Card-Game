using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatureCard : CardHolder
{

    public TextMeshPro attack;
    public TextMeshPro health;

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


}
