using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class MagicSpellAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            if (player.playerInventoryManager.currentSpell != null && player.playerInventoryManager.currentSpell.isMagicSpell)
            {
                if (player.playerStats.currentFocusPoints >= player.playerInventoryManager.currentSpell.focusPointCost)
                {
                    player.playerInventoryManager.currentSpell.AttemptToCastSpell(player.animatorHandler, player.playerStats, player.weaponSlotManager, player.isUsingLeftHand);
                }
                else
                {
                    player.animatorHandler.PlayTargetAnimation("Cant Spell", true);
                }
            }
        }
    }
}
