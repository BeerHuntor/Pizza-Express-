using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine.EventSystems;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager instance
    {
        get { return _instance; }
    }
    private float startingMoney = 50.00f;
    public bool GameIsRunning { get; private set; }
    public int CustomersFed { get; set; }
    public int DayCount { get; set; }
    public int CounterHealth { get; set; }
    public int PizzaSlices { get; private set; }
    public bool Restarted { get; set; }
    public float PizzaCostToPlayer { get; private set; } = 3f;

    public float Money { get; private set; }
    public int ActiveCustomers { get; private set; }

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

        UIManager.instance.SetMainMenuActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((CounterHealth == 0 || Money <= 0) && GameIsRunning)
        {
            GameOver();
        }

        PauseMenu();
    }

    //Calls game over. 
    void GameOver()
    {
        UIManager.instance.GameOverActive(true);
        UIManager.instance.SetMainUIActive(false);

        GameIsRunning = false;
        SpawnManager.instance.WaveActive = false;
    }

    //gets called when the start button is clicked on the psuedo title screen. 
    public void StartGame()
    {
        UIManager.instance.SetMainMenuActive(false);
        AudioManager.instance.StopMusicPlaying();
        AudioManager.instance.PlaySound(AudioManager.SoundType.GAME_MUSIC);
        GameIsRunning = true;

        PizzaSlices = 0;
        CounterHealth = 5;

        UIManager.instance.SetMainUIActive(true);
        UIManager.instance.UpdateCounterHealth(CounterHealth);
        UIManager.instance.UpdateWaveCounter();

        AddMoney(startingMoney);

        StartCoroutine(UIManager.instance.WaveCountdownTimer()); //? ETHANS CODE

        PlayerMovement.instance.AllowMovement = true;

    }
    //Shows the delivery icon on screen when collected delivery

    public void PauseGame(int value)
    {
        if (value == 0) 
        {
            //pauses the game
            Time.timeScale = 0;
            UIManager.instance.SetMainUIActive(false);
            UIManager.instance.SetPauseMenuActive(true);
            
        }
        else if (value == 1)
        {
            //Resumes the Game.
            Time.timeScale = 1;
            UIManager.instance.SetPauseMenuActive(false);
            UIManager.instance.SetMainUIActive(true);

        }
        else
        {
            Debug.LogWarning("You tried to pass in a number other than 0 or 1 to pause!");
        }
    }


    //PauseMenu
    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsRunning)
            {
                PauseGame(0);
                GameIsRunning = false;
            }
            else
            {
                PauseGame(1);
                GameIsRunning = true;
            }
        }
    }

    //Updates the customers. 
    public void UpdateCustomers(int customersRemaining)
    {
        if (DeliverySystem.instance.CurrentDelivery != "OVERTIME")
        {
            ActiveCustomers = customersRemaining;
        }
        else
        {
            ActiveCustomers += customersRemaining;
        }
        UIManager.instance.UpdateWaveCounter();
    }

    //removes the pizza slices when fired. 
    public void RemovePizzaSlices()
    {
        PizzaSlices--;
    }

    //Sets the pizza slices when picking up a pizza
    public void SetPizzaSlices(int number)
    {
        PizzaSlices = number;
    }

    //Adds money to money count
    public void AddMoney(float moneyToAdd)
    {
        if (!DeliverySystem.instance.HappyHourActive)
        {
            Money += moneyToAdd; 
        } else
        {
            Money += (moneyToAdd * DeliverySystem.instance.HappyHourMultiplier);
        }
        
        UIManager.instance.UpdateMoneyCount();
    }
    //Removes money from money count
    public void RemoveMoney (float moneyToRemove)
    {
        Money -= moneyToRemove;
        UIManager.instance.UpdateMoneyCount();
    }

    //Removes one from the active customer list when despawned, and checks if there is less than zero and if so spawns a new wave. 
    public void RemoveCustomer()
    {
        ActiveCustomers--; //Remove one customer everytime this gets called
        CustomersFed++;
        UIManager.instance.UpdateWaveCounter(); //Then we update the customer 'list'
        if (ActiveCustomers <= 0)
        {
            ActiveCustomers = 0; // Sets the active customers to zero if the customer count is below zero.
            SpawnManager.instance.WaveActive = false;
            StartCoroutine(UIManager.instance.WaveCountdownTimer());
        }
    }
}
