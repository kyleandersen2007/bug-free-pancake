using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace KA
{
    public class AnimatorManager : MonoBehaviour
    {
        protected CharacterManager characterManager;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        private bool handIKWeightsReset;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterManager.characterStatsManager = GetComponent<CharacterStatsManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
        {
            characterManager.anim.applyRootMotion = isInteracting;
            characterManager.anim.SetBool("canRotate", canRotate);
            characterManager.anim.SetBool("isInteracting", isInteracting);
            characterManager.anim.SetBool("isMirrored", mirrorAnim);
            characterManager.anim.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            characterManager.anim.applyRootMotion = isInteracting;
            characterManager.anim.SetBool("isRotatingWithRootMotion", true);
            characterManager.anim.SetBool("isInteracting", isInteracting);
            characterManager.anim.CrossFade(targetAnim, 0.2f);
        }

        public virtual void EnableCanRotate()
        {
            characterManager.anim.SetBool("canRotate", true);
        }

        public virtual void DisableCanRotate()
        {
            characterManager.anim.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            characterManager.anim.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            characterManager.anim.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            characterManager.anim.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            characterManager.anim.SetBool("isInvulnerable", false);
        }

        public virtual void EnableIsParrying()
        {
            characterManager.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            characterManager.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            characterManager.canBeRiposted = true;
        }

        public virtual void DisableCanBeRiposted()
        {
            characterManager.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            characterManager.characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage, 0);
            characterManager.pendingCriticalDamage = 0;
        }

        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
        {
            if (isTwoHandingWeapon)
            {
                if(rightHandTarget != null)
                {
                    rightHandConstraint.data.target = rightHandTarget.transform;
                    rightHandConstraint.data.targetPositionWeight = 1; //Assign this from a weapon variable if you'd like
                    rightHandConstraint.data.targetRotationWeight = 1;
                }
                if(leftHandTarget != null)
                {
                    leftHandConstraint.data.target = leftHandTarget.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
            else
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }

        public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
        {
            if (characterManager.isInteracting)
                return;

            if(handIKWeightsReset)
            {
                if (rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandIK.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }

                if (leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandIK.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
        }

        public virtual void EraseHandIKForWeapon()
        {
            handIKWeightsReset = true;

            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                rightHandConstraint.data.targetRotationWeight = 0;
            }

            if(leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0;
            }
        }
    }
}