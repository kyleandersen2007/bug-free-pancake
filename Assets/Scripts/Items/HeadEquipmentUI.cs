using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KA
{
    public class HeadEquipmentUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;

        HelmetEquipment headItem;

        private void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(HelmetEquipment helmetEquipment)
        {
            headItem = helmetEquipment;
            icon.sprite = headItem.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            headItem = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            uIManager.headEquipmentSlotSelected = true;
        }
    }
}
