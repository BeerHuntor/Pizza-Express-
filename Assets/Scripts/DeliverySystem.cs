using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    [SerializeField] List<string> deliveries = new List<string>();
    [SerializeField] float activeTime = 5f;
    [SerializeField] string currentDelivery;
    [SerializeField] bool biggerHandsReadyToUse;
    public bool gotDelivery;

    private PlayerMovement _playerMovement;
    private PizzaAttach _pizzaAttach;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private int happyHourMutiplier;
    public int HappyHourMultiplier
    {
        get { return happyHourMutiplier; }
    }
    private bool happyHour;

    public bool HappyHourActive
    {
        get { return happyHour; }
    }
    // Start is called before the first frame update
    void Start()
    {
        happyHourMutiplier = 2;
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _pizzaAttach = GameObject.Find("Player").GetComponent<PizzaAttach>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        deliveries.Add("ENERGIZED"); //Speeds up the player randomly 
        deliveries.Add("BIGGER_HANDS"); // double pizza stack
        deliveries.Add("RUSH_HOUR"); // quicker customers
        deliveries.Add("TIME_IS_DRAGGING"); //slower customers / slower player / slower pizzas
        deliveries.Add("OVERTIME"); //more customers in this wave
        deliveries.Add("WINDFALL"); //Stack of cash.
        deliveries.Add("HAPPY_HOUR"); //Double money earned.
    }
    public void Update()
    {
        
    }
    // Gets the current delivery at random from a list of set deliveries.
    public void GetDelivery ()
    {
        int deliveryIndex = Random.Range(0, deliveries.Count);

        currentDelivery = deliveries[deliveryIndex];
        ExecuteDelivery();
    }

    //Code to execute the deliveries
    private void ExecuteDelivery ()
    {
        switch(currentDelivery)
        {
            case "ENERGIZED":
                StartCoroutine(Energized());
                break;
            case "BIGGER_HANDS":
                BiggerHands();
                break;
            case "RUSH_HOUR":
                StartCoroutine(RushHour());
                break;
            case "TIME_IS_DRAGGING":
                StartCoroutine(TimeIsDragging());
                break;
            case "OVERTIME":
                OverTime();
                break;
            case "WINDFALL":
                WindFall();
                break;
            case "HAPPY_HOUR":
                StartCoroutine(HappyHour());
                break;

        }
    }

    //Setter for crate active bool
    public void SetCrateActive(bool isCrateActive)
    {
        gotDelivery = isCrateActive; 
    }

    //getter for crate active bool
    public bool GetCrateActive()
    {
        return gotDelivery;
    }

    public string GetCurrentDelivery()
    {
        return currentDelivery;
    }
    // Code for the energized delivery
    private IEnumerator Energized()
    {
        float speedChange = Random.Range(0.4f, 1f);
        _playerMovement.ChangeMovementSpeed(speedChange);

        yield return new WaitForSeconds(activeTime);
        _playerMovement.SetDefaultSpeed();
        SetCrateActive(false);
    }

    #region BIGGER HANDS
    // Code for the bigger hands delivery
    private void BiggerHands()
    {
        _pizzaAttach.SetNextPizzaBuff(true);

    }

    //Getter to check if bigger hands is ready to use.
    public bool GetBiggerHandsReadyToUse()
    {
        return biggerHandsReadyToUse;
    }

    //Setter to set if bigger hands is ready to use. 
    public void SetBiggerHandsReadyToUse(bool ready)
    {
        biggerHandsReadyToUse = ready;
    }
    #endregion


    //Code for RushHour Delivery.
    private IEnumerator RushHour ()
    {
        float speedChange = Random.Range(0.5f, 1f);
        foreach (GameObject obj in _spawnManager.Customers)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.IncreaseMovementSpeed(speedChange); 

        }

        yield return new WaitForSeconds(activeTime);
        foreach (GameObject obj in _spawnManager.Customers)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.SetDefaultSpeed();
        }
        SetCrateActive(false);
        //_hungryCustomerMovement.SetDefaultSpeed();

    }

    //Code for Time Is Dragging Delivery. 
    private IEnumerator TimeIsDragging()
    {
        float playerSpeedChange = Random.Range(1f, 2f);
        float pizzaSpawnTimerChange = Random.Range(0.5f, 1.5f);
        float customerSpeedChange = Random.Range(0.2f, 0.5f);
        //Slow pizza spawn down
        _spawnManager.ChangePizzaSpawnTimer(pizzaSpawnTimerChange);
        //Slow customer speed
        foreach (GameObject obj in _spawnManager.Customers)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.IncreaseMovementSpeed(-customerSpeedChange);

        }
        //Slow player speed
        _playerMovement.ChangeMovementSpeed(-playerSpeedChange);        
        
        yield return new WaitForSeconds(activeTime);
        _spawnManager.DefaultPizzaSpawnTimer();
        foreach (GameObject obj in _spawnManager.Customers)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.SetDefaultSpeed();
        }
        _playerMovement.SetDefaultSpeed();
        SetCrateActive(false);

    }

    //Code for overtime delivery. 
    private void OverTime()
    {
        _spawnManager.SpawnExtraCustomers();
        SetCrateActive(false);
    }
    //Code for Cash Flow Delivery
    private void WindFall()
    {
        float cashInjection = Random.Range(100f, 200f);
        _gameManager.AddMoney(cashInjection);
    }
    //Code for Happy Hour Delivery.
    private IEnumerator HappyHour()
    {
        happyHour = true;
        yield return new WaitForSeconds(activeTime);

        happyHour = false;
    }

    

}
