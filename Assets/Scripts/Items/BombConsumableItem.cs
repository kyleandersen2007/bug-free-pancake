using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item/Consumables/Bomb Item")]
    public class BombConsumableItem : ConsumableItem
    {
        [Header("Velocity")]
        public int upwardVelocity = 50;
        public int forwardVelocity = 50;
        public int bombMass = 1;

        [Header("Live Bomb Model")]
        public GameObject liveBombModel;

        [Header("Base Damage")]
        public int baseDamage = 200;
        public int explosiveDamage;

        public override void AttemptToConsumeItem(PlayerAnimatorManager animatorHandler, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if(currentItemAmount > 0)
            {
                weaponSlotManager.rightHandSlot.UnloadWeapon();
                animatorHandler.PlayTargetAnimation(consumeAnimation, true);
                GameObject bombModel = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform.position, Quaternion.identity, weaponSlotManager.rightHandSlot.transform);
                playerEffectsManager.instantiatedFXModel = bombModel;
            }
            else
            {
                //Can't
            }
        }
    }
}
