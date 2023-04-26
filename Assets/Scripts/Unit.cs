using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        public string unitName;
        public int maxHP;
        public int currentHP;
        public Animation attackAnim;
        public Animation hurtAnim;
        public Animation surrenderAnim;

        public void TakeDamage(int dmg)
        {
            if(currentHP - dmg <= 0)
            {
                currentHP = 0;
            } else
            {
                currentHP = currentHP - dmg;
            }
        }
    }
}
