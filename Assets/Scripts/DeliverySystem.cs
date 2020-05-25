using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    private static DeliverySystem _instance;

    public static DeliverySystem instance
    {
        get { return _instance; }
    }


    private string[] deliveries;
    public float ActiveTime { get; private set; } = 5f;
    public bool HappyHourActive { get; private set; }
    public int HappyHourMultiplier { get; private set; } = 2;
    public bool DeliveryActive { get; private set; } //To check if already have a crate and stop collecting another one. 
    public bool DoubleSlicesActive { get; set; } //To Check if double slices delivery is active and to keep the ui icon showing. 
    public string CurrentDelivery { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        deliveries = new string[] { "ENERGIZED", "DOUBLE_SLICES", "RUSH_HOUR", "TIME_IS_DRAGGING", "OVERTIME", "WINDFALL", "CASHFLOW", };

    }
    // Gets the current delivery at random from a list of set deliveries.
    public void GetDelivery ()
    {
        int deliveryIndex = Random.Range(0, deliveries.Length);

        CurrentDelivery = deliveries[deliveryIndex];
        UIManager.instance.ShowDeliveryIcon(CurrentDelivery);
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
                StartCoroutine(WindFall());
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
        AudioManager.instance.PlaySound(AudioManager.SoundType.ENERGIZED);
        float speedChange = Random.Range(0.4f, 1f);
        PlayerMovement.instance.ChangeMovementSpeed(speedChange);

        yield return new WaitForSeconds(ActiveTime);
        PlayerMovement.instance.SetDefaultSpeed();
        UIManager.instance.HideDeliveryIcon();
    }

    // Code for the bigger hands delivery
    private void DoubleSlices()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.DOUBLE_SLICES);
        PizzaAttach.instance.SetNextPizzaBuff(true);

    }

    //Code for RushHour Delivery.
    private IEnumerator RushHour ()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.RUSH_HOUR);
        float speedChange = Random.Range(0.5f, 1f);
        foreach (GameObject obj in SpawnManager.instance.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.IncreaseMovementSpeed(speedChange); 

        }

        yield return new WaitForSeconds(ActiveTime);
        foreach (GameObject obj in SpawnManager.instance.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.SetDefaultSpeed();
        }
        UIManager.instance.HideDeliveryIcon();

    }

    //Code for Time Is Dragging Delivery. 
    private IEnumerator TimeIsDragging()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.TIME_IS_DRAGGING);
        float playerSpeedChange = Random.Range(1f, 2f);
        float pizzaSpawnTimerChange = Random.Range(0.5f, 1.5f);
        float customerSpeedChange = Random.Range(0.2f, 0.5f);
        //Slow pizza spawn down
        SpawnManager.instance.ChangePizzaSpawnTimer(pizzaSpawnTimerChange);
        //Slow customer speed
        foreach (GameObject obj in SpawnManager.instance.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.IncreaseMovementSpeed(-customerSpeedChange);

        }
        //Slow player speed
        PlayerMovement.instance.ChangeMovementSpeed(-playerSpeedChange);        
        
        yield return new WaitForSeconds(ActiveTime);
        SpawnManager.instance.DefaultPizzaSpawnTimer();
        foreach (GameObject obj in SpawnManager.instance.CustomerList)
        {
            HungryCustomerMovement script = obj.GetComponent<HungryCustomerMovement>();
            script.SetDefaultSpeed();
        }
        PlayerMovement.instance.SetDefaultSpeed();
        UIManager.instance.HideDeliveryIcon();

    }

    //Code for overtime delivery. 
    private IEnumerator OverTime()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.OVERTIME);
        SpawnManager.instance.SpawnExtraCustomers();

        yield return new WaitForSeconds(ActiveTime);
        UIManager.instance.HideDeliveryIcon();
    }
    //Code for windfall Delivery
    private IEnumerator WindFall()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.WINDFALL);
        float cashInjection = Random.Range(100f, 200f);
        GameManager.instance.AddMoney(cashInjection);
        yield return new WaitForSeconds(ActiveTime);
        UIManager.instance.HideDeliveryIcon();
    }
    //Code for cashflow Delivery.
    private IEnumerator Cashflow()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundType.CASHFLOW);
        HappyHourActive = true;

        yield return new WaitForSeconds(ActiveTime);
        HappyHourActive = false;
        UIManager.instance.HideDeliveryIcon();
    }

    

}
