using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KA
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        PlayerManager player;
        private int vertical;
        private int horizontal;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else v = 0;
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else h = 0;
            #endregion

            if(isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            player.anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            player.anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        private void OnAnimatorMove()
        {
            if (player.isInteracting == false) return;

            float delta = Time.deltaTime;

            player.playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = player.anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            player.playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}