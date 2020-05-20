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

    [Header("Gameplay UI")]
    [SerializeField] TextMeshProUGUI customersRemainingText;
    [SerializeField] TextMeshProUGUI counterHealthText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] RawImage customersRemainingIcon;
    [SerializeField] RawImage counterHealthIcon;
    [SerializeField] RawImage moneyEarnedIcon;
    private float timeBetweenTextCountdown = 1f;

    [Header("Game States UI")]
    [SerializeField] RawImage mainMenuImage;
    [SerializeField] RawImage feedTheHordeText;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button playButton;

    [Header("HowToPlay")]
    [SerializeField] RawImage howToPlayImage;
    [SerializeField] Button mainMenuArrow;

    [Header("GameOver")]
    [SerializeField] RawImage gameOverImage;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI customersFedText;
    

    [Header("DeliverySystemIcons")]
    [SerializeField] List<GameObject> deliveryIcons = new List<GameObject>();


    private float iconXPos = 360f;
    private float iconYPos = -180f;
    private Vector2 deliveryIconNotification;

    private bool gameIsPaused;
    private int customersFed;

    #region MONEY
    private float money; // Money 
    public float Money
    {
        get { return money; }
    }
    private float startingMoney;
    #endregion

    private int activeCustomers; // Variable to calculate the active customers in the wave

    private bool gameIsRunning = false;
    public bool GameIsRunning
    {
        get { return gameIsRunning; }
    }
    private int pizzaSlices;
    private int counterHealth;
    public int CounterHealth
    {
        get { return counterHealth; }
        set { counterHealth = value; }
    }
    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        _deliverySystem = GameObject.Find("GameManager").GetComponent<DeliverySystem>();

        deliveryIconNotification = new Vector2(iconXPos, iconYPos);
        //playButton.onClick.AddListener(StartGame);
        SetMainMenuActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((counterHealth == 0 || money <= 0) && gameIsRunning)
        {
            GameOver();
        }

        PauseMenu();
    }

    //Calls game over. 
    void GameOver()
    {
        gameOverImage.gameObject.SetActive(true);
        customersFedText.gameObject.SetActive(true);
        dayCountText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        SetMainUIActive(false);

        customersFedText.text = customersFed.ToString();
        dayCountText.text = _spawnManager.DayCount.ToString();

        //restartButton.onClick.AddListener(GameReload);
        gameIsRunning = false;
        _spawnManager.WaveActive = false;
    }

    //gets called when the start button is clicked on the psuedo title screen. 
    public void StartGame()
    {
        gameIsRunning = true;

        SetMainMenuActive(false);

        pizzaSlices = 0;
        counterHealth = 5;
        startingMoney = 50.00f;

        SetMainUIActive(true);

        UpdateCounterHealth(counterHealth);
        UpdateWaveCounter();
        AddMoney(startingMoney);

        StartCoroutine(WaveCountdownTimer()); //? ETHANS CODE
        _playerMovement.AllowMovement = true;

    }
    //Shows the delivery icon on screen when collected delivery

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    //public void GameReload()
    //{
    //    SceneManager.LoadScene(0);
    //}


    //PauseMenu
    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                ResumeGame();
                gameIsPaused = false;
                _playerMovement.AllowMovement = true;
            }
            else
            {
                PauseGame();
                gameIsPaused = true;
                _playerMovement.AllowMovement = false;
            }
        }
    }
    public void SetMainMenuActive(bool b)
    {
        //Dispaly main menu screen
        mainMenuImage.gameObject.SetActive(b);
        feedTheHordeText.gameObject.SetActive(b);
        playButton.gameObject.SetActive(b);
        howToPlayButton.gameObject.SetActive(b);
    }

    private void SetMainUIActive (bool b)
    {
        customersRemainingIcon.gameObject.SetActive(b);
        customersRemainingText.gameObject.SetActive(b);
        counterHealthIcon.gameObject.SetActive(b);
        counterHealthText.gameObject.SetActive(b);
        moneyEarnedIcon.gameObject.SetActive(b);
        moneyText.gameObject.SetActive(b);
    }
    public void SetHowToPlayActive(bool b)
    {
        //display how to play screen. 
        howToPlayImage.gameObject.SetActive(b);
        mainMenuArrow.gameObject.SetActive(b);
    }
    private void OnPointerHover(PointerEventData pointerHoverEvent)
    {
        if (pointerHoverEvent.hovered.Contains(GameObject.Find("Play"))){
            playButton.animator.SetTrigger("Highlighted");
        }
    }

    //Counts down the counter in between waves. 
    public IEnumerator WaveCountdownTimer()
    {
        if (_spawnManager.DayCount == 0)
        {
            _spawnManager.DayCount = 1;
        }
        countdownText.gameObject.SetActive(true);
        countdownText.text = "day " + _spawnManager.DayCount;
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "ready?";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "3";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "2";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "1";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "go!";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.gameObject.SetActive(false);

        _spawnManager.SpawnWave();


    }

    public void ShowDeliveryIcon(string iconName)
    {
        GameObject icon =  deliveryIcons.Find(x => x.name == iconName);
        icon.SetActive(true);
        icon.transform.position = deliveryIconNotification;
    }
    //hides delivery icon when delivery is completed. 
    public void HideDeliveryIcon()
    {
        foreach (GameObject icon in deliveryIcons)
        {
            icon.SetActive(false);
        }
    }
    //Updates the wave counter on the ui.
    public void UpdateWaveCounter()
    {
        customersRemainingText.text = activeCustomers.ToString();
    }

    // updates the health of the counter on the ui. 
    public void UpdateCounterHealth(int health)
    {
        counterHealthText.text =  health.ToString();
    }

    //Updates the customers. 
    public void UpdateCustomers(int customersRemaining)
    {
        if (_deliverySystem.GetCurrentDelivery() != "OVERTIME")
        {
            activeCustomers = customersRemaining;
        }
        else
        {
            activeCustomers += customersRemaining;
        }
        UpdateWaveCounter();
    }

    public void RemovePizzaSlices()
    {
        pizzaSlices--;
    }

    public void SetPizzaSlices(int number)
    {
        pizzaSlices = number;
    }

    public int GetPizzaSlices()
    {
        return pizzaSlices;
    }
    public void AddMoney(float moneyToAdd)
    {
        if (!_deliverySystem.HappyHourActive)
        {
            money += moneyToAdd;
        } else
        {
            money += (moneyToAdd * _deliverySystem.HappyHourMultiplier);
        }
        
        UpdateMoneyCount();
    }
    public void RemoveMoney (float moneyToRemove)
    {
        money -= moneyToRemove;
        UpdateMoneyCount();
    }

    private void UpdateMoneyCount()
    {
        moneyText.text = "" + (float)Math.Round(money, 2) ;
    }
    //Removes one from the active customer list when despawned, and checks if there is less than zero and if so spawns a new wave. 
    public void RemoveCustomer()
    {
        activeCustomers--; //Remove one customer everytime this gets called
        customersFed++;
        UpdateWaveCounter(); //Then we update the customer 'list'
        if (activeCustomers <= 0)
        {
            activeCustomers = 0; // Sets the active customers to zero if the customer count is below zero.
            _spawnManager.WaveActive = false;
            StartCoroutine(WaveCountdownTimer());
        }
    }
}
