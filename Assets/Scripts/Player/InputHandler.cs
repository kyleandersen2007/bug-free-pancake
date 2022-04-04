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
        public bool rb_Input;
        public bool lt_Input;
        public bool lb_Input;
        public bool use_Input;
        public bool hold_RB_Input;
        public bool rt_Input;
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
        PlayerCombatManager playerAttacker;
        PlayerInventory playerInventory;
        PlayerEffectsManager playerEffectsManager;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        public UIManager uiManager;
        WeaponSlotManager weaponSlotManager;
        PlayerAnimatorManager animatorHandler;
        BlockingCollider blockingCollider;
        PlayerStats playerStats;

        public Transform criticalAttackRayCastStartPoint;

        Vector2 movementInput;
        Vector2 cameraInput;
        #endregion 

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerCombatManager>();
            playerInventory = GetComponent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerManager = GetComponent<PlayerManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            playerStats = GetComponent<PlayerStats>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.HoldRB.performed += i => hold_RB_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_RB_Input = false;
                inputActions.PlayerActions.HoldRB.canceled += i => fireFlag = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
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
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
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
            HandleMoveInput(delta);
            HandleRollInput(delta);

            HandleTapRBInput();
            HandleHoldRBInput();
            HandleTapRTInput();
            HandleTapLTInput();
            HandleLBInput();
            HandleFireBowInput();

            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
        }

        private void HandleMoveInput(float delta)
        {
            if(playerManager.isHoldingArrow)
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

                if (playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
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
            if (rb_Input)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventory.currentItemBeingUsed = playerInventory.rightWeapon;
                playerInventory.rightWeapon.tap_RB_Action.PerformAction(playerManager);
            }
        }

        private void HandleTapRTInput()
        {
            if (rt_Input)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventory.currentItemBeingUsed = playerInventory.rightWeapon;
                playerInventory.rightWeapon.tap_RT_Action.PerformAction(playerManager);
            }
        }

        private void HandleTapLTInput()
        {
            if (lt_Input)
            {
                if (playerManager.isTwoHandingWeapon)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventory.currentItemBeingUsed = playerInventory.rightWeapon;
                    playerInventory.rightWeapon.tap_LT_Action.PerformAction(playerManager);
                }
                else
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(false);
                    playerInventory.currentItemBeingUsed = playerInventory.leftWeapon;
                    playerInventory.leftWeapon.tap_LT_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleLBInput()
        {
            if (playerManager.isInAir || playerManager.isSprinting)
            {
                lb_Input = false;
                return;
            }

            if (lb_Input)
            {
                playerAttacker.HandleLBAction();
            }
            else if(lb_Input == false)
            {
                if(playerManager.isAiming)
                {
                    playerManager.isAiming = false;
                    uiManager.crosshair.SetActive(false);
                    cameraHandler.ResetAimCameraRotations();
                }
                
                if (blockingCollider.blockingCollider.enabled)
                {
                    playerManager.isBlocking = false;
                    blockingCollider.DisableBlockingCollider();
                }

            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if(x_Input)
            {
                x_Input = false;
                twoHandFlag = !twoHandFlag;

                if(twoHandFlag)
                {
                    playerManager.isTwoHandingWeapon = true;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    playerManager.isTwoHandingWeapon = false;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                    weaponSlotManager.LoadTwoHandIKTargets(false);
                }
            }
        }

        private void HandleHoldRBInput()
        {
            if(hold_RB_Input)
            {
                if(playerInventory.rightWeapon.weaponType == WeaponType.Bow)
                {
                    playerAttacker.HandleHoldRBAction();
                }
                else
                {
                    hold_RB_Input = false;
                    playerAttacker.AttemptBackStabOrReposte();
                }
            }
        }

        private void HandleFireBowInput()
        {
            if(fireFlag)
            {
                if(playerManager.isHoldingArrow)
                {
                    fireFlag = false;
                    playerAttacker.FireArrowAction();
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if(use_Input)
            {
                use_Input = false;
                playerInventory.currentConsumbale.AttemptToConsumeItem(animatorHandler, weaponSlotManager, playerEffectsManager);
            }
        }
    }
}