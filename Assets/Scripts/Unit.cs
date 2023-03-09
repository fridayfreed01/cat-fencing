using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHP;
    public int currentHP;

    public void TakeDamage(int dmg)
    {
        currentHP = currentHP - dmg;
    }
}
