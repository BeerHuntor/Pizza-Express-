using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PizzaAttach : MonoBehaviour
{
    private GameManager _gameManager;
    private DeliverySystem _deliverySystem;

    //public GameObject pizza;

    [SerializeField] bool nextPizzaBuff; //allows the check to see if the bigger hands buff will activate on the next pizza. 

    private bool hasPizza = false;
    public bool HasPizza
    {
        get { return hasPizza; }
        set { hasPizza = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();

    }

    void OnTriggerEnter(Collider other) {

        // check if the pizza is already parented to the player. If not then attach it and move
       
        if (other.CompareTag("Pizza") && !hasPizza) { //Check if the trigger is the pizza and the player doesnt already have a pizza

            other.transform.parent = gameObject.transform; //Setting the pizza to the player as a child
            other.transform.localPosition = new Vector3(0f, 0.5f, 0.3f);
            hasPizza = true;
            if (nextPizzaBuff)
            {
                _deliverySystem.SetBiggerHandsReadyToUse(true);
                _gameManager.SetPizzaSlices(12);
            }
            else
            {
                _gameManager.SetPizzaSlices(6);
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
