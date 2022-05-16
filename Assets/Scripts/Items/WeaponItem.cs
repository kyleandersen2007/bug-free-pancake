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

        [Header("Weapon Actions")]
        public ItemAction tap_RB_Action;
        public ItemAction hold_RB_Action;
        public ItemAction tap_RT_Action;
        public ItemAction hold_RT_Input;
        public ItemAction tap_LT_Action;
        public ItemAction hold_LT_Action;
        public ItemAction tap_LB_Action;
        public ItemAction hold_LB_Action;

        public ItemAction th_tap_RB_Action;
        public ItemAction th_hold_RB_Action;
        public ItemAction th_tap_RT_Action;
        public ItemAction th_hold_RT_Input;
        public ItemAction th_tap_LT_Action;
        public ItemAction th_hold_LT_Action;
        public ItemAction th_tap_LB_Action;
        public ItemAction th_hold_LB_Action;
    }
}