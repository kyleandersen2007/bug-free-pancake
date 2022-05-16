using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Light Attack Actions")]
    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStats.currentStamina <= 0)
                return;

            player.animatorHandler.EraseHandIKForWeapon();
            player.playerEffectsManager.PlayWeaponFX(false);

            if (player.isSprinting)
            {
                HandleRunningAttack(player);
                return;
            }

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleLightWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;

                if (player.canDoCombo)
                    return;

                HandleLightAttack(player);
            }
        }

        private void HandleLightAttack(PlayerManager player)
        {
            if(player.isUsingLeftHand)
            {
                player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_1, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_1;
            } 
            else if(player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_light_attack_1, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_1;
                }
                else
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_1, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_1;
                }
            }
        }

        private void HandleRunningAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_running_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_running_attack_01;
                }
                else
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
                }
            }
        }

        private void HandleLightWeaponCombo(PlayerManager player)
        {
            if (player.playerStats.currentStamina <= 0)
                return;

            if (player.inputHandler.comboFlag)
            {
                player.anim.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_1)
                    {
                        player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_2, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_2;
                    }
                    else
                    {
                        player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_1, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_1;
                    }
                }
                else if(player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_light_attack_1)
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_light_attack_2, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_2;
                        }
                        else
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_light_attack_1, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_1;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_1)
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_2, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_2;
                        }
                        else
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_1, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_1;
                        }
                    }
                }
            }
        }
    }
}
