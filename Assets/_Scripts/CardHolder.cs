using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardHolder : MonoBehaviour {

    public Card thisCard;

    public SpriteRenderer artWork;
    public TextMeshPro nameDisplay;
    public TextMeshPro costDisplay;
    public TextMeshPro description;


    public virtual void Start()
    {
      
    }


    public virtual void CreateCard()
    {
        artWork.sprite = thisCard.sprite;
        nameDisplay.text = thisCard.cardName;
        costDisplay.text = thisCard.castCost.ToString();
        description.text = thisCard.description;
    }


}
