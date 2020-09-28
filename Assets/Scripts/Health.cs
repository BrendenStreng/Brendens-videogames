using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int currentHealth;

    [SerializeField] HealthBar healthBar;
    [SerializeField] ThirdPersonMovement playerController;

    [SerializeField] AudioClip _takingDamage;
    [SerializeField] AudioClip _dying;

    public event Action Dying = delegate { };

    public void Start()
    {
        //set health and health bar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(20);
        }

        if(currentHealth <= 0)
        {
            playerController._isAlive = false;
            Debug.Log("Player has died!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hazard")
        {
            TakeDamage(20);
            AudioHelper.PlayClip2D(_takingDamage, 1f);
        }

        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage(20);
            AudioHelper.PlayClip2D(_takingDamage, 0.5f);
            Destroy(other.gameObject);
        }
    }

    //Player takes damage function
    void TakeDamage(int damage)
    {
        //change players health
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage!");

        //show change on health bar
        healthBar.SetHealth(currentHealth);
        
        if(currentHealth <= 0)
        {
            AudioHelper.PlayClip2D(_dying, .05f);
            Dying?.Invoke();
        }
    }
}
