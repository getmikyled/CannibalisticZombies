using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // HealthUI displays current health.
    // Calling SetHealth() with new Damage amount update healthBar to that amount.
    public Image healthBar;
    public float currentHealth;//unnecessary in implementation
    public float animationSpeed; //Rate at which damage bar fills/empties per frame
    public Color damageColor=Color.red;//Color of bar when take damage
    public Color healColor=Color.green; //Color of bar when heals/removes damage
    private float fillTarget;//Decimal amount of damage bar to be filled



    // Update is called once per frame
    void Update()
    {
        TestMethod(); // Remove in actual implementation
        UpdateHealth();
    }

    public void SetHealth(float newHealth, float maxHealth = 100f)
    {
        //newHealth is the newHealth to be displayed
        //maxHealth is maximum health, defaults to 100
        fillTarget = 1- newHealth / maxHealth;
        fillTarget = Mathf.Clamp(fillTarget, 0, 1);
        currentHealth = Mathf.Clamp(newHealth,0,maxHealth);
        Debug.Log(fillTarget);
        Debug.Log(healthBar.fillAmount);

    }

    private void TestMethod()
    {
        // Called in Update() for testing, not needed for actual implementation
        //Press enter to take 20 damage
        //Press space to heal 10 damage
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SetHealth(currentHealth - 20);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            SetHealth(currentHealth + 10);
        }
    }

    private void UpdateHealth() {
        //Call in Update function to animate HealthBar
        if(healthBar.fillAmount == fillTarget) { return; }//When health bar is up to date
        if (Mathf.Abs(healthBar.fillAmount - fillTarget) <= animationSpeed)//When frame animation is negligible
        { 
            healthBar.color = damageColor;
            healthBar.fillAmount = fillTarget;
        }
        if (healthBar.fillAmount< fillTarget) {//When player takes damage
            healthBar.fillAmount += animationSpeed;
        }
        if(healthBar.fillAmount > fillTarget)//When player heals/removes damage
        {
            healthBar.color = healColor;
            healthBar.fillAmount -=animationSpeed;
        }

    }

}
