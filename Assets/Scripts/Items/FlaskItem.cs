using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(AnimatorHandler animatorHandler, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(animatorHandler, weaponSlotManager, playerEffectsManager);
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountToBeHealed = healthRecoverAmount;
            playerEffectsManager.instantiatedFXModel = itemModel;
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}