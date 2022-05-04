using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class WeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager playerManager;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    playerManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerManager.animatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (playerManager.inputHandler.twoHandFlag)
                    {   
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerManager.animatorHandler.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    playerManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerManager.anim.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    playerManager.playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    playerManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerManager.animatorHandler.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    playerManager.playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    playerManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    playerManager.anim.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        public void SucessfullyThrowFireBomb()
        {
            Destroy(playerManager.playerEffectsManager.instantiatedFXModel);
            BombConsumableItem fireBombItem = playerManager.playerInventoryManager.currentConsumbale as BombConsumableItem;

            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, playerManager.cameraHandler.cameraPivotTransform.rotation);
            activeModelBomb.transform.rotation = Quaternion.Euler(playerManager.cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            damageCollider.explosiveDamage = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRB.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            damageCollider.bombRB.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumber = playerManager.playerStats.teamIDNumber;
            LoadWeaponOnSlot(playerManager.playerInventoryManager.rightWeapon, false);

        }
    }
}
