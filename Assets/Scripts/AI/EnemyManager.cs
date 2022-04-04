using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace KA
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;
        EnemyAnimatorHandler enemyAnimatorHandler;
        EnemyStats enemyStats;
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
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidBody = GetComponent<Rigidbody>();
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
            
            isRotatingWithRootMotion = enemyAnimatorHandler.anim.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorHandler.anim.GetBool("isInteracting");
            canDoCombo = enemyAnimatorHandler.anim.GetBool("canDoCombo");
            canRotate = enemyAnimatorHandler.anim.GetBool("canRotate");
            enemyAnimatorHandler.anim.SetBool("isDead", enemyStats.isDead);
        }

        private void LateUpdate()
        {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if(enemyStats.isDead)
            {
                SwitchToNextState(null);
                currentTarget = null;
            }

            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

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
