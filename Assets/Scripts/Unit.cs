using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        public string unitName;
        public int maxHP;
        public int currentHP;
        public string Death_Anim = "Death";
        public string Hurt_Anim = "Hurt";
        public string Attack_Anim = "Attack";
        public Animator animator;
        public bool tookDamage;

        void Start()
        {
            animator = GetComponent<Animator>();
        }
        public void TakeDamage(int dmg)
        {
            animator.SetTrigger(Hurt_Anim);
            if(currentHP - dmg <= 0)
            {
                currentHP = 0;
            } else
            {
                currentHP = currentHP - dmg;
            }
            tookDamage = true;
        }

        public void PlayAttackAnim()
        {
            animator.SetTrigger(Attack_Anim);
        }

        public void PlayDeathAnim()
        {
            animator.SetTrigger(Death_Anim);
        }
    }
}
