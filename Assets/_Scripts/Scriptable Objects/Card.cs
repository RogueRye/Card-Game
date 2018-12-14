using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Card : ScriptableObject {

    public string cardName = "Lion";
    public CardTypes type = CardTypes.Creature;
    public int castCost;
    [TextArea]
    public string description;
    public Sprite sprite;
    public GameObject model; // will be some kind of FX for spells

}
