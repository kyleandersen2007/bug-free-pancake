using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            if (player.isHoldingArrow)
                return;

            player.anim.SetBool("isHoldingArrow", true);
            player.animatorHandler.PlayTargetAnimation("Bow_TH_Draw_01", true);
            GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.weaponSlotManager.leftHandSlot.transform);
            player.playerEffectsManager.currentRangeEffects = loadedArrow;
            Animator bowAnimator = player.weaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw");
        }
    }
}
