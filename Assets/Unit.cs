using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int maxHP;
    public int currentHP;

    public int attack;

    internal int TakeDamage(int damage)
    {
        int originalHP = currentHP;
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        return originalHP - currentHP;
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
}
