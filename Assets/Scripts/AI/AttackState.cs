using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public EnemyAttackAction currentAttack;
        public PursueTargetState pursueTargetState;
        public RotateTowardsTargetState rotateTowardsTargetState;

        public bool hasPerformedAttack = false;
        bool willDoComboOnNextAttack = false;

        public override State Tick(EnemyManager enemyManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            RotateTowardsTargetWhilstAttacking(enemyManager);

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyManager.enemyAnimatorHandler, enemyManager);
            }

            if(!hasPerformedAttack)
            {
                AttackTarget(enemyManager);
                RollForComboChance(enemyManager);
            }

            if(willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;

        }

        private void AttackTarget(EnemyManager enemyManager)
        {
            enemyManager.anim.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
            enemyManager.anim.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
            enemyManager.enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.enemyAnimatorHandler.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
        {
            enemyManager.anim.SetBool("isUsingRightHand", currentAttack.isRightHandedAction);
            enemyManager.anim.SetBool("isUsingLeftHand", !currentAttack.isRightHandedAction);
            willDoComboOnNextAttack = false;
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorHandler.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Rotate manually
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if(currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}
