using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        PlayerAnimatorManager animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attack Animations")]
        public string oh_light_attack_1;
        public string oh_light_attack_2;
        public string oh_heavy_attack_1;
        public string oh_heavy_attack_2;
        public string th_light_attack_1;
        public string th_light_attack_2;
        public string th_heavy_attack_1;
        public string th_heavy_attack_2;

        public string weaponArt;

        public string lastAttack;

        public LayerMask backStabLayer;
        public LayerMask riposteLayer;

        private void Awake()
        {
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            playerInventory = GetComponent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == oh_light_attack_1)
                {
                    animatorHandler.PlayTargetAnimation(oh_light_attack_2, true);
                }
                else if (lastAttack == th_light_attack_1)
                {
                    animatorHandler.PlayTargetAnimation(th_light_attack_2, true);
                }

                if (lastAttack == oh_heavy_attack_1)
                {
                    animatorHandler.PlayTargetAnimation(oh_heavy_attack_2, true);
                }
                else if (lastAttack == th_heavy_attack_1)
                {
                    animatorHandler.PlayTargetAnimation(th_heavy_attack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(th_light_attack_1, true);
                lastAttack = th_light_attack_2;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(oh_light_attack_1, true);
                lastAttack = oh_light_attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(th_heavy_attack_1, true);
                lastAttack = th_heavy_attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(oh_heavy_attack_1, true);
                lastAttack = oh_heavy_attack_1;
            }
        }

        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.weaponType == WeaponType.StraightSword || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventory.rightWeapon.weaponType == WeaponType.SpellCaster || playerInventory.rightWeapon.weaponType == WeaponType.FaithCaster || playerInventory.rightWeapon.weaponType == WeaponType.PyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }

        public void HandleLTAction()
        {
            if(playerInventory.leftWeapon.weaponType == WeaponType.Shield || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if(playerInventory.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                //Do Left Melee
            }
        }

        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }

        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;
            if(weapon.weaponType == WeaponType.FaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if(playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Cant Spell", true);
                    }
                }
            }
            else if (weapon.weaponType == WeaponType.PyroCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Cant Spell", true);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

            if(isTwoHanding)
            {

            }
            else
            {
                animatorHandler.PlayTargetAnimation(weaponArt, true);
            }

        }

        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.isBlocking)
                return;
            animatorHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager);
        }

        public void AttemptBackStabOrReposte()
        {
            if (playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;

            if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Backstab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Backstabbed", true);
                }
            }
            else if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;
                
                if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }
}
