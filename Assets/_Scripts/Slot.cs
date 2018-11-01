using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Color highlightColor = new Color(0, 255, 255, 255);
    public CreatureCard currentCard;
    public Player owner;


    bool isBlocked = false;
    bool isLocked = true;



    public bool IsLocked
    {
        get
        {
            return isLocked;
        }

        private set
        {
            isLocked = value;
        }
    }

    public bool IsBlocked
    {
        get
        {
            return isBlocked;
        }

        private set
        {
            isBlocked = value;
        }
    }

    Image graphics;
    // Use this for initialization
    void Start()
    {
        Unblock();
        graphics = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Touch()
    {
        
    }

    public void OnTouchUp()
    {
        if (!isLocked && owner.currentPhase == TurnPhase.Casting)
        {
            Debug.Log("HI");
            owner.selectedSlot = this;
        }
    }

    public void Hover()
    {
        if(!isLocked && owner.currentPhase == TurnPhase.Casting)
            graphics.color = highlightColor;
    }

    public void StopHover()
    {
        if(!isLocked)
            graphics.color = Color.white;
    }

    public void LoseCard()
    {
        
        owner.DiscardCard(currentCard);
        currentCard.inSlot = false;
        currentCard = null;
        Unblock();
    }

    public void Unlock()
    {
        IsLocked = false;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }

    public void Block()
    {
        isBlocked = true;
    }
}
