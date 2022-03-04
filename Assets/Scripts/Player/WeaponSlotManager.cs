using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class WeaponSlotManager : CharacterWeaponSlotManager
    {
        private PlayerManager playerManager;

        private Animator animator;

        private QuickSlotsUI quickSlotsUI;

        private PlayerStats playerStats;
        private InputHandler inputHandler;

        private PlayerInventory playerInventory;
        private PlayerEffectsManager playerEffectsManager;


        public WeaponItem attackingWeapon;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();

            playerInventory = GetComponent<PlayerInventory>();

            animator = GetComponent<Animator>();

            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

            playerStats = GetComponent<PlayerStats>();

            inputHandler = GetComponent<InputHandler>();

            playerEffectsManager = GetComponent<PlayerEffectsManager>();

            LoadWeaponHolderSlots();
        }

        private void LoadWeaponHolderSlots()
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
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
                else if(weaponSlot.isShieldBackSlot)
                {
                    backShieldSlot = weaponSlot;
                }
            }
        }

        public void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
                }
                else
                {
                    if (inputHandler.twoHandFlag || weaponItem.isTwoHandedWeapon)
                    {
                        if(playerInventory.leftWeapon.isShieldWeapon)
                        {
                            backShieldSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            animator.CrossFade(weaponItem.TH_Idle, 0.2f);
                        }
                        else
                        {
                            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            animator.CrossFade(weaponItem.TH_Idle, 0.2f);
                        }  
                    }
                    else
                    {
                        animator.CrossFade("Both Arms Empty", 0.2f);
                        backSlot.UnloadWeaponAndDestroy();
                        backShieldSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                }
            }
            else
            {
                if(isLeft)
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                    playerInventory.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                    playerInventory.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                }
            }
        }

        #region Handle Weapon Damage Colliders

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
            playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
            playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        public void OpenDamageCollider()
        {
            if(playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if(playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if(rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }

            if(leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }
        #endregion

        #region Handle Weapon Stamina Drain
        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion

        #region Handle Weapon's Poise Bonus
        public void GrantWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefence = playerStats.totalPoiseDefence + attackingWeapon.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefence = playerStats.armorPoiseBonus;
        }
        #endregion
    }
}
