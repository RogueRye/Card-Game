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
        
        var path = Application.dataPath + "/Resources/Cards/";
        DirectoryInfo dir = new DirectoryInfo(path);
       // Debug.Log(dir);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(var f in info)
        {
           

            var name = f.Name;
            var extension = f.Extension;
            if (extension == ".meta")
                continue;
            var result = name.Replace(extension, "");
            
            var newCard = Resources.Load("Cards/" + result, typeof(Card)) as Card;
            if (ProfileData.currentProfile.cardCollection.ContainsKey(newCard.cardName))
            {

                var newHolder = Instantiate(holderPrefab, parentForCards);
                newHolder.transform.localPosition = Vector3.zero;
                newHolder.transform.localRotation = Quaternion.Euler(Vector3.zero);
                newHolder.Init();
                newHolder.AssignNewCard(newCard);
            }

        }

        


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
