using UnityEngine;

namespace KA
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public WeaponEquipmentSlotUI[] weaponEquipmentSlots;
        public HeadEquipmentUI headEquipmentSlot;
        public BodyEquipmentSlotUI bodyEquipmentSlotUI;
        public LegEquipmentSlotUI legEquipmentSlotUI;
        public HandEquipmentSlotUI handEquipmentSlotUI;

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

            if (playerInventory.currentTorsoEquipment != null)
            {
                bodyEquipmentSlotUI.AddItem(playerInventory.currentTorsoEquipment);
            }
            else
            {
                bodyEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentLegEquipment != null)
            {
                legEquipmentSlotUI.AddItem(playerInventory.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentHandEquipment != null)
            {
                handEquipmentSlotUI.AddItem(playerInventory.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotUI.ClearItem();
            }
        }
    }
}
