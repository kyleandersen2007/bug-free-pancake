using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        private InputHandler inputHandler;
        private PlayerInventory playerInventory;

        [Header("Equipment Model Changers")]
        HelmetModelChanger helmetModelChanger;
        TorsoEquipmentChanger torsoEquipmentChanger;
        HipModelChanger hipModelChanger;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoEquipmentChanger = GetComponentInChildren<TorsoEquipmentChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }

        private void EquipAllEquipmentModelsOnStart()
        {
            helmetModelChanger.UnEquipAllHelmetModels();

            if(playerInventory.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            }
            else
            {
                nakedHeadModel.SetActive(true);
            }

            torsoEquipmentChanger.UnEquipAllTorsoModels();

            if(playerInventory.currentTorsoEquipment != null)
            {
                torsoEquipmentChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            }
            else
            {
                torsoEquipmentChanger.EquipTorsoModelByName(nakedTorsoModel);
            }

            hipModelChanger.UnEquipAllHipModels();

            if(playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
            }
        }

        public void OpenBlockingCollider()
        {
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }

            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}