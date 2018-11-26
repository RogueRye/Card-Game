using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class CardCollection : MonoBehaviour {

    public CardHolder holderPrefab;

    ScrollRect scrollRect;
    Transform parentForCards;

	// Use this for initialization
	void Start () {
        scrollRect = GetComponent<ScrollRect>();

        parentForCards = scrollRect.content;

        List<Card> collection = new List<Card>();
        
        var path = "Assets/Resources/Cards/";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(var f in info)
        {
            var newCard = Resources.Load(f.Name, typeof(Card)) as Card;

            var newHolder = Instantiate(holderPrefab, parentForCards);

            newHolder.thisCard = newCard;
            newHolder.CreateCard();

        }

        


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
