using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Deck/New Deck")]
public class Deck : ScriptableObject {


    [SerializeField]
    private List<Card> deckList;

    public Stack<Card> mDeck = new Stack<Card>();


    public void Init()
    {
        
        foreach(Card card in deckList)
        {
           
            mDeck.Push(card);

        }   
    }


    public void Shuffle()
    {
       
        System.Random rnd = new System.Random();
        var values = mDeck.ToArray();
        
        mDeck.Clear();
        foreach (var value in values.OrderBy(x => rnd.Next()))
        {
            
            mDeck.Push(value);
        }
    }
}



