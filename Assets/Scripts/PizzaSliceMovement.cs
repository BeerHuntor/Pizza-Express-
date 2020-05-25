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
            if (!hasHit) //attempt at stopping the slice from 'double spawning' happy customers. 
            {
                hasHit = true;

                Destroy(other.gameObject);
                SpawnHappyCustomer(other.transform.position, other.transform.rotation);
                GameManager.instance.RemoveCustomer(); //removes a customer from the active customer count
                SpawnManager.instance.CustomerList.Remove(other.gameObject); //Removes the customer from the customers list. 
                AudioManager.instance.PlaySound(AudioManager.SoundType.CUSTOMER_FED);
                SpawnManager.instance.SpawnParticle();
                Destroy(gameObject);
                float sliceCost = UnityEngine.Random.Range(1.5f, 4f);
                GameManager.instance.AddMoney(sliceCost);


            }
        }

        if (other.gameObject.CompareTag("Counter"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

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
