using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;

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

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
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

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Character")
            {
                CharacterStatsManager enemyStats = other.GetComponent<CharacterStatsManager>();
                CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffectsManager = other.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();

                if(enemyCharacterManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    if(enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponent<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if(shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                        float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;

                        if(enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0, "Block Guard");
                            return;
                        }
                    }
                }

                if(enemyStats != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;

                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseResetTime = enemyStats.totalPoiseDefence - poiseBreak;
                    Debug.Log("Player's Poise is Currently " + enemyStats.totalPoiseDefence);

                    Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(physicalDamage, fireDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, 0);
                    }
                }
            }
        }
    }
}
