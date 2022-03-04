using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponSlotHolder leftHandSlot;
        public WeaponSlotHolder rightHandSlot;
        public WeaponSlotHolder backSlot;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;
    }
}
