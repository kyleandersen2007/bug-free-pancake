using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        private InputHandler inputHandler;
        private PlayerInventory playerInventory;
        private PlayerStats playerStats;

        [Header("Equipment Model Changers")]
        HelmetModelChanger helmetModelChanger;
        TorsoEquipmentChanger torsoEquipmentChanger;
        HipModelChanger hipModelChanger;
        RightLegModelChanger rightLegModelChanger;
        LeftLegModelChanger leftLegModelChanger;

        UpperLeftArmModelChanger upperLeftArmModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;

        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public string nakedLeftHand;
        public string nakedRightHand;
        public string nakedLeftLeg;
        public string nakedRightLeg;

        public BlockingCollider blockingCollider;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
            inputHandler = GetComponent<InputHandler>();
            playerInventory = GetComponent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoEquipmentChanger = GetComponentInChildren<TorsoEquipmentChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
            upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
            lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
            lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
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
                playerStats.physicalDamageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                nakedHeadModel.SetActive(true);
                playerStats.physicalDamageAbsorptionHead = 0;
            }

            torsoEquipmentChanger.UnEquipAllTorsoModels();
            upperLeftArmModelChanger.UnEquipAllModels();
            upperRightArmModelChanger.UnEquipAllModels();

            if (playerInventory.currentTorsoEquipment != null)
            {
                torsoEquipmentChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                upperLeftArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
                playerStats.physicalDamageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;   
            }
            else
            {
                torsoEquipmentChanger.EquipTorsoModelByName(nakedTorsoModel);
                upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
                playerStats.physicalDamageAbsorptionBody = 0;
            }

            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();

            if (playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
                playerStats.physicalDamageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                playerStats.physicalDamageAbsorptionLegs = 0;
            }

            lowerLeftArmModelChanger.UnEquipAllModels();
            lowerRightArmModelChanger.UnEquipAllModels();
            leftHandModelChanger.UnEquipAllModels();
            rightHandModelChanger.UnEquipAllModels();

            if (playerInventory.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.rightHandModelName);
                playerStats.physicalDamageAbsorptionHands = playerInventory.currentHandEquipment.physicalDefense;
            }
            else
            {
                lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipModelByName(nakedLeftHand);
                rightHandModelChanger.EquipModelByName(nakedRightHand);
                playerStats.physicalDamageAbsorptionHands = 0;
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