using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item/Ammo")]
    public class RangedAmmoItem : Item
    {
        [Header("Ammo Type")]
        public AmmoType ammoType;

        [Header("Ammo Velocity")]
        public float forwardVelocity = 550;
        public float upwardVelocity = 0;
        public float ammoMass = 0;
        public bool useGravity;

        [Header("Ammo Capacity")]
        public int carryLimit = 99;
        public int currentAmmo = 99;

        [Header("Ammo Base Damage")]
        public int physicalDamage = 50;

        [Header("Item Models")]
        public GameObject loadedItemModel;
        public GameObject liveAmmoModel;
        public GameObject penetratedModel;
        
    }
}
