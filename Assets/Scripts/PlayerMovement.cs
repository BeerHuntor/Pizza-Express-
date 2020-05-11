using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private PizzaAttach _pizzaAttach;
    private GameManager _gameManager;
    private DeliverySystem _deliverySystem;

    private Camera cam;
    private Plane groundPlane;

    private GameObject childPizza;
    private Vector3 pizzaPosition;

    public List<GameObject> reducedPizzas;

    public GameObject pizza;
    public GameObject pizzaSlice;
    private GameObject firingPoint;

    public Animator runAnim;

    private float leftScreenBounds = -14.5f; // Bounds of camera to the left the player can move to 

    [SerializeField] float movementSpeed;
    [SerializeField] float defaultSpeed;

    public bool allowMovement;

    // Start is called before the first frame update
    void Start()
    {
        _pizzaAttach = GetComponent<PizzaAttach>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();

        defaultSpeed = movementSpeed;
        runAnim = GetComponent<Animator>();
        cam = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        firingPoint = GameObject.Find("PizzaFiringPoint");



    }

    // Update is called once per frame
    void Update()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        // Player movement with the mouse cursor. 
        if (allowMovement && _gameManager.gameIsRunning)
        {
            float distance;
            runAnim.SetBool("isRunning", true);

            if (groundPlane.Raycast(camRay, out distance))
            {

                Vector3 rayHitPoint = camRay.GetPoint(distance);
                if (!(transform.position.x < leftScreenBounds))
                {
                    //move player to cursor position
                    transform.position = Vector3.MoveTowards(transform.position, rayHitPoint, movementSpeed * Time.deltaTime);
                    transform.LookAt(rayHitPoint);
                }
                else
                {
                    transform.position = new Vector3(leftScreenBounds, transform.position.y, transform.position.z);
                }
            }
        }

        //Checks if the player is holding a pizza, and if they are spawns the pizza slice on left mouse button from the firing point game objects position.
        if (Input.GetMouseButtonDown(0) && _pizzaAttach.hasPizza && _gameManager.gameIsRunning)
        {

            Instantiate(pizzaSlice, firingPoint.transform.position, transform.rotation);
            RemoveSlices();
        }
    }

    // When pizza slice is fired -- spawns the slice equivelant in its place. 
    //TODO: Re Write this code to be pretty -- PLEASE?
    public void RemoveSlices()
    {

        //Getting the current held pizzas position
        pizzaPosition = gameObject.transform.GetChild(3).gameObject.transform.position;

        //Getting the current held pizzas gameobject
        childPizza = gameObject.transform.GetChild(3).gameObject;


        _gameManager.RemovePizzaSlices();
        switch (_gameManager.GetPizzaSlices())
        {
            case 5:
                if(!_deliverySystem.GetBiggerHandsReadyToUse())
                {
                    Destroy(childPizza);
                    SpawnPizzaModel(reducedPizzas[0], pizzaPosition, reducedPizzas[0].transform.rotation, pizzaPosition); //5 and 10 slices left
                    break;
                }
                break;
            case 10://10
                Destroy(childPizza);
                SpawnPizzaModel(reducedPizzas[0], pizzaPosition, reducedPizzas[0].transform.rotation, pizzaPosition); //5 and 10 slices left
                break;
            case 4:
            case 8://8
                Destroy(childPizza);
                if (_deliverySystem.GetCurrentDelivery() == "BIGGER_HANDS" && _deliverySystem.GetBiggerHandsReadyToUse() == true && _gameManager.GetPizzaSlices() == 4)
                {
                    SpawnPizzaModel(reducedPizzas[3], pizzaPosition, reducedPizzas[3].transform.rotation, pizzaPosition); 
                    break;
                }
                SpawnPizzaModel(reducedPizzas[1], pizzaPosition, reducedPizzas[1].transform.rotation, pizzaPosition); // 4 and 8 slices left

                break;
            case 3:
                if(!_deliverySystem.GetBiggerHandsReadyToUse())
                {
                    Destroy(childPizza);
                    SpawnPizzaModel(reducedPizzas[2], pizzaPosition, reducedPizzas[2].transform.rotation, pizzaPosition); // 3 and 6 slices left
                    break;
                }
                break;
            case 6://6
                Destroy(childPizza);
                SpawnPizzaModel(reducedPizzas[2], pizzaPosition, reducedPizzas[2].transform.rotation, pizzaPosition); // 3 and 6 slices left

                break;
            case 2:
                //case 4: 
                Destroy(childPizza);
                if (_deliverySystem.GetCurrentDelivery() == "BIGGER_HANDS" && _deliverySystem.GetBiggerHandsReadyToUse() == true)
                {
                    SpawnPizzaModel(reducedPizzas[4], pizzaPosition, reducedPizzas[4].transform.rotation, pizzaPosition);
                    break;
                }
                SpawnPizzaModel(reducedPizzas[3], pizzaPosition, reducedPizzas[3].transform.rotation, pizzaPosition); // 2 and 4 slices left

                break;
            case 1:
                if (!_deliverySystem.GetBiggerHandsReadyToUse())
                {
                    Destroy(childPizza);
                    SpawnPizzaModel(reducedPizzas[4], pizzaPosition, reducedPizzas[4].transform.rotation, pizzaPosition); //1 and 2 slices left
                    break;
                }
                break;
            case 0:
                _pizzaAttach.hasPizza = false;
                Destroy(childPizza);
                if (_pizzaAttach.GetNextPizzaBuff() == true && _deliverySystem.GetBiggerHandsReadyToUse() == true)
                {
                    _pizzaAttach.SetNextPizzaBuff(false);
                    _deliverySystem.SetCrateActive(false);
                    _deliverySystem.SetBiggerHandsReadyToUse(false);
                    break;
                }
                break;
            default:
                break;
        }
    }

    //Spawns the pizza models upon firing the pizza
    void SpawnPizzaModel(GameObject go, Vector3 loc, Quaternion rot, Vector3 spawnPizzaLoc)
    {
        GameObject newPizza = Instantiate(go, loc, rot);

        newPizza.transform.parent = gameObject.transform;
        newPizza.transform.position = spawnPizzaLoc;

    }

    //public int GetPizzaModel()
    //{
    //    int modelIndex = 0;
    //    switch (_gameManager.pizzaSlices)
    //    {
    //        //0 1 2 3 4 5 6 8 10
    //        case 5:
    //            modelIndex = 0;
    //            break;
    //        case 4:
    //            modelIndex = 1;
    //            break;
    //        case 3:
    //            modelIndex = 2;
    //            break;
    //        case 2:
    //            modelIndex = 3;
    //            break;
    //        case 1:
    //            modelIndex = 4;
    //            break;
    //        case 0:
    //            _pizzaAttach.hasPizza = false;
    //            Debug.Log("Has pizza?: " + _pizzaAttach.hasPizza);
    //            break;
    //        default:
    //            break;
    //    }
    //    return modelIndex;
    //}
    //Changes the players movement speed.
    public void ChangeMovementSpeed(float speed)
    {
        movementSpeed += speed;
    }

    //Reverts the players movement speed back to the default setting. 
    public void SetDefaultSpeed()
    {
        movementSpeed = defaultSpeed;
    }
}
