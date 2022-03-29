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
        CameraHandler cameraHandler;

        [Header("Attack Animations")]
        public string oh_light_attack_1;
        public string oh_light_attack_2;
        public string oh_heavy_attack_1;
        public string oh_heavy_attack_2;
        public string th_light_attack_1;
        public string th_light_attack_2;
        public string th_heavy_attack_1;
        public string th_heavy_attack_2;
        public string draw_Arrow;

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
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void HandleLightWeaponCombo(WeaponItem weapon)
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
            }
        }

        public void HandleHeavyWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

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
                lastAttack = th_light_attack_1;
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

        public void HandleHoldRBAction()
        {
            if(playerManager.isTwoHandingWeapon)
            {
                PerformRBRangedAction();
            }
            else
            {
                
            }
        }

        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.weaponType == WeaponType.StraightSword || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventory.rightWeapon.weaponType == WeaponType.Bow)
            {
                FireArrowAction();
            }
            else if(playerInventory.rightWeapon.weaponType == WeaponType.SpellCaster || playerInventory.rightWeapon.weaponType == WeaponType.FaithCaster || playerInventory.rightWeapon.weaponType == WeaponType.PyroCaster)
            {
                
            }
        }

        public void HandleRTAction()
        {
            if (playerInventory.rightWeapon.weaponType == WeaponType.StraightSword || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRTMeleeAction();
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
            if(playerManager.isTwoHandingWeapon)
            {
                if(playerInventory.rightWeapon.weaponType == WeaponType.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if(playerInventory.leftWeapon.weaponType == WeaponType.Shield)
                {
                    PerformLBBlockingAction(playerInventory.leftWeapon);
                }
                else if(playerInventory.leftWeapon.weaponType == WeaponType.FaithCaster || playerInventory.leftWeapon.weaponType == WeaponType.PyroCaster)
                {
                    PerformMagicAction(playerInventory.leftWeapon, true);
                    animatorHandler.anim.SetBool("isUsingLeftHand", true);
                }
            }
        }

        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleLightWeaponCombo(playerInventory.rightWeapon);
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

        private void DrawArrowAction()
        {
            animatorHandler.anim.SetBool("isHoldingArrow", true);
            animatorHandler.PlayTargetAnimation("Bow_TH_Draw_01", true);
            GameObject loadedArrow = Instantiate(playerInventory.currentAmmo.loadedItemModel, weaponSlotManager.leftHandSlot.transform);
            Animator bowAnimator = weaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw");
            playerEffectsManager.currentRangeEffects = loadedArrow;
        }

        public void FireArrowAction()
        {
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = weaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = weaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_TH_Fire");
            animatorHandler.PlayTargetAnimation("Bow_TH_Fire_01", true);           
            animatorHandler.anim.SetBool("isHoldingArrow", false);
            Destroy(playerEffectsManager.currentRangeEffects);

            GameObject liveArrow = Instantiate(playerInventory.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if(playerManager.isAiming)
            {
                Ray ray = cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;

                if(Physics.Raycast(ray, out hitPoint, 100))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                    Debug.Log(hitPoint.transform.name);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraTransform.localEulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                if (cameraHandler.currentLockOnTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
                }
            }
            rigidbody.AddForce(liveArrow.transform.forward * playerInventory.currentAmmo.forwardVelocity);
            rigidbody.AddForce(liveArrow.transform.up * playerInventory.currentAmmo.upwardVelocity);
            rigidbody.useGravity = playerInventory.currentAmmo.useGravity;
            rigidbody.mass = playerInventory.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = playerManager;
            damageCollider.ammoItem = playerInventory.currentAmmo;
            damageCollider.physicalDamage = playerInventory.currentAmmo.physicalDamage;
        }

        private void PerformRBRangedAction()
        {
            if (playerStats.currentStamina <= 0)
                return;

            animatorHandler.EraseHandIKForWeapon();
            animatorHandler.anim.SetBool("isUsingRightHand", true);

            if(!playerManager.isHoldingArrow)
            {
                if(playerInventory.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    
                }
            }
        }

        private void PerformRTMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleHeavyAttack(playerInventory.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
        {
            if (playerManager.isInteracting)
                return;
            if(weapon.weaponType == WeaponType.FaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if(playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
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
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
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

        private void PerformLBBlockingAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.isBlocking)
                return;
            if(weapon.weaponType == WeaponType.Shield)
            {
                animatorHandler.PlayTargetAnimation("Block Start", false, true);
                playerEquipmentManager.OpenBlockingCollider();
                playerManager.isBlocking = true;
            }
        }

        private void PerformLBAimingAction()
        {
            if (playerManager.isAiming)
                return;

            inputHandler.uiManager.crosshair.SetActive(true);
            playerManager.isAiming = true;
        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager, playerManager.isUsingLeftHand);
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
