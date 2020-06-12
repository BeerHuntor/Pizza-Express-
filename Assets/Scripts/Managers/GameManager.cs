using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager instance
    {
        get { return _instance; }
    }
    public bool GameIsRunning { get; private set; }
    public int CustomersFed { get; set; }
    public int DayCount { get; set; }
    public int CounterHealth { get; set; } = 5;
    public int PizzaSlices { get; private set; }
    public bool Restarted { get; set; }
    public float PizzaCostToPlayer { get; private set; } = 3f;
    private float startingMoney = 50.00f;

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
            Debug.LogWarning($"Counter Health = {CounterHealth} & Money = {Money}");
            GameOver();
        }
        PauseMenu();
    }
    //Generic method to display debug messages without having to write everything each time. 
    public string DebugMessage()
    {
        return $"PizzaSlices {PizzaSlices}";
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            GameIsRunning = false;
        }
        else if (value == 1)
        {
            //Resumes the Game.
            Time.timeScale = 1;
            UIManager.instance.SetPauseMenuActive(false);
            UIManager.instance.SetMainUIActive(true);
            GameIsRunning = true;

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
            }
            else
            {
                PauseGame(1);
            }
        }
    }

    //Closes the game on quit button.
    public void QuitGame()
    {
        Application.Quit();
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
