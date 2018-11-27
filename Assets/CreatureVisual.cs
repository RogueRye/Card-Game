using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CreatureVisual : MonoBehaviour {

    public TMP_Text atkTxt;
    public TMP_Text hlthTxt;
    CreatureCard ownerCard;
    [HideInInspector]
    public Animator anim; 

    public void Init(CreatureCard m_Card)
    {
        anim = GetComponent<Animator>();
        ownerCard = m_Card;
        atkTxt.text = ownerCard.attack.text;
        hlthTxt.text = ownerCard.health.text;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hlthTxt.text = ownerCard.health.text;
        hlthTxt.color = ownerCard.health.color;
    }
}
