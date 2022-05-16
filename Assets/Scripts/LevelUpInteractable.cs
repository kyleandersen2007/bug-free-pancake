using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class LevelUpInteractable : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.uIManager.levelUpWindow.SetActive(true);
        }
    }
}
