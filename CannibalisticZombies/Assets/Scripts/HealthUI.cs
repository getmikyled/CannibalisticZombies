using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // HealthUI displays current health.
    // Calling UpdateHealth() with new Damage amount with update healthBar to that amount.
    public GameObject health;
    public Image healthBar;
    public float currentHealth;

    public  float animationTime=1f; // The time it takes the damge bar to move up/down



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TestMethod(); // Remove in actual implementation
    }

    public void UpdateHealth(float newHealth, float maxHealth = 100f)
    {
        //newHealth is the newHealth to be displayed
        //maxHealth is maximum health, defaults to 100
        float fillTarget = 1- newHealth / maxHealth;
        //float fillDiff = fillTarget - healthBar.fillAmount;
        fillTarget = Mathf.Clamp(fillTarget, 0, 1);
        float time = Time.time;
        if (newHealth > currentHealth){
            healthBar.color = Color.green;
        }
        else
        {
            healthBar.color = Color.red;
        }
        Debug.Log(fillTarget);
        healthBar.fillAmount= fillTarget;
        Debug.Log(healthBar.fillAmount);
        currentHealth = Mathf.Clamp(newHealth,0,maxHealth);
    }

    private void TestMethod()
    {
        // Only called in Update() for testing, not needed for actual implementation
        if (Input.GetKeyUp(KeyCode.Return))
        {
            UpdateHealth(currentHealth - 20);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            UpdateHealth(currentHealth + 10);
        }
    }

}
