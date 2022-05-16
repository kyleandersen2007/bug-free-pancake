using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStats.currentStamina <= 0)
                return;

            player.animatorHandler.EraseHandIKForWeapon();
            player.playerEffectsManager.PlayWeaponFX(false);

            //If we can do a combo, we combo
            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleChargeWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;

                if (player.canDoCombo)
                    return;

                HandleChargeAttack(player);
            }
        }

        private void HandleChargeAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_charge_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_charge_attack_01;
                }
                else
                {
                    player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_01;
                }
            }
        }

        private void HandleChargeWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.anim.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_charge_attack_01)
                    {
                        player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_02, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_02;
                    }
                    else
                    {
                        player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_01, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_01;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_charge_attack_01)
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_charge_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_charge_attack_02;
                        }
                        else
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.th_charge_attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_charge_attack_01;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_charge_attack_01)
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_02;
                        }
                        else
                        {
                            player.animatorHandler.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_01;
                        }
                    }
                }
            }
        }
    }
}
