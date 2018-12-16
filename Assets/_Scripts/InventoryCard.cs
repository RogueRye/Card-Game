using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;

public class InventoryCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler  {


    public Card thisCard;

    public Image imageArt;
    public TMP_Text nameDisplay;
    public TMP_Text costDisplay;
    public TMP_Text description;
    public TMP_Text attack;
    public TMP_Text health;

    public Creature thisCardC;

    Vector3 prevPosition;

    bool isInDeck;
    InventoryCard cloneToPlace;
    InventorySlot m_Slot;
    public UnityEvent onTouch;

    public void CreateCard()
    {
        if (imageArt != null)
            imageArt.sprite = thisCard.sprite;
        nameDisplay.text = thisCard.cardName;
        costDisplay.text = thisCard.castCost.ToString();
        description.text = thisCard.description;

        if (thisCard is Creature)
        {
            thisCardC = (Creature)thisCard;
            attack.text = thisCardC.attackValue.ToString();
            health.text = thisCardC.healthValue.ToString();

        }
    }
    public void Init(Card mCard)
    {
        isInDeck = false;
        thisCard = mCard;
        CreateCard();

    }

    public void AssignNewCard(Card newCard)
    {
        thisCard = newCard;
        CreateCard();
    }
    public void OnDrag(PointerEventData eventData)
    {     
        if(!isInDeck && cloneToPlace != null)
        {
            cloneToPlace.FollowMouse(eventData);
        }
        else if (isInDeck)
        {
            FollowMouse(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
        if (!isInDeck)
        {
            cloneToPlace = Instantiate(this, transform.position, Quaternion.identity, GameObject.Find("Deck Building").transform);
            cloneToPlace.Init(thisCard);
           
        }
        else
        {

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInDeck)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Exists(e => e.gameObject.GetComponent<InventorySlot>()))
            {
                foreach (var thing in results)
                {
                    var slot = thing.gameObject.transform.GetComponent<InventorySlot>();
                    if (slot != null && !slot.locked)
                    {
                        cloneToPlace.transform.position = slot.transform.position + (Vector3.forward * -.1f);
                        cloneToPlace.AddToDeck(slot);                       
                    }
                    else if(slot != null && slot.locked)
                    {
                        Destroy(cloneToPlace.gameObject);
                    }
                }
            }
        }
        else
        {
            RemoveFromDeck();

            Destroy(gameObject);
        }

        cloneToPlace = null;
    }

    public virtual void ToggleVisible(bool isVisible)
    {
        transform.Find("CardBack").gameObject.SetActive(!isVisible);
    }

    public virtual bool IsVisible()
    {
        return !transform.Find("CardBack").gameObject.activeInHierarchy;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)     
            onTouch.Invoke();
    }

    public void AddToDeck(InventorySlot slot)
    {
        m_Slot = slot;
        m_Slot.locked = true;
        isInDeck = true;
        if(ProfileData.currentProfile.deckCardNames.Count < 30)
             ProfileData.currentProfile.deckCardNames.Add(thisCard.cardName);
    }

    public void RemoveFromDeck()
    {
        m_Slot.locked = false;
        m_Slot = null;
        if (ProfileData.currentProfile.deckCardNames.Contains(thisCard.cardName))
            ProfileData.currentProfile.deckCardNames.Remove(thisCard.cardName);
    }

    public void FollowMouse(PointerEventData eventData)
    {
        RectTransform m_DraggingPlane = GameObject.Find("Deck Building").transform as RectTransform;

        var rt = gameObject.GetComponent<RectTransform>();
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
        }
    }

}
