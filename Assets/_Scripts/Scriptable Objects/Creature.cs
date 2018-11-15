﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Creature")]
public class Creature : Card
{

    public int attackCost;
    public int attackValue;
    public int healthValue;

    private int currentHealth;

    public AttackDir[] attackDirs = new AttackDir[1] { AttackDir.Forward };

    public enum AttackDir
    {
        Forward, Left, Right
    }

    public void InitHealth()
    {
        currentHealth = healthValue;
    }

    public int TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(cardName + " has a current health of: " +  currentHealth);
        return currentHealth;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

}
