using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSliceMovement : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameManager _gameManager; 
    [SerializeField] GameObject happyCustomerPrefab;
    [SerializeField] GameObject fedParticle; 
    private Rigidbody sliceRb;
    private float sliceSpeed = 25f;


    private bool hasHit; 
    // Start is called before the first frame update
    void Start()
    {
        sliceRb = GetComponent<Rigidbody>();
        sliceRb.velocity = transform.forward * sliceSpeed;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Customer")) {
            if (!hasHit) //attempt at stopping the slice from 'double spawning' happy customers. 
            {
                hasHit = true;

                Destroy(other.gameObject);
                _spawnManager.Spawn(happyCustomerPrefab, other.transform.position, other.transform.rotation);
                _spawnManager.SpawnParticle(fedParticle); // Spawns the particle Effect. 
                _gameManager.RemoveCustomer(); //removes a customer from the active customer count
                _spawnManager.Customers.Remove(other.gameObject); //Removes the customer from the customers list. 
                Destroy(gameObject);
                float sliceCost = UnityEngine.Random.Range(1.5f, 4f);
                _gameManager.AddMoney(sliceCost);  

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

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }   
    }

    // Destroys slices when out of camera view. 
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }




}
