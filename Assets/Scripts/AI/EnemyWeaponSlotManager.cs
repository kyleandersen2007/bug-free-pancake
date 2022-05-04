using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void GrantWeaponAttackingPoiseBonus()
        {
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.totalPoiseDefence + characterManager.characterStatsManager.offensivePoiseBonus;
        }

        public override void ResetWeaponAttackingPoiseBonus()
        {
            characterManager.characterStatsManager.totalPoiseDefence = characterManager.characterStatsManager.armorPoiseBonus;
        }
    }
}
