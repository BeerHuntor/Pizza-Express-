using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PizzaAttach _pizzaAttach;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private UIManager _uiManager;

    private string[] deliveries;
    public float ActiveTime { get; private set; } = 5f;
    public bool HappyHourActive { get; private set; }
    public int HappyHourMultiplier { get; private set; } = 2;
    public bool DeliveryActive { get; private set; } //To check if already have a crate and stop collecting another one. 
    public bool DoubleSlicesActive { get; set; } //To Check if double slices delivery is active and to keep the ui icon showing. 
    public string CurrentDelivery { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _pizzaAttach = GameObject.Find("Player").GetComponent<PizzaAttach>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();

        deliveries = new string[] { "ENERGIZED", "DOUBLE_SLICES", "RUSH_HOUR", "TIME_IS_DRAGGING", "OVERTIME", "WINDFALL", "CASHFLOW", };

    }
    // Gets the current delivery at random from a list of set deliveries.
    public void GetDelivery ()
    {
        int deliveryIndex = Random.Range(1, 2);

        CurrentDelivery = deliveries[deliveryIndex];
        _uiManager.ShowDeliveryIcon(CurrentDelivery);
        ExecuteDelivery();
    }

    //Code to execute the deliveries
    private void ExecuteDelivery ()
    {
        switch(CurrentDelivery)
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

    public  void StartCrateStatus()
    {
        StartCoroutine(SetCrateStatus());
    }

    private IEnumerator SetCrateStatus()
    {
        DeliveryActive = true;
        Debug.LogWarning(ActiveTime);
        yield return new WaitForSeconds(ActiveTime);
        DeliveryActive = false;
    }
    // Code for the energized delivery
    private IEnumerator Energized()
    {

        float speedChange = Random.Range(0.4f, 1f);
        _playerMovement.ChangeMovementSpeed(speedChange);

        yield return new WaitForSeconds(ActiveTime);
        _playerMovement.SetDefaultSpeed();
        _uiManager.HideDeliveryIcon();
    }

    // Code for the bigger hands delivery
    private void DoubleSlices()
    {
        _pizzaAttach.SetNextPizzaBuff(true);

    }

    //Code for RushHour Delivery.
    private IEnumerator RushHour ()
    {

        float speedChange = Random.Range(0.5f, 1f);
        foreach (GameObject obj in _spawnManager.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.IncreaseMovementSpeed(speedChange); 

        }

        yield return new WaitForSeconds(ActiveTime);
        foreach (GameObject obj in _spawnManager.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.SetDefaultSpeed();
        }
        _uiManager.HideDeliveryIcon();

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
        foreach (GameObject obj in _spawnManager.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.IncreaseMovementSpeed(-customerSpeedChange);

        }
        //Slow player speed
        _playerMovement.ChangeMovementSpeed(-playerSpeedChange);        
        
        yield return new WaitForSeconds(ActiveTime);
        _spawnManager.DefaultPizzaSpawnTimer();
        foreach (GameObject obj in _spawnManager.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.SetDefaultSpeed();
        }
        _playerMovement.SetDefaultSpeed();
        _uiManager.HideDeliveryIcon();

    }

    //Code for overtime delivery. 
    private IEnumerator OverTime()
    {
 
        _spawnManager.SpawnExtraCustomers();

        yield return new WaitForSeconds(ActiveTime);
        _uiManager.HideDeliveryIcon();
    }
    //Code for windfall Delivery
    private IEnumerator WindFall()
    {
 
        float cashInjection = Random.Range(100f, 200f);
        _gameManager.AddMoney(cashInjection);
        yield return new WaitForSeconds(ActiveTime);
        _uiManager.HideDeliveryIcon();
    }
    //Code for cashflow Delivery.
    private IEnumerator Cashflow()
    {
  
        HappyHourActive = true;

        yield return new WaitForSeconds(ActiveTime);
        HappyHourActive = false;
        _uiManager.HideDeliveryIcon();
    }

    

}
