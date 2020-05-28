using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    static SpawnManager _instance; 

    public static SpawnManager instance
    {
        get { return _instance; }
    }

   [SerializeField] GameObject[] customerSpawns; //Spawn locations of the customers.
   [SerializeField] GameObject customerPrefab; // Customer model. 
   [SerializeField] GameObject pizzaSpawner; // Spawn location of the pizza from in the oven. 
   [SerializeField] GameObject deliverySpawner; // Spawn location of the delivery crate. 
   [SerializeField] GameObject deliveryCrate; // Delivery Crate
   [SerializeField] GameObject pizzaPrefab; // Pizza model. 

   [SerializeField] GameObject particleSpawner;
   [SerializeField] GameObject particle;

    //private List<GameObject> customers = new List<GameObject>();

    private List<GameObject> customerList = new List<GameObject>();
    public List<GameObject> CustomerList { get { return customerList; } private set {; } }
    public bool CrateSpawned { get; set; }
    public bool WaveActive { get; set; }

    //Spawning Customers.
    private int minSpawnTimer = 1;
    private int maxSpawnTimer = 4;
    private int spawnerIndex;
    private float spawnInterval;



    //Varibles relating to wave size
    private bool isFirstWave = true;
    private int lastWaveSize;
    private int customersPerWave; //previously public
    private int minCustomerPerWave = 4; // 4 
    private int maxCustomerPerWave = 7; // 7
    private int newWaveSize;

    //Spawn timer fot the pizza. 
    [SerializeField] float secondsBetweenPizzaSpawns = 3f;
    [SerializeField] float pizzaSpawnTimer = 0f;
    private float defaultPizzaSpawnTimer;

    private void Awake()
    {
        if (instance == null)
        {
            _instance = this;
        }
    }


    private void Start()
    {
        defaultPizzaSpawnTimer = secondsBetweenPizzaSpawns;
    }


    // Update is called once per frame
    void Update()
    {

        if (CustomerList.Count != 0)
        {
            if (Time.time > pizzaSpawnTimer) // if the time since this frame is more than the value of pizzaspawn timer
            {
                pizzaSpawnTimer = Time.time + secondsBetweenPizzaSpawns; //change pizza spawn timer to current time + another X seconds.. 
                Instantiate(pizzaPrefab, pizzaSpawner.transform.position, pizzaPrefab.transform.rotation);
                GameManager.instance.RemoveMoney(GameManager.instance.PizzaCostToPlayer);

            }
        }
        NewDeliveryCrate();
    }


    public void SpawnParticle ()
    {
        StartCoroutine(SpawnParticles());
    }

    IEnumerator SpawnParticles()
    {
        var particles = Instantiate(particle, particleSpawner.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Destroy(particles);
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
        if (WaveActive)
        {

            lastWaveSize = customerNum; // Sets the last wave amount to the current number given to spawn.
            for (int i = 0; i < customerNum; i++)
            {
                spawnerIndex = Random.Range(0, customerSpawns.Length); // Gets a random spawn location from which the customer will be spawned. 
                spawnInterval = Random.Range(minSpawnTimer, maxSpawnTimer); //Calculates a spawn timer in between enemy spawns of the wave using random min/max values

                yield return new WaitForSeconds(spawnInterval);
                GameObject spawnedCustomer = Instantiate(customerPrefab, customerSpawns[spawnerIndex].transform.position, customerPrefab.transform.rotation);
                CustomerList.Add(spawnedCustomer);
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
            CustomerList.Add(spawnedCustomer);
        }
        GameManager.instance.UpdateCustomers(extraCustomers);
        Debug.LogWarning("Spawned Extra Customers");

    }
    //logic for the spawning of the waves of customers and when. 
    public void SpawnWave()
    {

        //customerCount = FindObjectsOfType<HungryCustomerMovement>().Length; //Gets the current objects with the hungry customer script active in the scene. 
        if (!WaveActive)
        {
            //GameManager.instance.DayCount++;
            WaveActive = true;
            customersPerWave = Random.Range(minCustomerPerWave, maxCustomerPerWave);
            newWaveSize = lastWaveSize + customersPerWave;

            // First we update the number of customers, then we spawn the wave.
            GameManager.instance.UpdateCustomers(newWaveSize);
            StartCoroutine(SpawnCustomers(newWaveSize));

        }
    }

    //Checks if the delivery crate does not exist && If its not the first wave of the game, && if the player already doesn't have an active delivery.
    private void NewDeliveryCrate()
    {
        if (!CrateSpawned)
        {
            if ( GameManager.instance.GameIsRunning && !isFirstWave)
            {
                if (GameObject.Find("DeliveryCrate(Clone)") == null)
                {
                    StartCoroutine(SpawnDeliveryCrate());
                }
            }
        }
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
        AudioManager.instance.PlaySound(AudioManager.SoundType.DELIVERY_CRATE);


    }
}
