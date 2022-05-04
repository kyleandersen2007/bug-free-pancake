using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Blocking Action")]
    public class BlockingAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            if (player.isBlocking)
                return;

            player.animatorHandler.PlayTargetAnimation("Block Start", false, true);
            player.playerEquipmentManager.OpenBlockingCollider();
            player.isBlocking = true;
        }
    }
}
