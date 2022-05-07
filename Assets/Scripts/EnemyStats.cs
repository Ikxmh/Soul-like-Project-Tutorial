﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IH
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }
        // Start is called before the first frame update
        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth; 
        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
           currentHealth = currentHealth - damage;

            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead_01");

            }
        }
    }
}

