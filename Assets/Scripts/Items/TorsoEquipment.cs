using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item/Equipment/Torso Equipment")]
    public class TorsoEquipment : EquipmentItem
    {
        public string torsoModelName;
        public string upperLeftArmModelName;
        public string upperRightArmModelName;
    }
}