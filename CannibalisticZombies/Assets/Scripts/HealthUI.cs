using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace CannibalisticZombies {
    //-/////////////////////////////////////////////////////////////////////
    ///UI Health Element
    public class HealthUI : MonoBehaviour
    {
        // HealthUI displays current health.
        // Calling SetHealth() with new Damage amount update healthBar to that amount.
        [SerializeField] private Image healthBar;
        public float currentHealth; //unnecessary in implementation
        public float animationSpeed; //Rate at which damage bar fills/empties per frame
        public Color damageColor; //Color of bar when take damage (RED)
        public Color healColor; //Color of bar when heals/removes damage (GREEN)
        private float fillTarget; //Decimal amount of damage bar to be filled



        //-/////////////////////////////////////////////////////////////////////
        // Update is called once per frame
        // Should only contain UpdateHealth() in final implementation
        void Update()
        {
            TestMethod(); // Remove in actual implementation
            UpdateHealth();
        }

        //-/////////////////////////////////////////////////////////////////////
        //Sets new health UI target fill Amount
        //Requires current maxHealth (defaults to 100)
        public void SetHealth(float newHealth, float maxHealth = 100f)
        {

            fillTarget = 1 - newHealth / maxHealth;
            fillTarget = Mathf.Clamp(fillTarget, 0, 1);
            currentHealth = Mathf.Clamp(newHealth, 0, maxHealth); //Remove in actual implementation


        }
        //-/////////////////////////////////////////////////////////////////////
        //Serves as a simple internal Health Manager
        //Call in Update to test 
        //Called in Update() for testing, not needed for actual implementation
        //Press enter to take 20 damage
        //Press space to heal 10 damage
        private void TestMethod() 
        {    
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SetHealth(currentHealth - 20);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                SetHealth(currentHealth + 10);
            }
        }

        //-/////////////////////////////////////////////////////////////////////
        //Frame change in Health UI fill Amount
        //Based on animationSpeed
        //Call in Update() to animate Health Bar
        private void UpdateHealth() {
            //When health bar is up to date
            if (healthBar.fillAmount == fillTarget) { return; }
            
            //When frame animation is negligible
            if (Mathf.Abs(healthBar.fillAmount - fillTarget) <= animationSpeed)
            { 
                healthBar.color = damageColor;
                healthBar.fillAmount = fillTarget;
            }
            
            //When player takes damage
            else if (healthBar.fillAmount < fillTarget) {
                healthBar.color = damageColor;
                healthBar.fillAmount += animationSpeed;
            }
            
            //When player heals/removes damage
            else if (healthBar.fillAmount > fillTarget)
            {
                healthBar.color = healColor;
                healthBar.fillAmount -= animationSpeed;
            }

        }

    }

}
