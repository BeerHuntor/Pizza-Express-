using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private SpawnManager _spawnManager;

    [Header("Gameplay UI")]
    [SerializeField] TextMeshProUGUI customersRemainingText;
    [SerializeField] RawImage customersRemainingIcon;
    [SerializeField] TextMeshProUGUI counterHealthText;
    [SerializeField] RawImage counterHealthIcon;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] RawImage moneyEarnedIcon;
    [SerializeField] TextMeshProUGUI countdownText;
    private float waveTextDelay = 1f;

    [Header("Game States UI")]
    [SerializeField] RawImage mainMenuImage;
    [SerializeField] RawImage feedTheHordeText;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Button playButton;

    [Header("How To Play")]
    [SerializeField] RawImage howToPlayImage;
    [SerializeField] Button mainMenuArrow;

    [Header("GameOver")]
    [SerializeField] RawImage gameOverImage;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI customersFedText;

    [Header("DeliverySystemIcons")]
    [SerializeField] List<GameObject> deliveryIcons = new List<GameObject>();

    [Header("DeliverySystemUI")]
    private float iconXPos = 360f;
    private float iconYPos = -180f;
    private Vector2 deliveryIconNotification;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
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
    }
    //Set Game Over Ui
    public void GameOverActive(bool b)
    {
        gameOverImage.gameObject.SetActive(b);
        customersFedText.gameObject.SetActive(b);
        dayCountText.gameObject.SetActive(b);
        restartButton.gameObject.SetActive(b);

        SetMainUIActive(false);

        customersFedText.text = _gameManager.CustomersFed.ToString();
        dayCountText.text = _gameManager.DayCount.ToString();
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
        if (_gameManager.DayCount == 0)
        {
            _gameManager.DayCount = 1;
        }
        countdownText.gameObject.SetActive(true);
        countdownText.text = "day " + _gameManager.DayCount;
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

        _spawnManager.SpawnWave();
    }


    //Updates the wave counter on the ui.
    public void UpdateWaveCounter()
    {
        customersRemainingText.text = _gameManager.ActiveCustomers.ToString();
    }

    // updates the health of the counter on the ui. 
    public void UpdateCounterHealth(int health)
    {
        counterHealthText.text = health.ToString();
    }
    //Updates the money on screen
    public void UpdateMoneyCount()
    {
        moneyText.text = "" + (float)Math.Round(_gameManager.Money, 2);
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
}
