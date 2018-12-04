using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CreatureCard : CardHolder, IPointerUpHandler
{

    public bool canAttack;
    public TMP_Text attack;
    public TMP_Text health;
    public Material lineMat;
    public Creature thisCardC;
    public Slot currentSlot;
    [HideInInspector]
    public CreatureVisual model;


    LineRenderer lineRenderer;
    
    private int currentHealth;
    Vector3 prevPosition;

    public override void CreateCard()
    {
        base.CreateCard();

        if (thisCard is Creature)
        {
            thisCardC = (Creature)thisCard;
            attack.text = thisCardC.attackValue.ToString();
            health.text = thisCardC.healthValue.ToString();
            lineRenderer = GetComponent<LineRenderer>();
            
            InitHealth();

        }
    }

    public override void Cast()
    {
        
    }

    public override void Cast(Slot targetSlot)
    {
        if (!targetSlot.IsBlocked && !targetSlot.IsLocked)
        {
            ToggleVisible(true);
            model = Instantiate(thisCardC.model, transform).GetComponent<CreatureVisual>();
            model.Init(this);
            currentSlot = targetSlot;
            canAttack = false;
            inSlot = true;
            transform.SetParent(targetSlot.transform);
            transform.localPosition = (Vector3.forward * .1f);
            transform.localRotation = Quaternion.Euler(Vector3.right * -180);
            targetSlot.currentCard = this;
            thisPlayer.SpendAP(thisCardC.castCost);
            thisPlayer.hand.Remove(this);
            targetSlot.Block();
        }

    }

    public void Attack(CreatureCard target)
    {
        model.anim.Play("Attack");
        target.model.anim.Play("Hit");
        target.TakeDamage(thisCardC.attackValue);
        TakeDamage(target.thisCardC.attackValue);
        target.health.text = target.GetHealth().ToString();
        health.text = GetHealth().ToString();
        target.health.color = Color.red;
        health.color = Color.red;
        canAttack = false;

        if (target.GetHealth() <= 0)
        {
            target.Die();        
        }
        if (GetHealth() <=0)
        {
            Die();
        }

    }

    public void Attack(Player target)
    {
        model.anim.Play("Attack");
        target.TakeDamage(thisCardC.attackValue);
        canAttack = false;
    }


    public override void OnDrag(PointerEventData eventData)
    {
        if (thisPlayer == null)
            return;
        if (thisPlayer.currentPhase == TurnPhase.NotTurnMyTurn)
            return;

        RectTransform m_DraggingPlane = thisPlayer.handObj as RectTransform;

        if(thisPlayer.currentPhase != TurnPhase.Main)
        {
            m_DraggingPlane = Board.instance.board;

        }

        var rt = gameObject.GetComponent<RectTransform>();
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            if(thisPlayer.currentPhase == TurnPhase.Main && thisPlayer.hand.Contains(this))
                rt.position = globalMousePos;
            else if(thisPlayer.currentPhase == TurnPhase.Attacking)
            {
                lineRenderer.SetPosition(0, rt.position + Vector3.up * .1f);

                RaycastHit hit;
                var linePos = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(linePos, out hit))
                {
                    var newPost = hit.point;
                    newPost.y = transform.position.y + .1f;
                    lineRenderer.SetPosition(1, newPost);
                }
                else
                    lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));

                if (eventData.hovered.Count != 0)
                {
                    var slot = eventData.hovered[0].GetComponent<Slot>();
                    if (slot != null)
                    {
                        if (!slot.IsLocked)
                        {
                            var linePost = slot.transform.position;
                                                       
                            linePost.y = transform.position.y + .1f;                           
                            lineRenderer.SetPosition(1, linePost);
                        }
                    }
                }               
            }
        }

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (thisPlayer == null)
            return;
        base.OnBeginDrag(eventData);

        if (thisPlayer.currentPhase == TurnPhase.NotTurnMyTurn)
            return;

        thisPlayer.combatOptionsMenu.gameObject.SetActive(false);
        prevPosition = transform.position;

        if (thisPlayer.currentPhase == TurnPhase.Combat)
        {
            thisPlayer.Attack();
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (thisPlayer == null)
            return;
        if (thisPlayer.currentPhase == TurnPhase.NotTurnMyTurn)
            return;        
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (thisPlayer.currentPhase == TurnPhase.Main )
        {
            transform.position = prevPosition;
            if (results.Exists(e => e.gameObject.GetComponent<Slot>()) && thisPlayer.hand.Contains(this))
            {
                
                thisPlayer.CastCard();
                foreach (var thing in results)
                {
                    var slot = thing.gameObject.GetComponent<Slot>();
                    if (slot != null)
                        slot.OnTouchUp();
                }
            }
            else
            {
               
                thisPlayer.DelselectCard();
            }
        }
        else if(thisPlayer.currentPhase == TurnPhase.Attacking)
        {
            lineRenderer.SetPosition(0, Vector3.up);
            lineRenderer.SetPosition(1, Vector3.up);
            if (results.Exists(e => e.gameObject.GetComponent<Slot>()))
            {
                //Debug.Log("slot exits");                
                foreach (var thing in results)
                {
                    var slot = thing.gameObject.GetComponent<Slot>();
                    if (slot != null)
                        slot.OnTouchUp();
                }
            }
            else
            {
                thisPlayer.DelselectCard();
            }
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (thisPlayer == null)
            return;
        base.OnPointerUp(eventData);

        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Exists(e => e.gameObject.GetComponent<Slot>()))
        {
            foreach (var thing in results)
            {
                var slot = thing.gameObject.GetComponent<Slot>();
                if (slot != null)
                    slot.OnTouchUp();
            }
        }
    }

    public override void Hover()
    {
        if (inSlot && !canAttack)
            return; 

        base.Hover();

    }

    public void InitHealth()
    {
        currentHealth = thisCardC.healthValue;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    bool isDying = false;
    public void Die()
    {
        if (!isDying)
            StartCoroutine(Dying());
    }

    IEnumerator Dying()
    {
        isDying = true;
        model.anim.SetBool("Dead", true);        
            
        if(thisPlayer.currentPhase == TurnPhase.NotTurnMyTurn)
            currentSlot.Lock();

       
        yield return new WaitForSeconds(2);
        health.text = thisCardC.healthValue.ToString();
        health.color = Color.white;       
        Destroy(model.gameObject);
        currentSlot.LoseCard();
        currentSlot = null;
        thisPlayer.selectedSlot = null;
        isDying = false;
        
    }
}
