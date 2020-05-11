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
    private GameObject hc;
    private HungryCustomerMovement _hungryCustomerMovement;
    

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _pizzaAttach = GameObject.Find("Player").GetComponent<PizzaAttach>();
        hc = Resources.Load("Prefabs/Z_Hungry_Customer") as GameObject; //Using resources.Load to get refrence to the hungry customer game object.
        _hungryCustomerMovement = hc.GetComponent<HungryCustomerMovement>();
        deliveries.Add("ENERGIZED"); //Speeds up the player randomly 
        deliveries.Add("BIGGER_HANDS"); // double pizza stack
        deliveries.Add("RUSH_HOUR"); // quicker customers
        deliveries.Add("TIME_IS_DRAGGING"); //slower customers / slower player / slower pizzas
        deliveries.Add("OVERTIME"); //more customers in this wave
        deliveries.Add("CASHFLOW"); //more money earned.
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _hungryCustomerMovement.SetMovementSpeed(2f);
        }
    }
    // Gets the current delivery at random from a list of set deliveries.
    public void GetDelivery ()
    {
        int deliveryIndex = Random.Range(2, 3);

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

    //Code for RushHour Delivery.
    private IEnumerator RushHour ()
    {
        float speedChange = Random.Range(1f, 5f);
        _hungryCustomerMovement.SetMovementSpeed(speedChange);
        Debug.LogWarning("Rush Hour activated: speed at ");

        yield return new WaitForSeconds(activeTime);
        Debug.Log("Completed!");
        //_hungryCustomerMovement.SetDefaultSpeed();

    }

}
