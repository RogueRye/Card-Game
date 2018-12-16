using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class CardCollection : MonoBehaviour {

    public InventoryCard holderPrefab;

    public InventoryCard displayCard;
    public GameObject textPanel;
    ScrollRect scrollRect;
    Transform parentForCards;
   

    // Use this for initialization
    void Start () {

        scrollRect = GetComponent<ScrollRect>();
        parentForCards = scrollRect.content;
        
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

            var newCard = Resources.Load("Cards/" + result, typeof(Card)) as Card;
            var newCardData = new CardData(newCard);
            if (ProfileData.currentProfile.cardCollection.ContainsKey(newCardData.cardName))
            {
                var newHolder = Instantiate(holderPrefab, parentForCards);
                newHolder.transform.localPosition = Vector3.zero;
                newHolder.transform.localRotation = Quaternion.Euler(Vector3.zero);
                newHolder.Init(newCard);
                newHolder.onTouch.AddListener(() => ShowText(newHolder));
            }

        }
	}


    /// <summary>
    /// Card text can be hard to read, this will show it in an easier to read panel
    /// </summary>
    public void ShowText(InventoryCard cardToShow)
    {

        if (textPanel != null)
        {
            textPanel.SetActive(true);            
            displayCard.Init(cardToShow.thisCard);            
        }
    }

    public void DebugDeck()
    {
        foreach(var card in ProfileData.currentProfile.deckCardNames)
        {
            Debug.Log(card);
        }
    }
}
