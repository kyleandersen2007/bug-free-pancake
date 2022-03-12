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

        private PlayerAnimatorManager playerAnimatorManager;
        CameraHandler cameraHandler;

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

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

            cameraHandler = FindObjectOfType<CameraHandler>();

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
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        if(playerInventory.leftWeapon.weaponType == WeaponType.Shield)
                        {
                            backShieldSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                        }
                        else
                        {
                            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                        }  
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                        backShieldSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerAnimatorManager.anim.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                if(isLeft)
                {
                    playerInventory.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerInventory.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerAnimatorManager.anim.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        public void SucessfullyThrowFireBomb()
        {
            Destroy(playerEffectsManager.instantiatedFXModel);
            BombConsumableItem fireBombItem = playerInventory.currentConsumbale as BombConsumableItem;

            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider bombDamageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            bombDamageCollider.explosiveDamage = fireBombItem.baseDamage;
            bombDamageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            bombDamageCollider.bombRB.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            bombDamageCollider.bombRB.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            bombDamageCollider.teamIDNumber = playerStats.teamIDNumber;
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        }

        #region Handle Weapon Damage Colliders

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.physicalDamage = playerInventory.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = playerInventory.leftWeapon.fireDamage;
            leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
            playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            leftHandDamageCollider.teamIDNumber = playerStats.teamIDNumber;
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.physicalDamage = playerInventory.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = playerInventory.rightWeapon.fireDamage;
            rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
            playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            rightHandDamageCollider.teamIDNumber = playerStats.teamIDNumber;
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
