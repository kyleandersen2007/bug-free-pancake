using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Aim Action")]
    public class AimAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isAiming)
                return;

            player.uIManager.crosshair.SetActive(true);
            player.isAiming = true;
        }
    }
}
