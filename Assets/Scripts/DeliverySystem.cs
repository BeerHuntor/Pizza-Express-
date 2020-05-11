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
    

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _pizzaAttach = GameObject.Find("Player").GetComponent<PizzaAttach>();

        deliveries.Add("ENERGIZED"); //Speeds up the player randomly 
        deliveries.Add("BIGGER_HANDS"); // double pizza stack
        deliveries.Add("RUSH_HOUR"); // quicker customers
        deliveries.Add("TIME_IS_DRAGGING"); //slower customers / slower player / slower pizzas
        deliveries.Add("OVERTIME"); //more customers in this wave
        deliveries.Add("CASHFLOW"); //more money earned.
    }
    // Gets the current delivery at random from a list of set deliveries.
    public void GetDelivery ()
    {
        int deliveryIndex = Random.Range(1, 2);

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

    private void BiggerHands()
    {
        _pizzaAttach.SetNextPizzaBuff(true);

        Debug.Log("Next pizza buffed!");
        //do somethinbg
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
}
