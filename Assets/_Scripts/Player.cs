using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public bool isPlayerA = true;
    public float lifePoints;
    public Deck deck;
    public int startingHandSize = 5;
    public Transform handObj;
    public CardHolder[] cardTypes;
    public Transform optionsMenu;
    public Transform deckSpot;
    List<CardHolder> hand = new List<CardHolder>();
    Stack<CardHolder> deckStack = new Stack<CardHolder>();
    Slot[,] field; 
    float curLifePoints;


    CardHolder selectedCard;
    HorizontalLayoutGroup layout;

    private void Start()
    {
        layout = handObj.GetComponent<HorizontalLayoutGroup>();
        deck.Init();
        curLifePoints = lifePoints;
        field = (isPlayerA) ? Board.fieldA : Board.fieldB;

        //for (int i = 0; i < deck.mDeck.Count; i++)
        //{
        //    var pos = deckSpot.position + (Vector3.up * (i * .015f));
        //    var deckCard = Instantiate(cardTypes[0], pos, deckSpot.rotation, deckSpot);

        //}

        deck.Shuffle();
        
        for (int i = 0; i < deck.deckSize; i++)
        {
            
            var cardToPlace = deck.mDeck.Pop();

            var pos = deckSpot.position + (Vector3.up * (i * 0.015f));
            var deckCard = Instantiate(cardTypes[(int)cardToPlace.type], pos, deckSpot.rotation, deckSpot);
            deckCard.thisCard = cardToPlace;
            deckCard.CreateCard();
            deckStack.Push(deckCard);
        }

        DrawCard(startingHandSize);

    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RayCastForSelection();
        }
        
    }

    public void DrawCard(int numOfCards)
    {
        
        for (int i = 0; i < numOfCards; i++)
        {
            //Deal with this later

            if (deckStack.Count == 0)
            {
                Debug.Log("out of cards");
                return;
            }

            //New way
            var newCard = deckStack.Pop();
            hand.Add(newCard);            
            newCard.transform.SetParent(handObj);
            newCard.transform.localRotation = Quaternion.Euler(Vector3.zero);
            newCard.transform.localPosition = Vector3.zero;

            if (hand.Count != 0 && hand.Count > 7)
                layout.spacing = 1.5f / (hand.Count * .15f);
            else
                layout.spacing = 1.5f;

        // Old way
            //var cardToDraw = deck.mDeck.Pop();
            ////Debug.Log(cardToDraw);
            //switch(cardToDraw.type)
            //{
            //    case CardTypes.Creature:
            //        CreatureCard temp;
            //        temp = (CreatureCard)Instantiate(cardTypes[0], handObj);
            //        temp.thisCard = (Creature)cardToDraw;
            //        temp.CreateCard();
            //        hand.Add(temp);

            //        break;
            //    case CardTypes.Spell:
            //        CardHolder temp2;
            //        temp2 = Instantiate(cardTypes[1], handObj);
            //        temp2.thisCard = cardToDraw;
            //        temp2.CreateCard();
            //        hand.Add(temp2);
            //        break;               
                  
            //}

        }   
    }

    public void RayCastForSelection()
    {
        var pos = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        DelselectCard();
        if(Physics.Raycast(pos, out hit))
        {
            if (hit.transform.gameObject.layer == 5)
                return;
            if(hit.transform.GetComponent<CardHolder>() != null)
            {
                
                var clickedCard = hit.transform.GetComponent<CardHolder>();
             
                if (clickedCard == selectedCard)
                {                    
                    return;
                }
               
                selectedCard = clickedCard;
                optionsMenu.gameObject.SetActive(true);
                optionsMenu.gameObject.transform.position = Input.mousePosition;
                selectedCard.transform.localScale *= 1.5f;
            }  
            else if(hit.transform.GetComponent<Slot>() != null)
            {

            }
        }
        Debug.Log(selectedCard);
    }


    public void DelselectCard()
    {
        Debug.Log(selectedCard);
        if (selectedCard == null)
            return;

        optionsMenu.gameObject.SetActive(false);
        selectedCard.transform.localScale /= 1.5f;
        selectedCard = null;

    }

    public void TakeDamage(float power)
    {
        curLifePoints -= power;
    }

    public float GetLifePoints()
    {
        return curLifePoints;
    }


   
}
