using UnityEngine;
namespace KA
{
    public class PlayerLocomotion : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Vector3 moveDirection;

        public new Rigidbody rigidbody;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        private float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        private float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float walkingSpeed = 1;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;

        [Header("Stamina Costs")]
        [SerializeField] private int rollStaminaCost = 15;
        [SerializeField] private int backstepStaminaCost = 12;
        [SerializeField] private int sprintStaminaCost = 1;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 5 | 1 << 11);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if(playerManager.canRotate)
            {
                if(playerManager.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, playerManager.cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotation;
                }
                else
                {
                    if (playerManager.inputHandler.lockOnFlag)
                    {
                        if (playerManager.inputHandler.sprintFlag || playerManager.inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = playerManager.cameraHandler.cameraTransform.forward * playerManager.inputHandler.vertical;
                            targetDirection += playerManager.cameraHandler.cameraTransform.right * playerManager.inputHandler.horizontal;
                            targetDirection.Normalize();
                            targetDirection.y = 0;

                            if (targetDirection == Vector3.zero)
                            {
                                targetDirection = transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = playerManager.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = playerManager.inputHandler.moveAmount;

                        targetDir = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
                        targetDir += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                            targetDir = transform.forward;

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);

                        transform.rotation = targetRotation;
                    }
                }
            }
        }

        public void HandleMovement(float delta)
        {
            if (playerManager.inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;

            moveDirection = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
            moveDirection += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (playerManager.inputHandler.sprintFlag && playerManager.inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerManager.playerStats.TakeStaminaDamage(sprintStaminaCost);
            }
            else
            {
                if (playerManager.inputHandler.moveAmount <= 0.5)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (playerManager.inputHandler.lockOnFlag && playerManager.inputHandler.sprintFlag == false)
            {
                playerManager.animatorHandler.UpdateAnimatorValues(playerManager.inputHandler.vertical, playerManager.inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                playerManager.animatorHandler.UpdateAnimatorValues(playerManager.inputHandler.moveAmount, 0, playerManager.isSprinting);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (playerManager.anim.GetBool("isInteracting"))
                return;

            if (playerManager.playerStats.currentStamina <= 0)
                return;

            if (playerManager.inputHandler.rollFlag)
            {
                playerManager.inputHandler.rollFlag = false;
                moveDirection = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
                moveDirection += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;

                if (playerManager.inputHandler.moveAmount > 0)
                {
                    playerManager.animatorHandler.PlayTargetAnimation("Rolling", true);
                    playerManager.animatorHandler.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = rollRotation;
                    playerManager.playerStats.TakeStaminaDamage(rollStaminaCost);
                }
                else
                {
                    playerManager.animatorHandler.PlayTargetAnimation("Backstep", true);
                    playerManager.animatorHandler.EraseHandIKForWeapon();
                    playerManager.playerStats.TakeStaminaDamage(backstepStaminaCost);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, transform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = transform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        playerManager.animatorHandler.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerManager.animatorHandler.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        playerManager.animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isInteracting || playerManager.inputHandler.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.inputHandler.jump_Input)
            {
                playerManager.inputHandler.jump_Input = false;

                if (playerManager.inputHandler.moveAmount > 0)
                {
                    moveDirection = playerManager.cameraHandler.cameraObject.transform.forward * playerManager.inputHandler.vertical;
                    moveDirection += playerManager.cameraHandler.cameraObject.transform.right * playerManager.inputHandler.horizontal;
                    playerManager.animatorHandler.PlayTargetAnimation("Jump", true);
                    playerManager.animatorHandler.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = jumpRotation;
                }
            }
        }
        #endregion
    }
}