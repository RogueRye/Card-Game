using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Deck/New Deck")]
public class Deck : ScriptableObject {

    public Card[] mDeck = new Card[40];

}
