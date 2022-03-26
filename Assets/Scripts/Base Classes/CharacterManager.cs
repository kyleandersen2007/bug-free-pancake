using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class CharacterManager : MonoBehaviour
    {
        AnimatorManager characterAnimatorManager;
        CharacterWeaponSlotManager characterWeaponSlotManager;
        public Transform lockOnTransform;
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        public int pendingCriticalDamage;

        [Header("Combat Flags")]
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

        public bool isRotatingWithRootMotion;
        public bool canRotate;

        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<AnimatorManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        }

        protected virtual void FixedUpdate()
        {
            characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
        }
    }
}
