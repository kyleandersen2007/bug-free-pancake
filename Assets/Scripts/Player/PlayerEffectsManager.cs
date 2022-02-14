using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountToBeHealed);
            //GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
            weaponSlotManager.LoadBothWeaponsOnSlots();
            Destroy(instantiatedFXModel.gameObject);
        }
    }
}