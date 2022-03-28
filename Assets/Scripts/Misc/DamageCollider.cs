using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        public bool enableDamageColliderOnStartUp = false;
        protected Collider damageCollider;

        [Header("Team ID")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseDefence;

        [Header("Damage")]
        public int physicalDamage = 30;
        public int fireDamage;
        public int magicDamage;

        protected bool shieldHasBeenHit;
        protected bool hasBeenParried;
        protected string currentDamageAnimation;

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            if(!enableDamageColliderOnStartUp)
            {
                damageCollider.enabled = false;
            }
            else
            {
                damageCollider.enabled = true;
            }
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            if(damageCollider != null)
            {
                damageCollider.enabled = false;
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterStatsManager enemyStats = other.GetComponent<CharacterStatsManager>();
                CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffectsManager = other.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();

                if(enemyCharacterManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyCharacterManager);
                    CheckForBlock(enemyCharacterManager, shield, enemyStats);
                    
                }

                if(enemyStats != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;
                    
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseResetTime = enemyStats.totalPoiseDefence - poiseBreak;

                    Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyCharacterManager.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(physicalDamage, fireDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, 0, currentDamageAnimation);
                    }
                }
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                characterManager.GetComponent<AnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected void CheckForBlock(CharacterManager enemyCharacterManager, BlockingCollider shield, CharacterStatsManager enemyStats)
        {
            if (shield != null && enemyCharacterManager.isBlocking)
            {
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0, "Block Guard");
                    shieldHasBeenHit = true;
                }
            }
        }

        protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
        {
            if(direction >= 145 && direction <= 180)
            {
                Debug.Log("Hit From Front");
            }
            else if(direction <= -145 && direction >= -180)
            {
                Debug.Log("Hit From Front");
            }
            else if(direction >= -45 && direction <= 45)
            {
                Debug.Log("Hit From Back");
            }
            else if (direction >= -144 && direction <= -45)
            {
                Debug.Log("Hit From Right");
            }
            else if (direction >= 45 && direction <= 144)
            {
                Debug.Log("Hit From Left");
            }
        }
    }
}
