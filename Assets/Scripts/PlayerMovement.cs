using System.Collections.Generic;
using UnityEngine;

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
    //When the pizza slice gets fired, this removes the current model held and spawns a new model representing the slices remaining in its place. 
    public void RemoveSlices()
    {
        /*
         * Index 0 = 5 slices
         * Index 1 = 4 slices
         * Index 2 = 3 slices
         * Index 3 = 2 slices
         * Index 4 = 1 slice
         */
        GameManager.instance.RemovePizzaSlices();
        switch (GameManager.instance.PizzaSlices)
        {
            case 10:
                SpawnNewPizzaModel(0);
                break;
            case 5:
                if (!PizzaAttach.instance.UsedPizzaBuff)
                {
                    SpawnNewPizzaModel(0);
                    break;
                }
                break;
            case 8:
                SpawnNewPizzaModel(1);
                break;
            case 4:
                if (!PizzaAttach.instance.UsedPizzaBuff)
                {
                    SpawnNewPizzaModel(1);
                    break;
                }
                else
                {
                    SpawnNewPizzaModel(3);
                    Debug.Log(GameManager.instance.DebugMessage());
                    break;
                }
            case 6:
                SpawnNewPizzaModel(2);
                break;
            case 3:
                if (!PizzaAttach.instance.UsedPizzaBuff)
                {
                    SpawnNewPizzaModel(2);
                    break;
                }
                break;
            case 2:
                if (!PizzaAttach.instance.UsedPizzaBuff)
                {
                    SpawnNewPizzaModel(3);
                    break;
                }
                else
                {
                    SpawnNewPizzaModel(4);
                }
                break;
            case 1:
                SpawnNewPizzaModel(4);
                break;
            case 0:
                PizzaAttach.instance.HasPizza = false;
                Destroy(gameObject.transform.GetChild(3).gameObject);
                if (PizzaAttach.instance.UsedPizzaBuff)
                {
                    Debug.Log(GameManager.instance.DebugMessage());
                    PizzaAttach.instance.UsedPizzaBuff = false;
                    PizzaAttach.instance.SetNextPizzaBuff(false);
                    DeliverySystem.instance.DoubleSlicesActive = false;
                    UIManager.instance.HideDeliveryIcon();
                    break;
                }
                break;
            default:
                Debug.LogWarning("Couldn't find the correct pizza slice model");
                break;
        }

    }


    //Spawns the pizza models upon firing the pizza
    void SpawnNewPizzaModel(int modelNumber)
    {
        //Getting the current held pizzas gameobject
        childPizza = gameObject.transform.GetChild(3).gameObject;
        //Getting the current held pizzas position
        pizzaPosition = gameObject.transform.GetChild(3).gameObject.transform.position;

        Destroy(childPizza);
        GameObject newPizza = Instantiate(reducedPizzas[modelNumber], pizzaPosition, reducedPizzas[modelNumber].transform.rotation);

        newPizza.transform.parent = gameObject.transform;
        newPizza.transform.position = pizzaPosition;

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
