 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KA
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Weapon Animator")]
        public AnimatorOverrideController weaponController;
        public string offHandIdleAnimation;

        [Header("Damage")]
        public int heavyWeaponAttackMultiplier;
        public int physicalDamage = 30;
        public int fireDamage;
        public int criticalDamageMultiplier = 4;

        [Header("Weapon Type")]
        public WeaponType weaponType;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Stamina Drain Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackStaminaMultiplier;

        [Header("Absorption")]
        public float physicalDamageAbsorption;
    }
}