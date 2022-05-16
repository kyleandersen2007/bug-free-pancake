using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KA
{
    public class BodyEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;

        BodyEquipment item;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(BodyEquipment bodyEquipment)
        {
            item = bodyEquipment;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            uIManager.bodyEquipmentSlotSelected = true;
        }
    }
}
