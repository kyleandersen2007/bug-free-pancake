using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Attempt Critical Action")]
    public class CriticalAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            player.playerCombatManager.AttemptBackStabOrReposte();
        }
    }
}
