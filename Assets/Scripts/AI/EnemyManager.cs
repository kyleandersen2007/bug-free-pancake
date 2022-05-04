using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace KA
{
    public class EnemyManager : CharacterManager
    {
        public EnemyLocomotion enemyLocomotion;
        public EnemyEffectsManager enemyEffectsManager;
        public EnemyAnimatorHandler enemyAnimatorHandler;
        public EnemyStats enemyStats;
        public CharacterStatsManager currentTarget;

        public State currentState;
        
        public NavMeshAgent navmeshAgent;
        public Rigidbody enemyRigidBody;

        public bool isPreformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        //The higher, and lower, respectively these angles are, the greater detection FIELD OF VIEW (basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;

        [Header("AI Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyHood;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            enemyAnimatorHandler = GetComponent<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidBody = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            navmeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();
            
            isRotatingWithRootMotion = anim.GetBool("isRotatingWithRootMotion");
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            canRotate = anim.GetBool("canRotate");
            anim.SetBool("isDead", isDead);
        }

        private void LateUpdate()
        {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if(isDead)
            {
                SwitchToNextState(null);
                currentTarget = null;
            }

            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPreformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPreformingAction = false;
                }
            }
        }
    }
}
