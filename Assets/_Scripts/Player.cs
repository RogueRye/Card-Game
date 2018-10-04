using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool isPlayerA = true;
    public float lifePoints;
    public Deck deck;
    public int startingHandSize = 5;
    public Transform handObj;
    public CardHolder[] cardTypes;


    List<CardHolder> hand = new List<CardHolder>();

    Slot[,] field; 
    float curLifePoints;

    private void Start()
    {
        curLifePoints = lifePoints;
        field = (isPlayerA) ? Board.fieldA : Board.fieldB;

        deck.Shuffle();

        DrawCard(startingHandSize);

    }



    public void DrawCard(int numOfCards)
    {
        for (int i = 0; i < numOfCards; i++)
        {
            var cardToDraw = deck.mDeck.Pop();

            switch(cardToDraw.type)
            {
                case CardTypes.Creature:
                    break;
                case CardTypes.Spell:
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
