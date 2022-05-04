using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            player.animatorHandler.EraseHandIKForWeapon();

            WeaponItem parryingWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;

            if(parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                player.animatorHandler.PlayTargetAnimation("Parry_Shield", true);
            }
            else if(parryingWeapon.weaponType != WeaponType.Shield)
            {
                player.animatorHandler.PlayTargetAnimation("Parry_Shield", true);
            }
        }
    }
}
