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

    ParticleSystem sleepyFX;

    public void Init(CreatureCard m_Card)
    {
        anim = GetComponent<Animator>();
        ownerCard = m_Card;
        sleepyFX = GetComponentInChildren<ParticleSystem>();
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

        if ((ownerCard.canAttack && sleepyFX.isPlaying) || ownerCard.currentSlot.id_X == 0)
            sleepyFX.Stop();
        else if (!ownerCard.canAttack && !sleepyFX.isPlaying && ownerCard.currentSlot.id_X != 0)
            sleepyFX.Play();

    }
}
