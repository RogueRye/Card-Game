using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class CardHolder : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public Card thisCard;
    public Player thisPlayer;
    public Player otherPlayer;
    public SpriteRenderer artWork;
    public Image imageArt;
    public TMP_Text nameDisplay;
    public TMP_Text costDisplay;
    public TMP_Text description;

    Image cardGraphics;
    [HideInInspector]
    public bool inSlot = false;

    public void Init(Card mCard, Player mPlayer, Player oPlayer)
    {
        thisCard = mCard;
        otherPlayer = oPlayer;
        thisPlayer = mPlayer;
        CreateCard();
        cardGraphics = GetComponent<Image>();
    }

    public virtual void CreateCard()
    {        
        if(artWork != null)
            artWork.sprite = thisCard.sprite;
        if (imageArt != null)
            imageArt.sprite = thisCard.sprite;
        nameDisplay.text = thisCard.cardName;
        costDisplay.text = thisCard.castCost.ToString();
        description.text = thisCard.description;
    }

    public abstract void Cast();

    public abstract void Cast(Slot targetSlot);

    public virtual void ToggleVisible(bool isVisible)
    {
        transform.Find("CardBack").gameObject.SetActive(!isVisible);

    }

    public virtual bool IsVisible()
    {
        return !transform.Find("CardBack").gameObject.activeInHierarchy;
    }

    public void Hover()
    {
        if (!Input.GetMouseButton(0))
        {
            if (thisPlayer.hand.Contains(this) && IsVisible() && thisPlayer.currentPhase == TurnPhase.Main)
            {
                var newColor = cardGraphics.color;
                newColor.a = 70;
                cardGraphics.color = newColor;
            }

            else if (inSlot && thisPlayer.currentPhase == TurnPhase.Combat)
            {
                cardGraphics.color = Color.red;
            }
        }

    }

   

    public void StopHover()
    {
        if (thisPlayer.hand.Contains(this))
        {
            var newColor = cardGraphics.color;
            newColor.a = 0;
            cardGraphics.color = newColor;
        }
        else if (inSlot)
        {
            var newColor = new Color(0, 255, 255, 0);            
            cardGraphics.color = newColor;
        }
    }

    public void SelectCard()
    {       
        thisPlayer.SelectCard(this);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
