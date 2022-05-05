using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        public PlayerManager player;

        [Header("Equipment Model Changers")]
        HelmetModelChanger helmetModelChanger;
        TorsoEquipmentChanger torsoEquipmentChanger;
        HipModelChanger hipModelChanger;
        RightLegModelChanger rightLegModelChanger;
        LeftLegModelChanger leftLegModelChanger;

        UpperLeftArmModelChanger upperLeftArmModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;
        LeftShoulderAttachmentModelChanger leftShoulderAttachmentModelChanger;
        RightShoulderAttachmentModelChanger rightShoulderAttachmentModelChanger;

        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;

        RightElbowAttachmentModelChanger rightElbowAttachmentModel;
        LeftElbowAttachmentModelChanger leftElbowAttachmentModel;

        [Header("Facial Features")]
        public GameObject[] facialFeatures;

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
            player = GetComponent<PlayerManager>();
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
            leftShoulderAttachmentModelChanger = GetComponentInChildren<LeftShoulderAttachmentModelChanger>();
            rightShoulderAttachmentModelChanger = GetComponentInChildren<RightShoulderAttachmentModelChanger>();
            rightElbowAttachmentModel = GetComponentInChildren<RightElbowAttachmentModelChanger>();
            leftElbowAttachmentModel = GetComponentInChildren<LeftElbowAttachmentModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModels();
        }

        public void EquipAllEquipmentModels()
        {
            helmetModelChanger.UnEquipAllHelmetModels();

            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                if (player.playerInventoryManager.currentHelmetEquipment)
                {
                    foreach (var feature in facialFeatures)
                    {
                        feature.SetActive(false);
                    }
                }

                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
                player.playerStats.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                nakedHeadModel.SetActive(true);
                player.playerStats.physicalDamageAbsorptionHead = 0;

                foreach (var feature in facialFeatures)
                {
                    feature.SetActive(true);
                }
            }

            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
                player.playerStats.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                nakedHeadModel.SetActive(true);
                player.playerStats.physicalDamageAbsorptionHead = 0;
            }

            torsoEquipmentChanger.UnEquipAllTorsoModels();
            upperLeftArmModelChanger.UnEquipAllModels();
            upperRightArmModelChanger.UnEquipAllModels();
            leftShoulderAttachmentModelChanger.UnEquipAllModels();
            rightShoulderAttachmentModelChanger.UnEquipAllModels();

            if (player.playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoEquipmentChanger.EquipTorsoModelByName(player.playerInventoryManager.currentTorsoEquipment.torsoModelName);
                upperLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
                leftShoulderAttachmentModelChanger.EquipModelByName(player.playerInventoryManager.currentTorsoEquipment.leftShoulderAttachment);
                rightShoulderAttachmentModelChanger.EquipModelByName(player.playerInventoryManager.currentTorsoEquipment.rightShoulderAttachment);

                player.playerStats.physicalDamageAbsorptionBody = player.playerInventoryManager.currentTorsoEquipment.physicalDefense;   
            }
            else
            {
                torsoEquipmentChanger.EquipTorsoModelByName(nakedTorsoModel);
                upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
                player.playerStats.physicalDamageAbsorptionBody = 0;
            }

            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();

            if (player.playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
                player.playerStats.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                player.playerStats.physicalDamageAbsorptionLegs = 0;
            }

            lowerLeftArmModelChanger.UnEquipAllModels();
            lowerRightArmModelChanger.UnEquipAllModels();
            leftHandModelChanger.UnEquipAllModels();
            rightHandModelChanger.UnEquipAllModels();

            if (player.playerInventoryManager.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
                leftElbowAttachmentModel.EquipModelByName(player.playerInventoryManager.currentHandEquipment.leftElbowModel);
                rightElbowAttachmentModel.EquipModelByName(player.playerInventoryManager.currentHandEquipment.rightElbowModel);

                player.playerStats.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            }
            else
            {
                lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipModelByName(nakedLeftHand);
                rightHandModelChanger.EquipModelByName(nakedRightHand);
                player.playerStats.physicalDamageAbsorptionHands = 0;
            }
        }

        public void OpenBlockingCollider()
        {
            if(player.inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(player.playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(player.playerInventoryManager.leftWeapon);
            }

            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}