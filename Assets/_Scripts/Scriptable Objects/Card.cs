using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject {

    public string cardName = "Lion";
    public CardTypes type = CardTypes.Creature;
    public int castCost;
    public string description;
    public Sprite sprite;
    public GameObject model; // will be some kind of FX for spells

    /// <summary>
    /// Cast the card
    /// </summary>
    public abstract void Cast();
    
    /// <summary>
    /// Destroy the card and send it to the graveyard
    /// </summary>
    public abstract void Destroy();

}
