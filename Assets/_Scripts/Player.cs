using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : MonoBehaviour {

    public bool isPlayerA = true;
    public float lifePoints;
    public int maxAP = 10;
    public Deck deck;
    public int startingHandSize = 5;
    public Transform handObj;
    public CardHolder[] cardTypes;
    public Transform optionsMenu;
    public RectTransform deckSpot;
    public RectTransform gySpot; 
    public GameObject TextPanel;
    public TMP_Text closeUp;

    [HideInInspector]
    public List<CardHolder> hand = new List<CardHolder>();
    Stack<CardHolder> deckStack = new Stack<CardHolder>();
    Stack<CardHolder> gyStack = new Stack<CardHolder>();
    Slot[,] field; 
    float curLifePoints;

    [HideInInspector]
    public CardHolder selectedCard;
    [HideInInspector]
    public Slot selectedSlot;
    [HideInInspector]
    public bool isSelecting = false;
    [HideInInspector]
    public int currentAP;

    HorizontalLayoutGroup layout;

    #region Inputs
    bool input1;
    bool input2;
    #endregion


    private void Start()
    {
        layout = handObj.GetComponent<HorizontalLayoutGroup>();
        deck.Init();
        curLifePoints = lifePoints;
        field = (isPlayerA) ? Board.fieldA : Board.fieldB;

        foreach(var slot in field)
        {
            slot.Unlock();
            slot.owner = this;
        }
        
        deck.Shuffle();

        StackDeck();
      
        DrawCard(startingHandSize);

        if (isPlayerA)
        {
            StartTurn();
        }
    }

    public void StartTurn()
    {
        DrawCard(1);
        currentAP = maxAP;
        StartCoroutine(PickingCard());
    }

    public void EndTurn()
    {

    }


    #region Turn States
    private IEnumerator PickingCard()
    {
        while(selectedCard == null)
        {

            yield return null;
        }        
    }

    private IEnumerator CastingCard()
    {

        isSelecting = true;
        optionsMenu.gameObject.SetActive(false);
        while (selectedSlot == null)
        {
            if(selectedCard == null)
            {
                break;
            }

            //Draw line
            yield return null;
        }
        isSelecting = false;
        if (selectedCard != null && selectedSlot.currentCard == null)
        {
            selectedCard.Cast(selectedSlot);
            selectedSlot.Block();
            DelselectCard();
            selectedSlot = null;
        }
       
    }

    #endregion

    private void Update()
    {
        GetInput();
        if (input2)
        {
            DelselectCard();
        }
    }



    void GetInput()
    {
#if UNITY_STANDALONE

        input1 = Input.GetMouseButtonDown(0);
        input2 = Input.GetMouseButtonDown(1);



#elif UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
                if(Input.touchCount > 1)
        {
           // input1 = Input.GetTouch(0).phase == TouchPhase.Began;
            input2 = Input.GetTouch(1).phase == TouchPhase.Began;
        }
#endif
    }

    /// <summary>
    /// Add card from deck to hand
    /// </summary>
    /// <param name="numOfCards">Number of cards to draw</param>
    public void DrawCard(int numOfCards)
    {  
        for (int i = 0; i < numOfCards; i++)
        {
            if (deckStack.Count == 0)
            {
                Debug.Log("out of cards");
                return;
            }
          
            var newCard = deckStack.Pop();

            if (hand.Count > 7)
            {
                DiscardCard(newCard);
                newCard.ToggleVisible(true);              
            }
            else
            {
                hand.Add(newCard);
                newCard.transform.SetParent(handObj);
                newCard.ToggleVisible(true);
                newCard.transform.localRotation = Quaternion.Euler(Vector3.up * 180);
                newCard.transform.localPosition = Vector3.zero - (Vector3.forward * .01f * hand.Count);

                if (hand.Count != 0 && hand.Count > 6)
                    layout.spacing -= (.7f);
                else if (hand.Count < 6)
                    layout.spacing = .7f;
            }
        }   
    }

    /// <summary>
    /// Send Card to the graveyard and add it to the GY stack/list
    /// </summary>
    /// <param name="card">card to discard</param>
    public void DiscardCard(CardHolder card)
    {
        //Unselect it, add to to stack, reset transform to the graveyard
        DelselectCard();
        gyStack.Push(card);
        card.transform.parent = gySpot;
        var pos = gySpot.position + (Vector3.up * (gyStack.Count * 0.075f));
        card.transform.position = pos;
        card.transform.rotation = gySpot.rotation;

        //Remove card from hang
        if (hand.Contains(card))
        {
            hand.Remove(card);
        }
        
    }

    /// <summary>
    /// Default override to discard the currently selected card. Used for UI buttons/Testing
    /// </summary>
    public void DiscardCard()
    {       
        if (selectedCard != null)
        {
            DiscardCard(selectedCard);
        }
    }



    /// <summary>
    /// Pick the card, show options menu, move card position to indicate selection
    /// </summary>
    /// <param name="card">card to select</param>
    public void SelectCard(CardHolder card)
    {
        if (hand.Contains(card))
        {
            DelselectCard();
            selectedCard = card;
            optionsMenu.gameObject.SetActive(true);
            optionsMenu.gameObject.transform.position = Input.mousePosition;
            selectedCard.transform.position += (Vector3.up * 2.5f);
        }
        else
        {
            // Do some attacking things
        }
    }
    /// <summary>
    /// move a selected card back and hide options menu when the card is no longer selected
    /// </summary>
    public void DelselectCard()
    {
        if (selectedCard == null)
            return;
      
        selectedCard.transform.position -= (Vector3.up * 2.5f);
        selectedCard = null;
        optionsMenu.gameObject.SetActive(false);
       

    }

    /// <summary>
    /// Card text can be hard to read, this will show it in an easier to read panel
    /// </summary>
    public void ShowText()
    {
        
        if (selectedCard == null)
            return;

        TextPanel.SetActive(true);
       
        closeUp.text = selectedCard.thisCard.description;
    }

    public void CastCard()
    {
        StartCoroutine(CastingCard());
    }

    public void TakeDamage(float power)
    {
        curLifePoints -= power;
    }

    public float GetLifePoints()
    {
        return curLifePoints;
    }

    private void StackDeck()
    {
        for (int i = 0; i < deck.deckSize; i++)
        {

            var cardToPlace = deck.mDeck.Pop();

            var pos = deckSpot.position + (Vector3.up * (i * 0.075f));
            var deckCard = Instantiate(cardTypes[(int)cardToPlace.type], pos, deckSpot.rotation, deckSpot);

            deckCard.Init(cardToPlace, this);
            deckCard.ToggleVisible(false);
            deckStack.Push(deckCard);
        }
    }
   
}
