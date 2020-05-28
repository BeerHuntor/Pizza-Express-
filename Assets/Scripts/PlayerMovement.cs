using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement _instance;

    public static PlayerMovement instance
    {
        get { return _instance; }
    }

    private Camera cam;
    private Plane groundPlane;

    private GameObject childPizza;
    private Vector3 pizzaPosition;

    [SerializeField] List<GameObject> reducedPizzas;

    public GameObject pizza;
    public GameObject pizzaSlice;
    private GameObject firingPoint;

    public Animator anim;

    private float leftScreenBounds = -14.5f; // Bounds of camera to the left the player can move to 

    [SerializeField] float movementSpeed;
    [SerializeField] float defaultSpeed;


    //private Transform personalSpace;

    public bool AllowMovement { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        //personalSpace = transform.Find("PersonalSpace");

        defaultSpeed = movementSpeed;
        anim = GetComponent<Animator>();
        cam = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        firingPoint = GameObject.Find("PizzaFiringPoint");



    }

    // Update is called once per frame
    void Update()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        // Player movement with the mouse cursor. 
        if (GameManager.instance.GameIsRunning)
        {
            float distance;
            //anim.SetBool("isRunning", true);

            //If mouse cursor is on player -- Stop moving idle anim = 0; 
            //If not over player then run anim. 

            if (groundPlane.Raycast(camRay, out distance))
            {

                Vector3 rayHitPoint = camRay.GetPoint(distance);
                if (!(transform.position.x < leftScreenBounds))
                {
                    if (!(rayHitPoint == transform.position))
                    {
                        //move player to cursor position
                        anim.SetInteger("moving", 1);
                        transform.position = Vector3.MoveTowards(transform.position, rayHitPoint, movementSpeed * Time.deltaTime);
                        transform.LookAt(rayHitPoint);
                    }
                    else
                    {
                        
                        anim.SetInteger("moving", 0);
                    }
                }
                else
                {
                    transform.position = new Vector3(leftScreenBounds, transform.position.y, transform.position.z);
                }
            }
        }

        //Checks if the player is holding a pizza, and if they are spawns the pizza slice on left mouse button from the firing point game objects position.
        if (Input.GetMouseButtonDown(0) && PizzaAttach.instance.HasPizza && GameManager.instance.GameIsRunning)
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


        GameManager.instance.RemovePizzaSlices();
        switch (GameManager.instance.PizzaSlices)
        {
            case 5:
                if (!DeliverySystem.instance.DoubleSlicesActive)
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
                if (DeliverySystem.instance.CurrentDelivery == "DOUBLE_SLICES" && DeliverySystem.instance.DoubleSlicesActive == true && GameManager.instance.PizzaSlices == 4)
                {
                    SpawnPizzaModel(reducedPizzas[3], pizzaPosition, reducedPizzas[3].transform.rotation, pizzaPosition);
                    break;
                }
                SpawnPizzaModel(reducedPizzas[1], pizzaPosition, reducedPizzas[1].transform.rotation, pizzaPosition); // 4 and 8 slices left

                break;
            case 3:
                if (!DeliverySystem.instance.DoubleSlicesActive)
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
                if (DeliverySystem.instance.CurrentDelivery == "DOUBLE_SLICES" && DeliverySystem.instance.DoubleSlicesActive == true)
                {
                    SpawnPizzaModel(reducedPizzas[4], pizzaPosition, reducedPizzas[4].transform.rotation, pizzaPosition);
                    break;
                }
                SpawnPizzaModel(reducedPizzas[3], pizzaPosition, reducedPizzas[3].transform.rotation, pizzaPosition); // 2 and 4 slices left

                break;
            case 1:
                if (!DeliverySystem.instance.DoubleSlicesActive)
                {
                    Destroy(childPizza);
                    SpawnPizzaModel(reducedPizzas[4], pizzaPosition, reducedPizzas[4].transform.rotation, pizzaPosition); //1 and 2 slices left
                    break;
                }
                break;
            case 0:
                PizzaAttach.instance.HasPizza = false;
                Destroy(childPizza);
                if (PizzaAttach.instance.GetNextPizzaBuff() == true && DeliverySystem.instance.DoubleSlicesActive == true)
                {
                    PizzaAttach.instance.SetNextPizzaBuff(false);
                    DeliverySystem.instance.DoubleSlicesActive = false;
                    UIManager.instance.HideDeliveryIcon();
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
