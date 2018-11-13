using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Color highlightColor = new Color(0, 255, 255, 255);
    public Color opponentColor = new Color(255, 0, 0, 255);
    public CreatureCard currentCard;
    public Player owner;
    /// <summary>
    /// Row Number
    /// </summary>
    public int id_X;
    /// <summary>
    /// Column Number
    /// </summary>
    public int id_Y; 
    /// <summary>
    /// Is there a creature in the slot?
    /// </summary>
    bool isBlocked = false;
    /// <summary>
    /// is the slot locked due to game state/owner?
    /// </summary>
    bool isLocked = true;


    /// <summary>
    /// is the slot locked due to game state/owner?
    /// </summary>
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

    /// <summary>
    /// Is there a creature in the slot?
    /// </summary>
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
    public void Init(int x, int y)
    {
        id_X = x;
        id_Y = y;
        Lock();
        Unblock();
        graphics = GetComponent<Image>();
    }

    public void Touch()
    {
        
    }

    public void OnTouchUp()
    {
        if (!isBlocked && !isLocked && owner.currentPhase == TurnPhase.Casting)
        {            
            owner.selectedSlot = this;
        }
        else if(!isLocked && owner.opponent.currentPhase == TurnPhase.Attacking)
        {
            owner.opponent.selectedSlot = this;
        }
    }

    public void Hover()
    {
        if(!isBlocked && !isLocked && owner.currentPhase == TurnPhase.Casting)
            graphics.color = highlightColor;
        else if (!isLocked && owner.opponent.currentPhase == TurnPhase.Attacking)
        {
            graphics.color = opponentColor;
        }
    }

    public void StopHover()
    {
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
    public void Lock()
    {
        IsLocked = true;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }

    public void Block()
    {
        IsBlocked = true;
    }
}
