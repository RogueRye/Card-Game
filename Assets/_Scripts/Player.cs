using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : MonoBehaviour {

    public bool isPlayerA = true;
    public float lifePoints;
    public Deck deck;
    public int startingHandSize = 5;
    public Transform handObj;
    public CardHolder[] cardTypes;
    public Transform optionsMenu;
    public RectTransform deckSpot;
    public GameObject TextPanel;
    public TMP_Text closeUp;

    [HideInInspector]
    public List<CardHolder> hand = new List<CardHolder>();
    Stack<CardHolder> deckStack = new Stack<CardHolder>();
    Slot[,] field; 
    float curLifePoints;


    CardHolder selectedCard;
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

        deck.Shuffle();
        
        for (int i = 0; i < deck.deckSize; i++)
        {
            
            var cardToPlace = deck.mDeck.Pop();

            var pos = deckSpot.position + (Vector3.up * (i * 0.075f));
            var deckCard = Instantiate(cardTypes[(int)cardToPlace.type], pos, deckSpot.rotation, deckSpot);

            deckCard.Init(cardToPlace, this);
            deckCard.ToggleVisible(false);
            deckStack.Push(deckCard);
        }
        DrawCard(startingHandSize);

        if (isPlayerA)
        {
            StartTurn();
        }
    }

    private void StartTurn()
    {
        DrawCard(1);
        StartCoroutine(PickingCard());
    }

    private IEnumerator PickingCard()
    {
        while(selectedCard == null)
        {

            yield return null;
        }

    }

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
#if UNITY_STANDALONE || UNITY_EDITOR

        input1 = Input.GetMouseButtonDown(0);
        input2 = Input.GetMouseButtonDown(1);



#elif UNITY_ANDROID || UNITY_IOS
                if(Input.touchCount > 0)
        {
            input1 = Input.GetTouch(0).tapCount == 1;
            input2 = Input.GetTouch(0).tapCount > 1;
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

            if (hand.Count > 7)
            {
                deckStack.Pop();
                //Send card to Graveyard
                return;
            }
            if (deckStack.Count == 0)
            {
                Debug.Log("out of cards");
                return;
            }

           
            var newCard = deckStack.Pop();
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


    /// <summary>
    /// Check to see if you clicked on something
    /// </summary>
    public void RayCastForSelection()
    {
        var pos = Camera.main.ViewportPointToRay(Input.mousePosition);
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
                SelectCard(clickedCard);
              
            }  
        }
        //Debug.Log(selectedCard);
    }

    public void SelectCard(CardHolder card)
    {

        DelselectCard();   
        selectedCard = card;
        optionsMenu.gameObject.SetActive(true);
        optionsMenu.gameObject.transform.position = Input.mousePosition;
        
        selectedCard.transform.position += (Vector3.up * 2.5f);
    }

    public void DelselectCard()
    {
        if (selectedCard == null)
            return;
      
        selectedCard.transform.position -= (Vector3.up * 2.5f);
        selectedCard = null;
        optionsMenu.gameObject.SetActive(false);
       

    }

    public void ShowText()
    {
        if (selectedCard == null)
            return;

        TextPanel.SetActive(true);
        closeUp.text = selectedCard.thisCard.description;
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
