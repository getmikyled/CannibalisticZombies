using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies {
    public class BulletCollision : MonoBehaviour
    {

        /// -////////////////////////////////////////////////////////////////////
        /// author: Ashley Roman
        /// combat system
        /// 

        [SerializeField] protected float damage = 5f;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
                enemy.OnHit(damage);
                Destroy(gameObject);
            }

        }
    }

}
