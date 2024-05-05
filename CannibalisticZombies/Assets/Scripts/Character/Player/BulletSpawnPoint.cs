using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BulletSpawn : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float launchVelocity = 150f, attackCooldown = 1f;
    [SerializeField] private Boolean canShoot = true;
    //public Camera camera;
    //public Transform crosshair;

    // Start is called before the first frame update
    void Start()
    {
        // check if gun is loaded
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootBullet();
        }
    }

    // method for shooting: checks if player can shoot; after every shot, make canShoot false
    // and call shootColdown() to wait until canShoot is true again.
    void shootBullet()
    {
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
       // RaycastHit hit;

        if (canShoot == true)
        {
           // Physics.Raycast(ray, out hit);
            GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * launchVelocity;
            canShoot = false;
            StartCoroutine(shootCooldown());
        }
        
    }

    // Coroutine for shooting cooldown and making canShoot
    public IEnumerator shootCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canShoot = true;
    }

    private void Awake()
    {
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }*/

}
