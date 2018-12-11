using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : MonoBehaviour
{

    #region Public Fields
    public String playerName = "Ryan";
    public bool isPlayerA = true;
    public Player opponent;
    public int lifePoints;
    public int maxAP = 10;
    public Deck deck;
    public int startingHandSize = 5;
    public Transform handObj;
    public Slot playerSlot;
    public CardHolder[] cardTypes;
    public Transform optionsMenu;
    public Transform combatOptionsMenu;
    public Transform optionsOpponentCard;
    public RectTransform deckSpot;
    public RectTransform gySpot;
    public GameObject TextPanel;
    public TurnPhase currentPhase;
    public bool hasAI = false;
    #endregion

    #region Hidden Public
    [HideInInspector]
    public List<CardHolder> hand = new List<CardHolder>();
    [HideInInspector]
    public CardHolder selectedCard;
    [HideInInspector]
    public Slot[,] field;
    [HideInInspector]
    public Slot selectedSlot;
    [HideInInspector]
    public List<CreatureCard> creaturesOnField = new List<CreatureCard>();
    [HideInInspector]
    public GraphicRaycaster graphicRaycaster;
    #endregion

    #region Private References
    Stack<CardHolder> deckStack = new Stack<CardHolder>();
    Stack<CardHolder> gyStack = new Stack<CardHolder>();
    HorizontalLayoutGroup layout;

    #endregion

    #region Private Values

    int curLifePoints;
    int currentAP;
    int currentMaxAp;
    #endregion

    #region Inputs
    bool input1;
    bool input2;

    public int CurrentMaxAp
    {
        get
        {
            return currentMaxAp;
        }

        private set
        {
            currentMaxAp = value;
        }
    }
    #endregion


    virtual protected void Start()
    {
        graphicRaycaster = GetComponentInChildren<GraphicRaycaster>();
        layout = handObj.GetComponent<HorizontalLayoutGroup>();
        deck.Init();
        curLifePoints = lifePoints;
        CurrentMaxAp = (isPlayerA) ? 0 : 1;
        field = (isPlayerA) ? Board.fieldA : Board.fieldB;

        playerSlot.Init(-1, -1);
        foreach (var slot in field)
        {
            slot.Unlock();
            slot.owner = this;
        }

        deck.Shuffle();

        StackDeck();

        DrawCard(startingHandSize);

        if (isPlayerA)
        {
            StartTurn();
        }
        else
        {
            currentPhase = TurnPhase.End;
        }
    }


    #region Turn State Calls
    public void StartTurn()
    {
        if (currentPhase != TurnPhase.Start)
        {
            StartCoroutine(TurnStart());
        }
    }

    public void CastCard()
    {
        if (currentPhase != TurnPhase.Casting)
        {
            if (selectedCard.thisCard.castCost <= GetAP())
            {
                StartCoroutine(CastingCard());
            }
            else
            {

                DelselectCard();
                currentPhase = TurnPhase.Main;
            }
        }

    }

    public void StartAttackPhase()
    {
        DelselectCard();
        if (currentPhase == TurnPhase.Combat || currentPhase == TurnPhase.Attacking)
        {
            StopAttackPhase();
            return;
        }

        currentPhase = TurnPhase.Combat;

        if (!hasAI)
            CamBehaviour.singleton.SwitchToPosition(2);
    }

    public void Attack()
    {
        if (selectedCard != null)
        {
            var temp = selectedCard as CreatureCard;
            
            if (temp.canAttack)
                StartCoroutine(WaitForAttackTarget(temp));
            else
                DelselectCard();
        }
    }

    public void StopAttackPhase()
    {
        selectedSlot = null;
        DelselectCard();
        currentPhase = TurnPhase.Main;
        CamBehaviour.singleton.SwitchToPosition(0);
    }

    public void EndTurn()
    {
        StartCoroutine(TurnEnding());      
    }

    #endregion

    #region Turn State Coroutines

    private IEnumerator TurnStart()
    {
        currentPhase = TurnPhase.Start;
        DrawCard(1);

        foreach (var card in creaturesOnField)
        {
            if(card.currentSlot.id_X == 1)
                card.canAttack = true;
        }
        
        if (CurrentMaxAp < maxAP)
        {
            CurrentMaxAp++;
        }
        currentAP = CurrentMaxAp;

        yield return new WaitForSeconds(.1f);
        StartCoroutine(PickingCard());
    }

    private IEnumerator PickingCard()
    {
        currentPhase = TurnPhase.Main;
        while (selectedCard == null)
        {

            yield return null;
        }
    }

    private IEnumerator CastingCard()
    {
        currentPhase = TurnPhase.Casting;
        if (optionsMenu != null)
            optionsMenu.gameObject.SetActive(false);

        if (selectedCard is CreatureCard)
        {
            while (selectedSlot == null)
            {
                if (selectedCard == null)
                {
                    break;
                }

                yield return null;
            }

            if (selectedCard != null && selectedSlot.currentCard == null)
            {
                selectedCard.Cast(selectedSlot);
                creaturesOnField.Add((CreatureCard)selectedCard);
                if (hand.Contains(selectedCard))
                    hand.Remove(selectedCard);
                DelselectCard();
                selectedSlot = null;
            }

            if (hand.Count <= 6)
                layout.spacing = .7f;
        }
        else if (selectedCard is SpellCard)
        {
            //Do different Things
        }
        yield return new WaitForSeconds(.1f);
        currentPhase = TurnPhase.Main;
    }

    private IEnumerator WaitForAttackTarget(CreatureCard attackingCreature)
    {
        currentPhase = TurnPhase.Attacking;
        if(combatOptionsMenu != null)
            combatOptionsMenu.gameObject.SetActive(false);
        playerSlot.Lock();
        opponent.playerSlot.Lock();
        int creaturesInBackRow = 0;       
        foreach (var slot in field)
            slot.Lock();
        foreach (var slot in opponent.field)
        {
            if (slot.id_X == 0 && slot.currentCard != null)
                creaturesInBackRow++;
            slot.Lock();
        }        

        foreach (var dir in attackingCreature.thisCardC.attackDirs)
        {
            switch (dir)
            {
                case Creature.AttackDir.Forward:
                    
                    for (int i = 0; i < 2; i++)
                    {
                        var tempSlot = opponent.field[i, attackingCreature.currentSlot.id_Y];
                        if (tempSlot != null)
                        {
                            if (tempSlot.currentCard != null)
                            {                               
                                tempSlot.Unlock();                               
                            }
                        }
                    }
                    break;
                case Creature.AttackDir.Left:
                    if (attackingCreature.currentSlot.id_Y - 1 >= 0)
                    {
                        for (int i = 0; i < 2; i++)


                        {
                            var tempSlot = opponent.field[i, attackingCreature.currentSlot.id_Y - 1];
                            if (tempSlot != null)
                            {
                                if (tempSlot.currentCard != null)
                                {
                                    tempSlot.Unlock();                                   
                                }
                            }
                        }
                    }
                    break;
                case Creature.AttackDir.Right:                   
                    if (attackingCreature.currentSlot.id_Y + 1 < 5)
                    {
                        for (int i = 0; i < 2; i++)
                        {

                            var tempSlot = opponent.field[i, attackingCreature.currentSlot.id_Y + 1];
                            if (tempSlot != null)
                            {
                                if (tempSlot.currentCard != null)
                                {
                                    tempSlot.Unlock();                                   
                                }
                            }
                        }
                    }
                    break;
            }
        }

        if (creaturesInBackRow <= 0)
        {
            opponent.playerSlot.Unlock();
        }
       
        while (selectedSlot == null)
        {
            if ((selectedCard as CreatureCard) != attackingCreature || currentPhase == TurnPhase.Main)
                break;
            yield return null;
        }
      
        if (currentPhase != TurnPhase.Main)
        {
            if (selectedCard == attackingCreature)
            {                
                if (selectedSlot.currentCard != null)
                    attackingCreature.Attack(selectedSlot.currentCard);
                else if (selectedSlot == opponent.playerSlot)
                    attackingCreature.Attack(opponent);
                
            }

            yield return new WaitForSeconds(.1f);
            currentPhase = TurnPhase.Combat;
        }

        DelselectCard();
        selectedSlot = null;
        foreach (var slot in field)
            slot.Unlock();
        foreach (var slot in opponent.field)
            slot.Unlock();
       
    }

    private IEnumerator TurnEnding()
    {
        if (currentPhase != TurnPhase.End)
        {
            if (!hasAI)
                CamBehaviour.singleton.SwitchToPosition(0);

            currentPhase = TurnPhase.End;
            yield return new WaitForSeconds(.3f);
            opponent.StartTurn();
        }
    }

    #endregion

    virtual protected void Update()
    {

        if (currentPhase == TurnPhase.End)
            return;


        if (!hasAI)
        {
            GetInput();
            if (input2)
            {
                DelselectCard();
            }
        }
    }

    virtual protected void GetInput()
    {
#if UNITY_STANDALONE

        input1 = Input.GetMouseButtonDown(0);
        input2 = Input.GetMouseButtonDown(1);



#elif UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR

        if (Input.touchCount > 1)
        {
            // input1 = Input.GetTouch(0).phase == TouchPhase.Began;
            input2 = Input.GetTouch(1).phase == TouchPhase.Began;
        }
#endif
    }


    #region Gameplay Functions
    /// <summary>
    /// Add card from deck to hand
    /// </summary>
    /// <param name="numOfCards">Number of cards to draw</param>
    public void DrawCard(int numOfCards)
    {
        for (int i = 0; i < numOfCards; i++)
        {
            if (deckStack.Count == 0)
            {
                Debug.Log("out of cards");
                TakeDamage(GetLifePoints());
                return;
            }

            var newCard = deckStack.Pop();

            //Only reveal card if its the players and not the AI
            if (isPlayerA)
                newCard.ToggleVisible(true);

            if (hand.Count > 7)
            {
                DiscardCard(newCard);

            }
            else
            {
                hand.Add(newCard);
                newCard.transform.SetParent(handObj);
                newCard.transform.localRotation = Quaternion.Euler(Vector3.up * 180);
                newCard.transform.localPosition = Vector3.zero - (Vector3.forward * .01f * hand.Count);               

                if (hand.Count != 0 && hand.Count > 6)
                    layout.spacing -= (.7f);
                else if (hand.Count <= 6)
                    layout.spacing = .7f;
            }
        }
    }

    /// <summary>
    /// Send Card to the graveyard and add it to the GY stack/list
    /// </summary>
    /// <param name="card">card to discard</param>
    public void DiscardCard(CardHolder card)
    {
        //Unselect it, add to to stack, reset transform to the graveyard
        DelselectCard();
        gyStack.Push(card);
        card.transform.SetParent(gySpot);
        card.transform.position = gySpot.position + (Vector3.up * (gyStack.Count * 0.075f));
        
        card.transform.rotation = gySpot.rotation;

        //Remove card from hand
        if (hand.Contains(card))
            hand.Remove(card);
        //Remove card from field and unassign its slot
        if (creaturesOnField.Contains((CreatureCard)card))       
            creaturesOnField.Remove(card as CreatureCard);       
    }

    /// <summary>
    /// Default override to discard the currently selected card. Used for UI buttons/Testing
    /// </summary>
    public void DiscardCard()
    {
        if (selectedCard != null)
        {
            DiscardCard(selectedCard);
        }
    }

    /// <summary>
    /// Pick the card, show options menu, indicate selection
    /// </summary>
    /// <param name="card">card to select</param>
    public void SelectCard(CardHolder card)
    {
        DelselectCard();
        selectedCard = card;
        selectedCard.selectionFX.Play();
        if (hand.Contains(card) && currentPhase == TurnPhase.Main)
        {              
            if (optionsMenu != null)
            {
                optionsMenu.gameObject.SetActive(true);
                optionsMenu.gameObject.transform.position = Input.mousePosition;
            }          
        }
        else if (currentPhase == TurnPhase.Combat && card is CreatureCard)
        {             
            if (combatOptionsMenu != null && (card as CreatureCard).canAttack)
            {
                combatOptionsMenu.gameObject.SetActive(true);
                combatOptionsMenu.gameObject.transform.position = Input.mousePosition;
            }
        }
    }


    /// <summary>
    /// move a selected card back and hide options menu when the card is no longer selected
    /// </summary>
    public void DelselectCard()
    {
        if (selectedCard == null)
            return;

        selectedCard.selectionFX.Stop();
        selectedCard = null;
        if (optionsMenu != null)
        {
            optionsMenu.gameObject.SetActive(false);
            combatOptionsMenu.gameObject.SetActive(false);
            optionsOpponentCard.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Card text can be hard to read, this will show it in an easier to read panel
    /// </summary>
    public void ShowText()
    {
        if (selectedCard == null)
            return;

        optionsMenu.gameObject.SetActive(false);
        combatOptionsMenu.gameObject.SetActive(false);
        optionsOpponentCard.gameObject.SetActive(false);

        if (TextPanel != null)
        {
            TextPanel.SetActive(true);
            var displayCard = TextPanel.transform.GetComponentInChildren<CardHolder>();
            displayCard.Init();
            displayCard.AssignNewCard(selectedCard.thisCard);
        }       
    }

    public void TakeDamage(int power)
    {
        curLifePoints -= power;
    }

    public int GetLifePoints()
    {
        return curLifePoints;
    }

    public void SpendAP(int amount)
    {
        currentAP -= amount;
    }

    public int GetAP()
    {
        return currentAP;
    }

    private void StackDeck()
    {
        for (int i = 0; i < deck.deckSize; i++)
        {

            var cardToPlace = deck.mDeck.Pop();

            var pos = deckSpot.position + (Vector3.up * (i * 0.075f));
            var deckCard = Instantiate(cardTypes[(int)cardToPlace.type], pos, deckSpot.rotation, deckSpot);

            deckCard.Init(cardToPlace, this, opponent);
            deckCard.ToggleVisible(false);
            deckStack.Push(deckCard);
        }
    }

    #endregion

}

public enum TurnPhase
{
    Start,
    Main,
    Casting,
    Combat,
    Attacking,
    End
}
