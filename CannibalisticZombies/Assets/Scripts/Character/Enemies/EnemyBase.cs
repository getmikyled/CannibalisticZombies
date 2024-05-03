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
        [SerializeField] protected float damage = 5f;
        [SerializeField] protected float stoppingDistance = 5f;

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
            // Set Animation

            TakeDamage(argDamage);
        }

        ///-////////////////////////////////////////////////////////////////////
        ///
        protected void TakeDamage(float argDamage)
        {

        }
    }
}
