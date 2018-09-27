using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Deck/New Deck")]
public class Deck : ScriptableObject {

    public Card[] mDeck = new Card[40];


    

    public void Shuffle()
    {
        mDeck.Shuffle();
    }

}


namespace System
{
    public static class MSSystemExtenstions
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this T[] array)
        {
            rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n);
                n--;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
