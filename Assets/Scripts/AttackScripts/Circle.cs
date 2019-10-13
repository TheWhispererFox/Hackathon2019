using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float areaOfEffect;
    public LayerMask whatIsDestructible;
    public int damage;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            Collider2D[] objectsToDamage =
                Physics2D.OverlapCircleAll(transform.position, areaOfEffect, whatIsDestructible);

            for (int i = 0; i < objectsToDamage.Length; i++)
            {
                objectsToDamage[i].GetComponent<DestructibleEnvi>().health -= damage;
            }

            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}
