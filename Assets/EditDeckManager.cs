using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditDeckManager : MonoBehaviour
{

    [SerializeField]
    Transform deckSlotParent;
    [SerializeField]
    InventoryCard holderPrefab;



    public void DisplayDeckToEdit()
    {
        var slots = deckSlotParent.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {           
            if (ProfileData.currentProfile.deckCardNames.Count > i && ProfileData.currentProfile.deckCardNames[i] != null)
            {
                var newCard = Instantiate(holderPrefab, deckSlotParent.parent.parent);
                newCard.AssignNewCard(LoadCard(ProfileData.currentProfile.deckCardNames[i]));
                newCard.transform.position = slots[i].gameObject.transform.position;
                newCard.isInDeck = true;
                newCard.m_Slot = slots[i];
                slots[i].locked = true;
            }
            else
            {
                slots[i].locked = false;
            }
        }

    }

    public void ClearPrevious()
    {
        var prevCards = deckSlotParent.parent.parent.GetComponentsInChildren<InventoryCard>();
        for (int i = 0; i < prevCards.Length; i++)
        {
            Destroy(prevCards[i].gameObject);
        }
    }

    Card LoadCard(string cardName)
    {
        Card r = null;
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
                r = newCard;
            }
        }

        return r;
    }
}
