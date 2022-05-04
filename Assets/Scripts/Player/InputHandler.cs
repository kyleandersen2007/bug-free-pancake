using UnityEngine;
namespace KA
{
    public class InputHandler : MonoBehaviour
    {
        #region Input Variables
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        [Header("Player Input Booleans")]
        public bool b_Input;
        public bool y_Input;
        public bool x_Input;
        public bool tap_rb_Input;
        public bool hold_rb_Input;
        public bool tap_rt_Input;

        public bool hold_LB_Input;
        public bool tap_lb_Input;
        public bool tap_lt_Input;
        public bool use_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        [Header("Player Flags")]
        public bool rollFlag;
        public bool fireFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        PlayerControls inputActions;
        PlayerManager player;

        public Transform criticalAttackRayCastStartPoint;

        Vector2 movementInput;
        Vector2 cameraInput;
        #endregion 

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;
                inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
                inputActions.PlayerActions.RT.performed += i => tap_rt_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerQuickslots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickslots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += inputActions => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += inputActions => right_Stick_Left_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;               
                inputActions.PlayerActions.LT.performed += i => tap_lt_Input = true;
                inputActions.PlayerActions.LB.performed += i => hold_LB_Input = true;
                inputActions.PlayerActions.LB.canceled += i => hold_LB_Input = false;
                inputActions.PlayerActions.Use.performed += i => use_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            if (player.isDead)
                return;
            HandleMoveInput(delta);
            HandleRollInput(delta);

            HandleHoldRBInput();
            HandleHoldLBInput();

            HandleTapLBInput();
            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();

            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
        }

        private void HandleMoveInput(float delta)
        {
            if(player.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

                if(moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }

                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            
        }

        private void HandleRollInput(float delta)
        {
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;

                if (player.playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && player.playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleTapRBInput()
        {
            if (tap_rb_Input)
            {
                tap_rb_Input = false;

                if (player.playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(player);
                }
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_rb_Input)
            {
                if (player.playerInventoryManager.rightWeapon.hold_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(player);
                }
            }
        }

        private void HandleTapRTInput()
        {
            if (tap_rt_Input)
            {
                tap_rt_Input = false;

                if (player.playerInventoryManager.rightWeapon.tap_RT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(player);
                }
            }
        }

        private void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    //It will be the right handed weapon
                    if (player.playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_lb_Input)
            {
                tap_lb_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (player.isInAir ||
                player.isSprinting)
            {
                hold_LB_Input = false;
                return;
            }

            if (hold_LB_Input)
            {
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(player);
                    }
                }
            }
            else if (hold_LB_Input == false)
            {
                if (player.isAiming)
                {
                    player.isAiming = false;
                    player.uIManager.crosshair.SetActive(false);
                    player.cameraHandler.ResetAimCameraRotations();
                }

                if (player.blockingCollider.blockingCollider.enabled)
                {
                    player.isBlocking = false;
                    player.blockingCollider.DisableBlockingCollider();
                }
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                player.playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                player.playerInventoryManager.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventoryFlag)
            {
                player.uIManager.UpdateUI();
            }

            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    player.uIManager.OpenSelectWindow();
                    player.uIManager.hudWindow.SetActive(false);
                }
                else
                {
                    player.uIManager.CloseSelectWindow();
                    player.uIManager.CloseAllInventoryWindows();
                    player.uIManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.nearestLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.leftLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                }
            }

            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                player.cameraHandler.HandleLockOn();
                if(player.cameraHandler.rightLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
                }
            }
            if(player.cameraHandler != null)
            {
                player.cameraHandler.SetCameraHeight();
            }
        }

        private void HandleTwoHandInput()
        {
            if(x_Input)
            {
                x_Input = false;
                twoHandFlag = !twoHandFlag;
                
                if(twoHandFlag)
                {
                    player.isTwoHandingWeapon = true;
                    player.weaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.weaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    player.isTwoHandingWeapon = false;
                    player.weaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.weaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    player.weaponSlotManager.LoadTwoHandIKTargets(false);
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if(use_Input)
            {
                use_Input = false;
                player.playerInventoryManager.currentConsumbale.AttemptToConsumeItem(player.animatorHandler, player.weaponSlotManager, player.playerEffectsManager);
            }
        }
    }
}