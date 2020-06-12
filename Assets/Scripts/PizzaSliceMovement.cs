using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSliceMovement : MonoBehaviour
{


    public GameObject happyCustomerPrefab;

    private Rigidbody sliceRb;
    private float sliceSpeed = 25f;

    private bool hasHit; 
    // Start is called before the first frame update
    void Start()
    {
        sliceRb = GetComponent<Rigidbody>();
        sliceRb.velocity = transform.forward * sliceSpeed;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Customer")) {

            //Minus's one from the hunger value on hit before any logic is completed. This was to stop the hunger 1's from having to take 2 pizzas instead of their intended 1. 
            other.transform.GetComponent<HungryCustomerMovement>().customer.UpdateHungerValue();
            if (other.transform.GetComponent<HungryCustomerMovement>().customer.HungerValue <= 0)
            {
                Destroy(other.gameObject);
                SpawnHappyCustomer(other.transform.position, other.transform.rotation);
                GameManager.instance.RemoveCustomer(); //removes a customer from the active customer count
                SpawnManager.instance.CustomerList.Remove(other.gameObject); //Removes the customer from the customers list. 
                AudioManager.instance.PlaySound(AudioManager.SoundType.CUSTOMER_FED);
                SpawnManager.instance.SpawnParticle();
                Destroy(gameObject);
                GameManager.instance.AddMoney(other.transform.GetComponent<HungryCustomerMovement>().customer.TipValue);
            } else
            {
                AudioManager.instance.PlaySound(AudioManager.SoundType.CUSTOMER_FED);
                other.transform.GetComponent<HungryCustomerMovement>().UpdateSprite();
                Destroy(gameObject);
                
                
            }
        }

        if (other.gameObject.CompareTag("Counter") || other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }

    //method to spawn customers. 
    private void SpawnHappyCustomer(Vector3 spawnLoc, Quaternion rotation) {
        Instantiate(happyCustomerPrefab, spawnLoc, rotation);
    }

    // Destroys slices when out of camera view. 
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }




}
