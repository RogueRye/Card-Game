using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public float lifePoints;
    public Deck deck;

    List<Card> hand = new List<Card>();

    float curLifePoints;

    private void Start()
    {
        curLifePoints = lifePoints;

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
