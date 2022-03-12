using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KA
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Explosive Damage & Radius")]
        public int explosiveRadius;
        public int explosiveDamage;
        public int explosionSplashDamage;

        public Rigidbody bombRB;

        private bool hasCollided = false;
        public GameObject impactParticles;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            bombRB = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                Explode();

                CharacterStatsManager character = collision.transform.GetComponent<CharacterStatsManager>();

                if(character != null)
                {
                    if(character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosiveDamage);
                    }
                }

                Destroy(impactParticles, 5f);
                Destroy(transform.parent.parent.gameObject);
            }
        }

        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

            foreach (Collider objectsInExplosion in characters)
            {
                CharacterStatsManager character = objectsInExplosion.GetComponent<CharacterStatsManager>();

                if(character != null)
                {
                    if (character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionSplashDamage);
                    }
                }
            }
        }
    }
}
