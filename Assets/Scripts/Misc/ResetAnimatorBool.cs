using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public string isMirroredBool = "isMirrored";
        public bool isMirroredStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterManager character = animator.GetComponent<CharacterManager>();

            character.isUsingLeftHand = false;
            character.isUsingRightHand = false;

            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(canRotateBool, canRotateStatus);

            animator.SetBool(isMirroredBool, isMirroredStatus);
        }
    }
}
