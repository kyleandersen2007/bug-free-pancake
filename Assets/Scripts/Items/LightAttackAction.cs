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
            player.animatorHandler.EraseHandIKForWeapon();

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleLightWeaponCombo(player.playerInventory.rightWeapon, player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;

                if (player.canDoCombo)
                    return;

                HandleLightAttack(player.playerInventory.rightWeapon, player);
            }
        }

        private void HandleLightAttack(WeaponItem weapon, PlayerManager player)
        {
            if(player.isUsingLeftHand)
            {
                player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_light_attack_1, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_1;
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

        private void HandleLightWeaponCombo(WeaponItem weapon, PlayerManager player)
        {
            if (player.playerStats.currentStamina <= 0)
                return;

            if (player.inputHandler.comboFlag)
            {
                player.animatorHandler.anim.SetBool("canDoCombo", false);

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
