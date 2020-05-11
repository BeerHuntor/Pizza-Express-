using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject[] customerSpawns; //Spawn locations of the customers.
    public GameObject customerPrefab; // Customer model. 
    public GameObject pizzaSpawner; // Spawn location of the pizza from in the oven. 
    public GameObject deliverySpawner; // Spawn location of the delivery crate. 
    public GameObject deliveryCrate; // Delivery Crate
    public GameObject pizzaPrefab; // Pizza model. 

    private GameManager _gameManager;

    [SerializeField] bool crateSpawned;

    //Spawning Customers.
    private int minSpawnTimer = 1;
    private int maxSpawnTimer = 4;
    private int spawnerIndex;
    private float spawnInterval;
    public int dayCount = 0;


    //Varibles relating to wave size
    [SerializeField] bool isFirstWave = true;
    private int lastWave;
    private int customersPerWave;
    private int minCustomerPerWave = 3; // 4 
    private int maxCustomerPerWave = 4; // 7
    private int newWave;
    public bool waveIsActive = false; 
    public int customersThisWave; // variable to access the counting down of the customers per wave in gamemanager. 

    //Spawn timer fot the pizza. 
    public float pizzaSpawnTimer = 3f;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    // Update is called once per frame
    void Update()
    {

        if (waveIsActive)
        {
            //Checks if the current time since the start of this game is greater than the spawn timer.
            if (Time.time > pizzaSpawnTimer)
            {
                pizzaSpawnTimer += 3; //TODO Spawn timer rising exponentially causing spawns to slow down find a fix for this! 
                StartCoroutine(SpawnPizza());
            }
        }

        //NewDeliveryCrate();
        //_gameManager.DisplayDataOnPress("crate has spawned " + crateSpawned);
    }

    //spawns the pizza from the oven. 
    IEnumerator SpawnPizza() {

           yield return new WaitForSeconds(pizzaSpawnTimer);
           Instantiate(pizzaPrefab, pizzaSpawner.transform.position, pizzaPrefab.transform.rotation);

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
                Instantiate(customerPrefab, customerSpawns[spawnerIndex].transform.position, customerPrefab.transform.rotation);
            }
                isFirstWave = false;
                waveIsActive = false;
            Debug.Log("SpawnCustomers is wave is active " + waveIsActive);
        }
    }

    //logic for the spawning of the waves of customers and when. 
     public void SpawnWave() {

        //customerCount = FindObjectsOfType<HungryCustomerMovement>().Length; //Gets the current objects with the hungry customer script active in the scene. 
        if (!waveIsActive)
        {
            dayCount++;
            waveIsActive = true;
            Debug.Log("Spawn Wave() where wave is " + waveIsActive);
            customersPerWave = Random.Range(minCustomerPerWave, maxCustomerPerWave);
            newWave = lastWave + customersPerWave;

            // First we update the number of customers, then we spawn the wave.
            _gameManager.UpdateCustomers(newWave);
            StartCoroutine(SpawnCustomers(newWave));

        }
     }

    //Checks if the delivery crate does not exist && If its not the first wave of the game, && if the player already doesn't have an active delivery.
     private void NewDeliveryCrate()
    {
        if (!crateSpawned)
        {
            if (_gameManager.gameIsRunning && !isFirstWave)
            {
                if (GameObject.Find("DeliveryCrate(Clone)") == null)
                {
                    StartCoroutine(SpawnDeliveryCrate());
                }
            }
        }
    }

    //Spawns the crate after a set time, 
    private IEnumerator SpawnDeliveryCrate ()
    {
        int minSpawnTime = 5;
        int maxSpawnTime = 10;

        int crateSpawnTimer = Random.Range(minSpawnTime, maxSpawnTime);

        crateSpawned = true;
        yield return new WaitForSeconds(crateSpawnTimer); 
        Instantiate(deliveryCrate, deliverySpawner.transform.position, deliveryCrate.transform.rotation);

    }

    public void SetCrateSpawned (bool spawned)
    {
        crateSpawned = spawned;
    }

    public bool GetCrateSpawned()
    {
        return crateSpawned;
    }
}
