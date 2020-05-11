using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public SpawnManager _spawnManager;
    public PlayerMovement _playerMovement;

    [Header("Canvas GUI")]
    //TODO canvas UI.
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI counterHealthText;
    public TextMeshProUGUI moneyEarned;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI customersFedText;
    public RawImage gameOverImage;
    public RawImage mainMenuImage;
    public Button restartButton;
    private Button playButton;

    [SerializeField] int customersFed;
    [SerializeField] int money; // Money 
    [SerializeField] bool gameIsPaused;
    private float timeBetweenTextCountdown = 1f;


    private int activeCustomers; // Variable to calculate the active customers in the wave

    public bool gameIsRunning = false;
    private int pizzaSlices;
    public int counterHealth;
    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (counterHealth == 0 && gameIsRunning)
        {
            GameOver();
        }

        InGameStates();
    }

    //Counts down the counter in between waves. 
    public IEnumerator WaveCountdownTimer()
    {
        if (_spawnManager.DayCount == 0)
        {
            _spawnManager.DayCount = 1;
        }
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Day " + _spawnManager.DayCount;
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "Ready?";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "3";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "2";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "1";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(timeBetweenTextCountdown);
        countdownText.gameObject.SetActive(false);

        _spawnManager.SpawnWave();
        

    }

    void GameOver()
    {
        gameOverImage.gameObject.SetActive(true);
        customersFedText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        customersFedText.text = "Customers Fed: " + customersFed;

        restartButton.onClick.AddListener(GameReload);
        gameIsRunning = false;
        _spawnManager.WaveActive = false;
    }

    //gets called when the start button is clicked on the psuedo title screen. 
    public void StartGame()
    {
        gameIsRunning = true;
        ////Removes button and title screen. 
        playButton.gameObject.SetActive(false);
        mainMenuImage.gameObject.SetActive(false);


        pizzaSlices = 0;
        counterHealth = 5; //? changed this value for testing. 

        waveText.gameObject.SetActive(true);
        counterHealthText.gameObject.SetActive(true);
        moneyEarned.gameObject.SetActive(true);


        UpdateCounterHealth(counterHealth);
        UpdateWaveCounter();

        StartCoroutine(WaveCountdownTimer()); //? ETHANS CODE
        _playerMovement.allowMovement = true;

    }

    public void PauseGame()
    {
        Time.timeScale = 0; 
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; 
    }
  

    //Updates the wave counter on the ui.
    public void UpdateWaveCounter()
    {
        waveText.text = "Customers Remaining: " + activeCustomers;
    }

    // updates the health of the counter on the ui. 
    public void UpdateCounterHealth(int health)
    {
        counterHealthText.text = "Counter Health: " + health;
    }

    //Updates the customers. 
    public void UpdateCustomers(int customersRemaining)
    {
        activeCustomers = customersRemaining;
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

    public void GameReload()
    {
        SceneManager.LoadScene(0);
    }
    //For keypresses in the game. 
    public void InGameStates()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                ResumeGame();
                gameIsPaused = false;
                _playerMovement.allowMovement = true;
            }
            else
            {
                PauseGame();
                gameIsPaused = true;
                _playerMovement.allowMovement = false;
            }
        }
    }

    //? Debug method
    public void DisplayDataOnPress(string msg)
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Debug.Log(msg); 
        }
    }
}
