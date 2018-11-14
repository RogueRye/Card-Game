using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CreatureCard : CardHolder
{

    public bool canAttack;
    public TMP_Text attack;
    public TMP_Text health;

    public Creature thisCardC;
    public Slot currentSlot;
    GameObject model;


    Vector3 prevPosition;

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
            currentSlot = targetSlot;
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


    public override void OnDrag(PointerEventData eventData)
    {
        RectTransform m_DraggingPlane = thisPlayer.handObj as RectTransform;

        var rt = gameObject.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
        }      
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        prevPosition = transform.position;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {

        if (thisPlayer.selectedSlot != null)
            thisPlayer.CastCard();
        else
        {
            transform.position = prevPosition;
            thisPlayer.DelselectCard(true);
        }

    }

}
