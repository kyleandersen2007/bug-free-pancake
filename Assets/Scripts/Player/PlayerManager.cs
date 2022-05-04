using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KA
{
    public class PlayerManager : CharacterManager
    {
        [Header("Variables")]
        public InputHandler inputHandler;
        public CameraHandler cameraHandler;
        public PlayerLocomotion playerLocomotion;
        public PlayerStats playerStats;
        public PlayerAnimatorManager animatorHandler;
        public PlayerInventory playerInventoryManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerCombatManager playerCombatManager;
        public WeaponSlotManager weaponSlotManager;
        public PlayerEquipmentManager playerEquipmentManager;
        public UIManager uIManager;
        public QuickSlotsUI quickSlotsUI;
        public BlockingCollider blockingCollider;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponent<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStats = GetComponent<PlayerStats>();
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            uIManager = FindObjectOfType<UIManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            canRotate = anim.GetBool("canRotate");
            isInvulnerable = anim.GetBool("isInvulnerable");
            isHoldingArrow = anim.GetBool("isHoldingArrow");
            anim.SetBool("isInAir", isInAir);
            anim.SetBool("isBlocking", isBlocking);
            anim.SetBool("isDead", isDead);
            anim.SetBool("isTwoHandingWeapon", isTwoHandingWeapon); 

            inputHandler.TickInput(delta);
            
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerStats.RegenerateStamina();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            if(cameraHandler == null)
            {
                return;
            }
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRotation(delta);
        }

        private void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.y_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string currentInteractableText = interactableObject.interactableText;
                        interactableUI.text.text = currentInteractableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.y_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.y_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            animatorHandler.PlayTargetAnimation("Open Chest", true);
        }
    }
}