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
        RightLegModelChanger rightLegModelChanger;
        LeftLegModelChanger leftLegModelChanger;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;
        public string nakedLeftLeg;
        public string nakedRightLeg;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoEquipmentChanger = GetComponentInChildren<TorsoEquipmentChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
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
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();

            if (playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
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