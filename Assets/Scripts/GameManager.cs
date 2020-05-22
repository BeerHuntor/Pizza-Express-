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
    private SpawnManager _spawnManager;
    private PlayerMovement _playerMovement;
    private DeliverySystem _deliverySystem;
    private UIManager _uiManager;

    public bool GameIsRunning { get; private set; }
    public int CustomersFed { get; set; }
    public int DayCount { get; set; }
    public int CounterHealth { get; set; }
    public int PizzaSlices { get; private set; }
    public float PizzaCostToPlayer { get; private set; } = 3f;
    #region MONEY
    //private float money; // Money 
    //public float Money
    //{
    //    get { return money; }
    //}
    //private float startingMoney;

    public float Money { get; private set; }
    #endregion
    public int ActiveCustomers { get; private set; }

    //TODO removed GameIsRunning substituted it for GameIsPausedProperty

    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();
        _uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        //playButton.onClick.AddListener(StartGame);
        _uiManager.SetMainMenuActive(true);
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
        _uiManager.GameOverActive(true);
        _uiManager.SetMainUIActive(false);

        GameIsRunning = false;
        _spawnManager.WaveActive = false;
    }

    //gets called when the start button is clicked on the psuedo title screen. 
    public void StartGame()
    {
        _uiManager.SetMainMenuActive(false);
        GameIsRunning = true;

        PizzaSlices = 0;
        CounterHealth = 55;
        Money = 50.00f; //TODO Find out why add money is being called twice at the start of the game. 

        _uiManager.SetMainUIActive(true);
        _uiManager.UpdateCounterHealth(CounterHealth);
        _uiManager.UpdateWaveCounter();

        AddMoney(Money);

        StartCoroutine(_uiManager.WaveCountdownTimer()); //? ETHANS CODE

        _playerMovement.AllowMovement = true;

    }
    //Shows the delivery icon on screen when collected delivery

    public void PauseGame(int value)
    {
        if (value == 0) 
        {
            //Pauses The Game
            Time.timeScale = 0;
            //GameIsRunning = false;
            //Debug.Log("game should be paused" + GameIsRunning);
        }
        else if (value == 1)
        {
            //Resumes the Game.
            Time.timeScale = 1;
            //GameIsRunning = true;
            //Debug.Log("Game should be unpaused" + GameIsRunning);
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
        if (_deliverySystem.CurrentDelivery != "OVERTIME")
        {
            ActiveCustomers = customersRemaining;
        }
        else
        {
            ActiveCustomers += customersRemaining;
        }
        _uiManager.UpdateWaveCounter();
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
        if (!_deliverySystem.HappyHourActive)
        {
            Money += moneyToAdd;
        } else
        {
            Money += (moneyToAdd * _deliverySystem.HappyHourMultiplier);
        }
        
        _uiManager.UpdateMoneyCount();
    }
    //Removes money from money count
    public void RemoveMoney (float moneyToRemove)
    {
        Money -= moneyToRemove;
        _uiManager.UpdateMoneyCount();
    }

    //Removes one from the active customer list when despawned, and checks if there is less than zero and if so spawns a new wave. 
    public void RemoveCustomer()
    {
        ActiveCustomers--; //Remove one customer everytime this gets called
        CustomersFed++;
        _uiManager.UpdateWaveCounter(); //Then we update the customer 'list'
        if (ActiveCustomers <= 0)
        {
            ActiveCustomers = 0; // Sets the active customers to zero if the customer count is below zero.
            _spawnManager.WaveActive = false;
            StartCoroutine(_uiManager.WaveCountdownTimer());
        }
    }
}
