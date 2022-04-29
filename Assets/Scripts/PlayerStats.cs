using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IH
{
    public class PlayerStats : MonoBehaviour
    {

        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth; 

        public HealthBar healthBar;

        AnimationHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimationHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth; 
            healthBar.SetMaxHealth(maxHealth);
        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10; 
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);
            animatorHandler.PlayTargetAnimations("Damage_01", true); 

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimations("Dead_01", true); 
            }
        }

      
    }
}

