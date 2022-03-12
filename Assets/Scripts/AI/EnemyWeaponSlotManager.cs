using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        EnemyStats enemyStats;
        EnemyEffectsManager enemyEffectsManager;

        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            LoadWeaponHolderSlot();
        }

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }

        private void LoadWeaponHolderSlot()
        {
            WeaponSlotHolder[] weaponHolderSlots = GetComponentsInChildren<WeaponSlotHolder>();
            foreach (WeaponSlotHolder weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
                enemyEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
                leftHandDamageCollider.physicalDamage = leftHandWeapon.physicalDamage;
                leftHandDamageCollider.fireDamage = leftHandWeapon.fireDamage;
                leftHandDamageCollider.teamIDNumber = enemyStats.teamIDNumber;
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
                enemyEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
                rightHandDamageCollider.physicalDamage = rightHandWeapon.physicalDamage;
                rightHandDamageCollider.fireDamage = rightHandWeapon.fireDamage;
                rightHandDamageCollider.teamIDNumber = enemyStats.teamIDNumber;
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void DrainStaminaLightAttack()
        {

        }

        public void DrainStaminaHeavyAttack()
        {

        }

        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //anim.SetBool("canDoCombo", false);
        }

        #region Handle Weapon's Poise Bonus
        public void GrantWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence + enemyStats.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefence = enemyStats.armorPoiseBonus;
        }
        #endregion
    }
}
