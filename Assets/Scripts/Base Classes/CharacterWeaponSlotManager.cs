using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponSlotHolder leftHandSlot;
        public WeaponSlotHolder rightHandSlot;
        public WeaponSlotHolder backSlot;
        public WeaponSlotHolder shieldSlot;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Hand IK Targets")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();
        }

        protected virtual void LoadWeaponHolderSlots()
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
                    shieldSlot = weaponSlot;
                }
            }
        }

        public virtual void LoadBothWeaponsOnSlots()
        {
            LoadWeaponOnSlot(characterManager.characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(characterManager.characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    characterManager.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (characterManager.isTwoHandingWeapon)
                    {
                        if (leftHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                        {
                            shieldSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            characterManager.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                        }

                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        characterManager.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(characterManager.isTwoHandingWeapon);
                    characterManager.anim.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    characterManager.characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    characterManager.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterManager.characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    characterManager.anim.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            leftHandDamageCollider.physicalDamage = characterManager.characterInventoryManager.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = characterManager.characterInventoryManager.leftWeapon.fireDamage;

            leftHandDamageCollider.characterManager = characterManager;
            leftHandDamageCollider.teamIDNumber = characterManager.characterStatsManager.teamIDNumber;

            leftHandDamageCollider.poiseBreak = characterManager.characterInventoryManager.leftWeapon.poiseBreak;
            characterManager.characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        
        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            rightHandDamageCollider.physicalDamage = characterManager.characterInventoryManager.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = characterManager.characterInventoryManager.rightWeapon.fireDamage;

            rightHandDamageCollider.characterManager = characterManager;
            rightHandDamageCollider.teamIDNumber = characterManager.characterStatsManager.teamIDNumber;

            rightHandDamageCollider.poiseBreak = characterManager.characterInventoryManager.rightWeapon.poiseBreak;
            characterManager.characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
            characterManager.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (characterManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }

            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = characterManager.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.armorPoiseBonus;
        }
    }
}
