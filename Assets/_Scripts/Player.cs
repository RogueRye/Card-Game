using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool isPlayerA = true;
    public float lifePoints;
    public Deck deck;



    List<Card> hand = new List<Card>();
    Slot[,] field; 
    float curLifePoints;

    private void Start()
    {
        curLifePoints = lifePoints;
        field = (isPlayerA) ? Board.fieldA : Board.fieldB;

        deck.Shuffle();

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
