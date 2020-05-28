using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager instance
    {
        get { return _instance; }
    }



    [Header("Gameplay UI")]
    [SerializeField] TextMeshProUGUI customersRemainingText;
    [SerializeField] RawImage customersRemainingIcon;
    [SerializeField] TextMeshProUGUI counterHealthText;
    [SerializeField] RawImage counterHealthIcon;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] RawImage moneyEarnedIcon;
    [SerializeField] TextMeshProUGUI countdownText;
    private float waveTextDelay = 1f;

    [Header("Main Menu")]
    [SerializeField] RawImage mainMenuImage;
    [SerializeField] RawImage feedTheHordeText;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button playButton;
    [SerializeField] Button optionsButton;

    [Header("Main Menu Options")]
    [SerializeField] RawImage o_volumeText;
    [SerializeField] RawImage o_overallVolumeText;
    [SerializeField] Slider o_overallSlider;
    [SerializeField] RawImage o_musicVolumeText;
    [SerializeField] Slider o_musicSlider;
    [SerializeField] RawImage o_soundFxText;
    [SerializeField] Slider o_soundFxSlider;
    [SerializeField] Button o_menuArrow;

    [Header("Pause Menu")]
    [SerializeField] RawImage pauseMenuBackground;
    [SerializeField] RawImage p_volumeText;
    [SerializeField] RawImage p_overallVolumeText;
    [SerializeField] Slider p_overallSlider;
    [SerializeField] RawImage p_musicVolumeText;
    [SerializeField] Slider p_musicSlider;
    [SerializeField] RawImage p_soundFxText;
    [SerializeField] Slider p_soundFxSlider;
    [SerializeField] Button resumeText;

    [Header("How To Play")]
    [SerializeField] RawImage howToPlayImage;
    [SerializeField] Button mainMenuArrow;

    [Header("GameOver")]
    [SerializeField] RawImage gameOverImage;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI customersFedText;

    [Header("Delivery System Icons")]
    [SerializeField] List<GameObject> deliveryIcons = new List<GameObject>();

    [Header("Delivery System UI")]
    private float iconXPos = 360f;
    private float iconYPos = -180f;
    private Vector2 deliveryIconNotification;

    [Header("Dev Console")]
    [SerializeField] InputField devConsole;
    [SerializeField] GameObject consoleInputText;

    private bool firstTimePaused = true;
    private bool gameJustStarted = true;

    public float OverallSliderVal { get; set; } = 1f;
    public float MusicSliderVal { get; set; } = 1f;
    public float SfxSliderVal { get; set; } = 1f;
    public bool ConsoleOpen { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            _instance = this;
        }
        deliveryIconNotification = new Vector2(iconXPos, iconYPos);
    }

    #region Screen UI Menus/Screens
    //Main Menu
    public void SetMainMenuActive(bool b)
    {
        //Dispaly main menu screen
        mainMenuImage.gameObject.SetActive(b);
        feedTheHordeText.gameObject.SetActive(b);
        playButton.gameObject.SetActive(b);
        howToPlayButton.gameObject.SetActive(b);
        optionsButton.gameObject.SetActive(b);
        if (b && gameJustStarted)
        {
            AudioManager.instance.PlaySound(AudioManager.SoundType.MENU_MUSIC);
            gameJustStarted = false;
        }

    }

    //Sets main menu options screen
    public void SetMainMenuOptionsActive(bool b)
    {
        mainMenuImage.gameObject.SetActive(b);
        o_volumeText.gameObject.SetActive(b);
        o_overallVolumeText.gameObject.SetActive(b);
        o_overallSlider.gameObject.SetActive(b);
        o_musicVolumeText.gameObject.SetActive(b);
        o_musicSlider.gameObject.SetActive(b);
        o_soundFxText.gameObject.SetActive(b);
        o_soundFxSlider.gameObject.SetActive(b);
        o_menuArrow.gameObject.SetActive(b);
    }

    //Sets pause menu
    public void SetPauseMenuActive(bool b)
    {
        pauseMenuBackground.gameObject.SetActive(b);
        p_volumeText.gameObject.SetActive(b);
        p_overallVolumeText.gameObject.SetActive(b);
        p_overallSlider.gameObject.SetActive(b);
        p_musicVolumeText.gameObject.SetActive(b);
        p_musicSlider.gameObject.SetActive(b);
        p_soundFxText.gameObject.SetActive(b);
        p_soundFxSlider.gameObject.SetActive(b);

        if (firstTimePaused)
        {
            SetSliderValues();
            firstTimePaused = false;
        }

        //other stuff.
        resumeText.gameObject.SetActive(b);
    }

    //Set Game Over Ui
    public void GameOverActive(bool b)
    {
        gameOverImage.gameObject.SetActive(b);
        customersFedText.gameObject.SetActive(b);
        dayCountText.gameObject.SetActive(b);
        restartButton.gameObject.SetActive(b);

        SetMainUIActive(false);

        customersFedText.text = GameManager.instance.CustomersFed.ToString();
       // GameManager.instance.DayCount--; //TODO This causes the game stat to decrease on game over screen when pause buttons are pressed.
        dayCountText.text = GameManager.instance.DayCount.ToString();
    }

    //Menu Icon Animation.
    private void OnPointerHover(PointerEventData pointerHoverEvent)
    {
        if (pointerHoverEvent.hovered.Contains(GameObject.Find("Play")))
        {
            playButton.animator.SetTrigger("Highlighted");
        }
    }

    //Set Howtoplay UI
    public void SetHowToPlayActive(bool b)
    {
        //display how to play screen. 
        howToPlayImage.gameObject.SetActive(b);
        mainMenuArrow.gameObject.SetActive(b);
    }


    //main game UI
    public void SetMainUIActive(bool b)
    {
        customersRemainingIcon.gameObject.SetActive(b);
        customersRemainingText.gameObject.SetActive(b);
        counterHealthIcon.gameObject.SetActive(b);
        counterHealthText.gameObject.SetActive(b);
        moneyEarnedIcon.gameObject.SetActive(b);
        moneyText.gameObject.SetActive(b);


    }
    #endregion
    #region UI Functionality
    //Wave Countdown Timer
    //Counts down the counter in between waves. 
    public IEnumerator WaveCountdownTimer()
    {
        countdownText.gameObject.SetActive(true);
        GameManager.instance.DayCount++;
        countdownText.text = "day " + GameManager.instance.DayCount;
        yield return new WaitForSeconds(waveTextDelay);
        countdownText.text = "ready?";
        yield return new WaitForSeconds(waveTextDelay);
        countdownText.text = "3";
        yield return new WaitForSeconds(waveTextDelay);
        countdownText.text = "2";
        yield return new WaitForSeconds(waveTextDelay);
        countdownText.text = "1";
        yield return new WaitForSeconds(waveTextDelay);
        countdownText.text = "go!";
        yield return new WaitForSeconds(waveTextDelay);
        countdownText.gameObject.SetActive(false);

        SpawnManager.instance.SpawnWave();
    }

    //Get slider values
    public void SetSliderValues ()
    {
        p_overallSlider.value = OverallSliderVal;
        p_musicSlider.value = MusicSliderVal;
        p_soundFxSlider.value = SfxSliderVal;
    }
    //Updates the wave counter on the ui.
    public void UpdateWaveCounter()
    {
        customersRemainingText.text = GameManager.instance.ActiveCustomers.ToString();
    }

    // updates the health of the counter on the ui. 
    public void UpdateCounterHealth(int health)
    {
        counterHealthText.text = health.ToString();
    }
    //Updates the money on screen
    public void UpdateMoneyCount()
    {
        moneyText.text = "" + (float)Math.Round(GameManager.instance.Money, 2);
    }
    //Shows the delivery icons on screen
    public void ShowDeliveryIcon(string iconName)
    {
        GameObject icon = deliveryIcons.Find(x => x.name == iconName);
        icon.SetActive(true);
        icon.transform.position = deliveryIconNotification;
    }

    //hides delivery icon when delivery is completed
    public void HideDeliveryIcon()
    {
        foreach (GameObject icon in deliveryIcons)
        {
            icon.SetActive(false);
        }
    }

    #endregion
    public void OpenDevConsole(bool b)
    {
        devConsole.gameObject.SetActive(b);
        if (ConsoleOpen == true)
        {
            DevConsole();
        }
        else
        {
            return;
        }
    }

    public void DevConsole()
    {
        string cmd = devConsole.text;
        
        switch (cmd)
        {
            case "slices":
                DeliverySystem.instance.DoubleSlices();
                break;
            default:
                Debug.LogWarning("No command set for this string!");
                break;

        }

    }
}
