using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class CardHolder : MonoBehaviour {

    public Card thisCard;
    public Player thisPlayer;
    public SpriteRenderer artWork;
    public Image imageArt;
    public TMP_Text nameDisplay;
    public TMP_Text costDisplay;
    public TMP_Text description;

    Image cardGraphics;

    public void Init(Card mCard, Player mPlayer)
    {
        thisCard = mCard;
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
        return transform.Find("CardBack").gameObject.activeInHierarchy;
    }

    public void Hover()
    {
        if (thisPlayer.hand.Contains(this))
        {
            var newColor = cardGraphics.color;
            newColor.a = 70;
            cardGraphics.color = newColor;
        }
    }

    //public void OnMouseDown()
    //{
    //    if (thisPlayer.hand.Contains(this) && thisPlayer.selectedCard != this)
    //    {
    //        SelectCard();
    //    }
    //}


    public void StopHover()
    {
        if (thisPlayer.hand.Contains(this))
        {
            var newColor = cardGraphics.color;
            newColor.a = 0;
            cardGraphics.color = newColor;
        }
    }

    public void SelectCard()
    {
        thisPlayer.SelectCard(this);
    }

}
