using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Creature")]
public class Creature : Card
{

    public int attackCost;
    public int attackValue;
    public int healthValue;
    
    public AttackDir[] attackDirs = new AttackDir[1] { AttackDir.Forward };

    public enum AttackDir
    {
        Forward, Left, Right
    }


    public override void Cast()
    {
        throw new System.NotImplementedException();
    }

    public override void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
