using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class HandleUIBehaviour : MonoBehaviour
{

    public Slider lifePointSldr;
    public Image[] phaseBtns;
    public TMP_Text lifePointTxt;
    public TMP_Text playerNameDisplay;
    [Range(0, 20)]
    public float sliderSpeed = 1;
    public Slider manaSldr;
    public TMP_Text manaTxt;

    private Player m_Player;

    public UnityEvent onChangedPhase;
    TurnPhase prevPhase;
    public Color selectedButtonColor;
    Color defaultColor;
    // Use this for initialization
    void Start()
    {

        m_Player = GetComponent<Player>();
        prevPhase = m_Player.currentPhase;
        lifePointSldr.maxValue = m_Player.lifePoints;
        lifePointSldr.value = lifePointSldr.maxValue;
        if(phaseBtns.Length > 0)
            defaultColor = phaseBtns[0].color;
        manaSldr.maxValue = m_Player.maxAP;

        if (playerNameDisplay != null)
            playerNameDisplay.text = m_Player.playerName;

        onChangedPhase.AddListener(ChangeSelectedButton);

    }

    // Update is called once per frame
    void Update()
    {


        if (lifePointSldr.value > m_Player.GetLifePoints())
        {
            lifePointSldr.value -= (Time.deltaTime * sliderSpeed);
        }

        if (lifePointSldr.value < m_Player.GetLifePoints())
        {
            lifePointSldr.value = m_Player.GetLifePoints();
        }

        lifePointTxt.text = Mathf.RoundToInt(lifePointSldr.value).ToString();

        if (manaSldr.value > m_Player.GetAP())
        {
            manaSldr.value--;
        }

        if (manaSldr.value < m_Player.GetAP())
        {
            manaSldr.value++;
        }

        manaTxt.text = string.Format("{0}/{1}", m_Player.GetAP().ToString(), m_Player.CurrentMaxAp);
        //if (selectedCard != null && m_Player.currentPhase != TurnPhase.NotTurnMyTurn)
        //{
        //    selectedCard.text = string.Format("Current Phase: {0}", m_Player.currentPhase.ToString());
        //   // selectedCard.text = m_Player.selectedCard == null ? "None" : m_Player.selectedCard.thisCard.cardName;
        //}

        //if(phaseBtns.Length > 0)
        //    onChangedPhase.Invoke();
        if (prevPhase != m_Player.currentPhase && phaseBtns.Length > 0)
        {
            onChangedPhase.Invoke();
        }

    }

    void ChangeSelectedButton()
    {

        //Debug.Log(m_Player.currentPhase);
        EventSystem.current.SetSelectedGameObject(phaseBtns[(int)m_Player.currentPhase].gameObject);
        for (int i = 0; i < phaseBtns.Length; i++)
        {
            if (i == (int)m_Player.currentPhase)
            {
                phaseBtns[i].color = selectedButtonColor;
            }
            else
            {
                phaseBtns[i].color = defaultColor;
            }
        }
        prevPhase = m_Player.currentPhase;

    }

}
