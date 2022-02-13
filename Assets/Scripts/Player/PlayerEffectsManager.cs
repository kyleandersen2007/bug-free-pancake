using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStats playerStats;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
        }

        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountToBeHealed);
        }
    }
}