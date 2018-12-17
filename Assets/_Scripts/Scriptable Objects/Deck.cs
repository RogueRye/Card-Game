using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.IO;

[CreateAssetMenu(menuName = "Deck/New Deck")]
public class Deck : ScriptableObject {


    [SerializeField]
    private List<Card> deckList;
    public int deckSize; 
    public Stack<Card> mDeck = new Stack<Card>();
    public bool overrideWithProfile = true;

    public void Init()
    {
        if (overrideWithProfile)
        {
            deckList.Clear();
            foreach (var name in ProfileData.currentProfile.deckCardNames)
            {
                LoadCard(name);
            }
        }

        deckSize = deckList.Count;
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

    void LoadCard(string cardName)
    {
        var path = Application.dataPath + "/Resources/Cards/";
        DirectoryInfo dir = new DirectoryInfo(path);
        // Debug.Log(dir);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (var f in info)
        {
            var name = f.Name;
            var extension = f.Extension;
            if (extension == ".meta")
                continue;
            var result = name.Replace(extension, "");
            if (result == cardName)
            {
                var newCard = Resources.Load("Cards/" + result, typeof(Card)) as Card;
                deckList.Add(newCard);
            }
        }
    }
}



