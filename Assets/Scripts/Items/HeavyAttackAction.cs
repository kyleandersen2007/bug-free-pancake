using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Actions")]
    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStats.currentStamina <= 0)
                return;

            player.animatorHandler.EraseHandIKForWeapon();

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;

                if (player.canDoCombo)
                    return;

                HandleHeavyAttack(player);
            }
        }

        private void HandleHeavyAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_1, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_1;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_1, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_1;
                }
                else
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_1, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_1;
                }
            }
        }

        private void HandleHeavyWeaponCombo(PlayerManager player)
        {
            if (player.playerStats.currentStamina <= 0)
                return;

            if (player.inputHandler.comboFlag)
            {
                player.anim.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_heavy_attack_1)
                    {
                        player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_2, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_2;
                    }
                    else
                    {
                        player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_1, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_1;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_heavy_attack_1)
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_2, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_2;
                        }
                        else
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_1, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_1;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_heavy_attack_1)
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_2, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_2;
                        }
                        else
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_1, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_1;
                        }
                    }
                }
            }
        }
    }
}
