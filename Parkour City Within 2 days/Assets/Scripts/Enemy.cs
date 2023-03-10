using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject deathEffect;
    //public GameObject floorEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Instantiate(floorEffect, transform.position, Quaternion.identity);
        Debug.Log("Died " + gameObject.name);
        Destroy(gameObject);
    }
}
