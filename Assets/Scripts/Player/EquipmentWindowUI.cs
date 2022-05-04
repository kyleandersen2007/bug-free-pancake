using UnityEngine;

namespace KA
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public WeaponEquipmentSlotUI[] weaponEquipmentSlots;
        public HeadEquipmentUI headEquipmentSlot;

        public void LoadWeaponOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < weaponEquipmentSlots.Length; i++)
            {
                if (weaponEquipmentSlots[i].rightHandSlot01)
                {
                    weaponEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (weaponEquipmentSlots[i].rightHandSlot02)
                {
                    weaponEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (weaponEquipmentSlots[i].leftHandSlot01)
                {
                    weaponEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    weaponEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void LoadArmorOnEquipmentScreen(PlayerInventory playerInventory)
        {
            if (playerInventory.currentHelmetEquipment != null)
            {
                headEquipmentSlot.AddItem(playerInventory.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlot.ClearItem();
            }
        }
    }
}
