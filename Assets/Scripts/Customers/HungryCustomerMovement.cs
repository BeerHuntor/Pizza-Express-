using System;
using UnityEditor.U2D;
using UnityEngine;

public class HungryCustomerMovement : MonoBehaviour
{

    private Transform player;
    public HungryCustomer customer;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] hungerSprites;
    private Light spotlight;

    //v0.5 EDITED HERE (Commented out the previous code, added awake method to instantiate a new object.)
    //private float defaultSpeed;
    //private float movementSpeed;
    private void Awake()
    {
        float speedRange = UnityEngine.Random.Range(0.5f, 2f);
        int hungerValue = UnityEngine.Random.Range(1, 4); //int is exclusive.
        customer = new HungryCustomer(speedRange, hungerValue);
        Debug.Log("Created a new customer");
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        //v0.5 EDITED HERE (Added code to access the spotlight color)
        spotlight = GetComponentInChildren<Light>();
        spotlight.color = customer.spotLightColor;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UpdateSprite();

        //v0.5 EDITED HERE (Commented out the previous code)
        //movementSpeed = 0.8f;
        //defaultSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameIsRunning)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, customer.MovementSpeed * Time.deltaTime);
            transform.LookAt(player);

        }
    }

    //checks for contact with the counter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Counter"))
        {
            GameManager.instance.CounterHealth--;
            UIManager.instance.UpdateCounterHealth(GameManager.instance.CounterHealth);
        }
    }
    public void SetDefaultSpeed()
    {
        //v0.5 EDITED HERE (Referenced the base class)
        customer.MovementSpeed = customer.DefaultSpeed;
    }

    public void IncreaseMovementSpeed(float speed)
    {
        //v0.5 EDITED HERE (referenced the base class.)
        customer.MovementSpeed += speed;
    }

    //v0.5 EDITED HERE (Added method to update the sprite from the pizza slice movement script. 
    public void UpdateSprite()
    {
        switch (customer.HungerValue)
        {
            case 1:
                spriteRenderer.sprite = hungerSprites[0];
                break;
            case 2:
                spriteRenderer.sprite = hungerSprites[1];
                break;
            case 3:
                spriteRenderer.sprite = hungerSprites[2];
                break;
            default:
                break;
        }
    }
}
