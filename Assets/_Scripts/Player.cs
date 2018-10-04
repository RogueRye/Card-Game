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

    List<CardHolder> hand = new List<CardHolder>();
    
    Slot[,] field; 
    float curLifePoints;

    private void Start()
    {
        deck.Init();
        curLifePoints = lifePoints;
        field = (isPlayerA) ? Board.fieldA : Board.fieldB;

        deck.Shuffle();

        DrawCard(startingHandSize);

    }



    public void DrawCard(int numOfCards)
    {
        
        for (int i = 0; i < numOfCards; i++)
        {
            //if(i < deck.mDeck.Count)
            //{
            //    Debug.Log("out of cards");
            //    return;
            //}
            var cardToDraw = deck.mDeck.Pop();
            //Debug.Log(cardToDraw);
            switch(cardToDraw.type)
            {
                case CardTypes.Creature:
                    CreatureCard temp;
                    temp = (CreatureCard)Instantiate(cardTypes[0], handObj);
                    temp.thisCard = (Creature)cardToDraw;
                    temp.CreateCard();
                    hand.Add(temp);

                    break;
                case CardTypes.Spell:
                    CardHolder temp2;
                    temp2 = Instantiate(cardTypes[1], handObj);
                    temp2.thisCard = cardToDraw;
                    temp2.CreateCard();
                    hand.Add(temp2);
                    break;               
                  
            }

        }     


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
