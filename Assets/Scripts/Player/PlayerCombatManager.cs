using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager playerManager;

        [Header("Attack Animations")]
        public string oh_light_attack_1;
        public string oh_light_attack_2;
        public string oh_heavy_attack_1;
        public string oh_heavy_attack_2;
        public string th_light_attack_1;
        public string th_light_attack_2;
        public string th_heavy_attack_1;
        public string th_heavy_attack_2;
        public string oh_running_attack_01;
        public string oh_jumping_attack_01;
        public string th_running_attack_01;
        public string th_jumping_attack_01;

        public string oh_charge_attack_01;
        public string oh_charge_attack_02;

        public string th_charge_attack_01;
        public string th_charge_attack_02;
        public string draw_Arrow;

        public string weaponArt;

        public string lastAttack;

        public LayerMask backStabLayer;
        public LayerMask riposteLayer;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        private void SuccessfullyCastSpell()
        {
            playerManager.playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerManager.animatorHandler, playerManager.playerStats, playerManager.weaponSlotManager, playerManager.isUsingLeftHand);
        }

        public void AttemptBackStabOrReposte()
        {
            if (playerManager.playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;

            if(Physics.Raycast(playerManager.inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerManager.weaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerManager.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerManager.animatorHandler.PlayTargetAnimation("Backstab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Backstabbed", true);
                }
            }
            else if(Physics.Raycast(playerManager.inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerManager.weaponSlotManager.rightHandDamageCollider;
                
                if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPoint.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerManager.playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerManager.animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }
}
