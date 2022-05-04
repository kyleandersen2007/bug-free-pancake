using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KA
{
    public class EnemyAnimatorHandler : AnimatorManager
    {
        EnemyManager enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemy.characterStatsManager.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
        }

        public void PlayWeaponTrailFX()
        {
            enemy.enemyEffectsManager.PlayWeaponFX(false);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemy.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = enemy.anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemy.enemyRigidBody.velocity = velocity;

            if (enemy.isRotatingWithRootMotion)
            {
                enemy.transform.rotation *= enemy.anim.deltaRotation;
            }
        }
    }
}