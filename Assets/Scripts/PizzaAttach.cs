using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PizzaAttach : MonoBehaviour
{
    private static PizzaAttach _instance; 

    public static PizzaAttach instance
    {
        get { return _instance; }
    }

    [SerializeField] GameObject pizza;

    private bool nextPizzaBuff; //allows the check to see if the bigger hands buff will activate on the next pizza. 

    public bool HasPizza { get; set; } //Used to check if the player has a pizza currently. 

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }

    void OnTriggerEnter(Collider other) {

        // check if the pizza is already parented to the player. If not then attach it and move
       
        if (other.CompareTag("Pizza") && !HasPizza) { //Check if the trigger is the pizza and the player doesnt already have a pizza

            other.transform.parent = gameObject.transform; //Setting the pizza to the player as a child
            other.transform.localPosition = new Vector3(0f, 0.5f, 0.3f);
            HasPizza = true;
            if (nextPizzaBuff)
            {
                DeliverySystem.instance.DoubleSlicesActive = true;
                GameManager.instance.SetPizzaSlices(12);
            }
            else
            {
                GameManager.instance.SetPizzaSlices(6);
            }
        }
    }

    //Public method to change the buff for the next pizza pickup. 
    public void SetNextPizzaBuff(bool buffNextTime)
    {
        nextPizzaBuff = buffNextTime;
    }
    
    //getter to reference next pizza buff. 
    public bool GetNextPizzaBuff()
    {
        return nextPizzaBuff;
    }

    

}
