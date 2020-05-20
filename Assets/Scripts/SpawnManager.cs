using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] customerSpawns; //Spawn locations of the customers.
    public GameObject customerPrefab; // Customer model. 
    public GameObject pizzaSpawner; // Spawn location of the pizza from in the oven. 
    public GameObject deliverySpawner; // Spawn location of the delivery crate. 
    public GameObject deliveryCrate; // Delivery Crate
    public GameObject pizzaPrefab; // Pizza model. 

    private List<GameObject> customers = new List<GameObject>();
    public List<GameObject> Customers
    {
        get { return customers; }
    }

    private List<GameObject> pizzas = new List<GameObject>();

    public List<GameObject> Pizzas
    {
        get { return pizzas; }
    }

    private GameManager _gameManager;
    private DeliverySystem _deliverySystem;

    private bool crateSpawned;

    //Spawning Customers.
    private int minSpawnTimer = 1;
    private int maxSpawnTimer = 4;
    private int spawnerIndex;
    private float spawnInterval;
    private int dayCount = 0;


    //Varibles relating to wave size
    private bool isFirstWave = true;
    private int lastWave;
    private int customersPerWave; //previously public
    private int minCustomerPerWave = 4; // 4 
    private int maxCustomerPerWave = 7; // 7
    private int newWave;
    private bool waveIsActive = false;
    private int customersThisWave; // variable to access the counting down of the customers per wave in gamemanager

    //Spawn timer fot the pizza. 
    [SerializeField] float secondsBetweenPizzaSpawns = 3f;
    [SerializeField] float pizzaSpawnTimer = 0f;
    private float defaultPizzaSpawnTimer;

    private float pizzaCostToPlayer = 3f;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();
        defaultPizzaSpawnTimer = secondsBetweenPizzaSpawns;
    }


    // Update is called once per frame
    void Update()
    {

        if (Customers.Count != 0)
        {
            if (Time.time > pizzaSpawnTimer) // if the time since this frame is more than the value of pizzaspawn timer
            {
                pizzaSpawnTimer = Time.time + secondsBetweenPizzaSpawns; //change pizza spawn timer to current time + another X seconds.. 
                Instantiate(pizzaPrefab, pizzaSpawner.transform.position, pizzaPrefab.transform.rotation);
                _gameManager.RemoveMoney(pizzaCostToPlayer);

            }
        }
        NewDeliveryCrate();
    }


    public void ChangePizzaSpawnTimer(float timeToAdd)
    {
        secondsBetweenPizzaSpawns += timeToAdd;
    }

    public void DefaultPizzaSpawnTimer()
    {
        secondsBetweenPizzaSpawns = defaultPizzaSpawnTimer;
    }

    //Spawns the actual customers as specified by the wave logic.
    IEnumerator SpawnCustomers(int customerNum)
    {
        if (waveIsActive)
        {

            lastWave = customerNum; // Sets the last wave amount to the current number given to spawn.
            customersThisWave = customerNum;
            for (int i = 0; i < customerNum; i++)
            {
                spawnerIndex = Random.Range(0, customerSpawns.Length); // Gets a random spawn location from which the customer will be spawned. 
                spawnInterval = Random.Range(minSpawnTimer, maxSpawnTimer); //Calculates a spawn timer in between enemy spawns of the wave using random min/max values

                yield return new WaitForSeconds(spawnInterval);
                GameObject spawnedCustomer = Instantiate(customerPrefab, customerSpawns[spawnerIndex].transform.position, customerPrefab.transform.rotation);
                Customers.Add(spawnedCustomer);
            } 

            isFirstWave = false;
            WaveActive = false;
        }
    }

    public void SpawnExtraCustomers()
    {
        int extraCustomers = Random.Range(3, 7);

        for(int i = 0; i < extraCustomers; i++)
        {
            int index = Random.Range(0, customerSpawns.Length);
            GameObject spawnedCustomer = Instantiate(customerPrefab, customerSpawns[index].transform.position, customerPrefab.transform.rotation);
            Customers.Add(spawnedCustomer);
        }
        _gameManager.UpdateCustomers(extraCustomers);
        Debug.LogWarning("Spawned Extra Customers");

    }
    //logic for the spawning of the waves of customers and when. 
    public void SpawnWave()
    {

        //customerCount = FindObjectsOfType<HungryCustomerMovement>().Length; //Gets the current objects with the hungry customer script active in the scene. 
        if (!WaveActive)
        {
            dayCount++;
            WaveActive = true;
            customersPerWave = Random.Range(minCustomerPerWave, maxCustomerPerWave);
            newWave = lastWave + customersPerWave;

            // First we update the number of customers, then we spawn the wave.
            _gameManager.UpdateCustomers(newWave);
            StartCoroutine(SpawnCustomers(newWave));

        }
    }

    //General Spawn method used to instantiate anything.
    public void Spawn(GameObject obj, Vector3 loc, Quaternion rot) 
    {
        Instantiate(obj, loc, rot);
    }

    //Checks if the delivery crate does not exist && If its not the first wave of the game, && if the player already doesn't have an active delivery.
    private void NewDeliveryCrate()
    {
        if (!CrateSpawned)
        {
            if (_gameManager.GameIsRunning && !isFirstWave)
            {
                if (GameObject.Find("DeliveryCrate(Clone)") == null)
                {
                    StartCoroutine(SpawnDeliveryCrate());
                }
            }
        }
    }
    //Spawns particle system. 
    public void SpawnParticle(GameObject obj)
    {
        //Instantiate(obj, loc , rot);
        Instantiate(obj, new Vector3(7.5f, 1f, -5.5f), Quaternion.identity);
    }

    //Spawns the crate after a set time, 
    private IEnumerator SpawnDeliveryCrate()
    {
        int minSpawnTime = 5;
        int maxSpawnTime = 10;

        int crateSpawnTimer = Random.Range(minSpawnTime, maxSpawnTime);

        CrateSpawned = true;
        yield return new WaitForSeconds(crateSpawnTimer);
        Instantiate(deliveryCrate, deliverySpawner.transform.position, deliveryCrate.transform.rotation);

    }

    public bool CrateSpawned
    {
        get { return crateSpawned; }
        set { crateSpawned = value; }
    }

    public int DayCount
    {
        get { return dayCount; }
        set { dayCount = value; }
    }

    public bool WaveActive
    {
        get { return waveIsActive; }
        set { waveIsActive = value; }
    }

}
