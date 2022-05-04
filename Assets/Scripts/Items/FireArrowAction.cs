using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = player.weaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = player.weaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_TH_Fire");
            player.animatorHandler.PlayTargetAnimation("Bow_TH_Fire_01", true);
            player.anim.SetBool("isHoldingArrow", false);
            Destroy(player.playerEffectsManager.currentRangeEffects);

            GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if (player.isAiming)
            {
                Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;

                if (Physics.Raycast(ray, out hitPoint, 100))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                    Debug.Log(hitPoint.transform.name);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                if (player.cameraHandler.currentLockOnTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }
            rigidbody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity);
            rigidbody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity);
            rigidbody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;
            rigidbody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = player;
            damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
        }
    }
}
