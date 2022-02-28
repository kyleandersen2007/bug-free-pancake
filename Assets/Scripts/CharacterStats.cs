using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Health Stats")]
        public int healthLevel;
        public int maxHealth;
        public int currentHealth;
        [Header("Stamina Stats")]
        public int staminaLevel;
        public int maxStamina;
        public float currentStamina;
        [Header("Focus Points Stats")]
        public int focusLevel;
        public float maxFocusPoints;
        public float currentFocusPoints;

        [Header("Poise")]
        public float totalPoiseDefence;
        public float offensivePoiseBonus;
        public float armorPoiseBonus;
        public float totalPoiseResetTime;
        public float poiseResetTimer;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        public int soulCount = 0;   

        public bool isDead;

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage")
        {
            if (isDead)
                return;
            float totalPhysicalDamageAbsorption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionLegs / 100) *
                (1 - physicalDamageAbsorptionHands / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            Debug.Log("Total Damage Absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage; //+ fireDamage + magicDamage + lightningDamage + darkDamage

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("Total Damage Dealt is " + finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }
    }
}
