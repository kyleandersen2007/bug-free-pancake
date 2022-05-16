using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class CharacterManager : MonoBehaviour
    {
        public AnimatorManager characterAnimatorManager;
        public CharacterWeaponSlotManager characterWeaponSlotManager;
        public CharacterInventoryManager characterInventoryManager;
        public CharacterStatsManager characterStatsManager;
        public CharacterEffectsManager characterEffectsManager;
        public Transform lockOnTransform;
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;
        public Animator anim;
        public int pendingCriticalDamage;

        [Header("Flags")]
        public bool isInteracting;
        public bool canBeRiposted;
        public bool isParrying;
        public bool canBeParried;
        public bool isBlocking;
        public bool isTwoHandingWeapon;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;
        public bool isHoldingArrow;
        public bool isAiming;
        public bool isDead;
        public bool isPerformingFullyChargedAttack;

        public bool isRotatingWithRootMotion;
        public bool canRotate;

        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<AnimatorManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
        }

        protected virtual void FixedUpdate()
        {
            characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
        {
            if(usingRightHand)
            {
                isUsingLeftHand = false;
                isUsingRightHand = true;
            }
            else
            {
                isUsingLeftHand = true;
                isUsingRightHand = false;
            }
        }
    }
}
