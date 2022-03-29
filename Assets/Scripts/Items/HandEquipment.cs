using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item/Equipment/Hand Equipment")]
    public class HandEquipment : EquipmentItem
    {
        public string leftHandModelName;
        public string rightHandModelName;
        public string lowerLeftArmModelName;
        public string lowerRightArmModelName;
        public string rightElbowModel;
        public string leftElbowModel;
    }
}
