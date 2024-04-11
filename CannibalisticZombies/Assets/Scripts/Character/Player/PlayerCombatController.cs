using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies {

    /// -////////////////////////////////////////////////////////////////////
    /// author: Ashley Roman
    /// combat system
    /// 

    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;
        [SerializeField] private float launchVelocity = 150f, attackCooldown = 1f;
        [SerializeField] private Boolean canShoot = true;
        GameObject bullet;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                shootBullet();
            }
        }

        ///-////////////////////////////////////////////////////////////////////
        /// method for shooting: checks if player can shoot; after every shot, make canShoot false, and call shootColdown() to wait until canShoot is true again. 
        ///
        private void shootBullet()
        {
            if (canShoot == true)
            {
                bullet = Instantiate(projectile, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * launchVelocity;
                canShoot = false;
                StartCoroutine(shootCooldown());
            }

        }
        ///-//////////////////////////////////////////////////////////////////// 
        /// Coroutine for shooting cooldown and making canShoot = true. 
        /// 
        private IEnumerator shootCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            canShoot = true;
        }


    }
    
}
