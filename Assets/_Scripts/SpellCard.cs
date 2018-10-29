using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : CardHolder {


    public override void CreateCard()
    {
        base.CreateCard();
    }

    public override void Cast()
    {
        throw new System.NotImplementedException();
    }

    public override void Cast(Slot targetSlot)
    {
        throw new System.NotImplementedException();
    }
}
