using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager player;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void HealPlayerFromEffect()
        {
            player.playerStats.HealPlayer(amountToBeHealed);
            player.weaponSlotManager.LoadBothWeaponsOnSlots();
            Destroy(instantiatedFXModel.gameObject);
        }
    }
}