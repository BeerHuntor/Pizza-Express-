using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    [SerializeField] string[] deliveries;
    [SerializeField] float activeTime = 5f;
    [SerializeField] string currentDelivery;
    [SerializeField] bool biggerHandsReadyToUse;
    [SerializeField] bool gotDelivery;

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
        deliveries = new string[] { "ENERGIZED", "DOUBLE_SLICES", "RUSH_HOUR", "TIME_IS_DRAGGING", "OVERTIME", "WINDFALL", "CASHFLOW", };
        //deliveries.("ENERGIZED"); //Speeds up the player randomly 
        //deliveries.Add("DOUBLE_SLICES"); // double pizza stack
        //deliveries.Add("RUSH_HOUR"); // quicker customers
        //deliveries.Add("TIME_IS_DRAGGING"); //slower customers / slower player / slower pizzas
        //deliveries.Add("OVERTIME"); //more customers in this wave
        //deliveries.Add("WINDFALL"); //Stack of cash.
        //deliveries.Add("HAPPY_HOUR"); //Double money earned.
    }
    // Gets the current delivery at random from a list of set deliveries.
    public void GetDelivery ()
    {
        int deliveryIndex = Random.Range(0, deliveries.Length);

        currentDelivery = deliveries[deliveryIndex];
        _gameManager.ShowDeliveryIcon(currentDelivery);
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
            case "DOUBLE_SLICES":
                DoubleSlices();
                break;
            case "RUSH_HOUR":
                StartCoroutine(RushHour());
                break;
            case "TIME_IS_DRAGGING":
                StartCoroutine(TimeIsDragging());
                break;
            case "OVERTIME":
                StartCoroutine(OverTime());
                break;
            case "WINDFALL":
                WindFall();
                break;
            case "CASHFLOW":
                StartCoroutine(Cashflow());
                break;

        }
    }

    //getter for crate active bool
    public bool GetCrateActive()
    {
        return gotDelivery;
    }
    public  void StartCrateStatus()
    {
        StartCoroutine(SetCrateStatus());
    }

    private IEnumerator SetCrateStatus()
    {
        gotDelivery = true;
        Debug.LogWarning(activeTime);
        yield return new WaitForSeconds(activeTime);
        gotDelivery = false;
    }
    public string GetCurrentDelivery()
    {
        return currentDelivery;
    }
    // Code for the energized delivery
    private IEnumerator Energized()
    {
        Debug.LogWarning("Energized Activated!");
        float speedChange = Random.Range(0.4f, 1f);
        _playerMovement.ChangeMovementSpeed(speedChange);

        yield return new WaitForSeconds(activeTime);
        _playerMovement.SetDefaultSpeed();
        _gameManager.HideDeliveryIcon();
    }

    #region DOUBLESLICES
    // Code for the bigger hands delivery
    private void DoubleSlices()
    {
        Debug.LogWarning("Bigger Hands Activated!");
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
        Debug.LogWarning("RushHour Activated!");
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
        _gameManager.HideDeliveryIcon();

    }

    //Code for Time Is Dragging Delivery. 
    private IEnumerator TimeIsDragging()
    {
        Debug.LogWarning("TimeIsDragging Activated");
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
        _gameManager.HideDeliveryIcon();

    }

    //Code for overtime delivery. 
    private IEnumerator OverTime()
    {
        Debug.LogWarning("OverTime Activated.");
        _spawnManager.SpawnExtraCustomers();

        yield return new WaitForSeconds(activeTime);
        _gameManager.HideDeliveryIcon();
    }
    //Code for windfall Delivery
    private IEnumerator WindFall()
    {
        Debug.LogWarning("WindFall Activated");
        float cashInjection = Random.Range(100f, 200f);
        _gameManager.AddMoney(cashInjection);
        yield return new WaitForSeconds(activeTime);
        _gameManager.HideDeliveryIcon();
    }
    //Code for cashflow Delivery.
    private IEnumerator Cashflow()
    {
        Debug.LogWarning("Happy Hour Activated!");
        happyHour = true;

        yield return new WaitForSeconds(activeTime);
        happyHour = false;
        _gameManager.HideDeliveryIcon();
    }

    

}
