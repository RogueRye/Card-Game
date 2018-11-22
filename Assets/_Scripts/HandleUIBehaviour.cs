using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleUIBehaviour : MonoBehaviour {

    public Slider lifePointSldr;
    public TMP_Text lifePointTxt;
    [Range(0, 20)]
    public float sliderSpeed = 1;
    public Slider manaSldr;
    public TMP_Text manaTxt;

    private Player m_Player;

	// Use this for initialization
	void Start () {

        m_Player = GetComponent<Player>();

        lifePointSldr.maxValue = m_Player.lifePoints;
        lifePointSldr.value = lifePointSldr.maxValue;
        manaSldr.maxValue = m_Player.maxAP;
        manaSldr.maxValue = m_Player.GetAP();
    }

    // Update is called once per frame
    void Update() {

        if (lifePointSldr.value > m_Player.GetLifePoints())
        {
            lifePointSldr.value -= (Time.deltaTime * sliderSpeed);
        }

        if(lifePointSldr.value < m_Player.GetLifePoints())
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

        manaTxt.text = m_Player.GetAP().ToString();

    }
}
