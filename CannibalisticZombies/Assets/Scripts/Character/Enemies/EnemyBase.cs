using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{
    ///-////////////////////////////////////////////////////////////////////
    ///
    public class EnemyBase : CharacterBase
    {
        [Header("Enemy Properties")]
        [SerializeField] protected float stoppingDistance = 5f;
        [SerializeField] protected float health = 15;

        ///-////////////////////////////////////////////////////////////////////
        ///
        protected void Start()
        {

        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        protected void Update()
        {

        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        public void OnHit(float argDamage)
        {
            TakeDamage(argDamage);
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        protected void TakeDamage(float argDamage)
        {
            health -= argDamage;

            if (health <= 0)
            {
                Destroy(gameObject);
            } 
            
        }

    }
}
