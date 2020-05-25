using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager instance
    {
        get { return _instance; }
    }

    private AudioSource audioSource;
    private AudioSource gameManagerAudioSource; //There to solely handle the music everything else is to be handled via audio mgr

    [SerializeField] AudioMixer mixer;

    private AudioClip
        menuMusic,
        gameMusic,
        deliveryCrateSound,
        customerFedSound,
        cashFlowVoiceOver,
        doubleSliceVoiceOver,
        energizedVoiceOver,
        overtimeVoiceOver,
        rushHourVoiceOver,
        timeIsDraggingVoiceOver,
        windFallVoiceOver;

    public enum SoundType
    {
        NONE,
        MENU_MUSIC,
        GAME_MUSIC,
        DELIVERY_CRATE,
        CUSTOMER_FED,
        CASHFLOW,
        DOUBLE_SLICES,
        ENERGIZED,
        OVERTIME,
        RUSH_HOUR,
        TIME_IS_DRAGGING,
        WINDFALL
    };

    public SoundType soundType { get; set; } = SoundType.NONE;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuMusic = Resources.Load<AudioClip>("Sounds/Main_Menu_Track");
        gameMusic = Resources.Load<AudioClip>("Sounds/Game_Play_Track");
        deliveryCrateSound = Resources.Load<AudioClip>("Sounds/Delivery_Crate_Sound");
        customerFedSound = Resources.Load<AudioClip>("Sounds/Eating");
        cashFlowVoiceOver = Resources.Load<AudioClip>("Sounds/Cash_Flow_Voice_Over");
        doubleSliceVoiceOver = Resources.Load<AudioClip>("Sounds/Double_Slices_Voice_Over");
        energizedVoiceOver = Resources.Load<AudioClip>("Sounds/Energized_Voice_Over");
        overtimeVoiceOver = Resources.Load<AudioClip>("Sounds/OverTime_Voice_Over");
        rushHourVoiceOver = Resources.Load<AudioClip>("Sounds/Rush_Hour_Voice_Over");
        timeIsDraggingVoiceOver = Resources.Load<AudioClip>("Sounds/Time_Is_Slow_Voice_Over");
        windFallVoiceOver = Resources.Load<AudioClip>("Sounds/Windfall_Voice_Over");

        audioSource = gameObject.GetComponent<AudioSource>();
        gameManagerAudioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }
    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.MENU_MUSIC:
                gameManagerAudioSource.clip = menuMusic;
                gameManagerAudioSource.Play();
                break;
            case SoundType.GAME_MUSIC:
                gameManagerAudioSource.clip = gameMusic;
                gameManagerAudioSource.Play();
                break;
            case SoundType.DELIVERY_CRATE:
                audioSource.clip = deliveryCrateSound;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.CUSTOMER_FED:
                audioSource.clip = customerFedSound;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.CASHFLOW:
                audioSource.clip = cashFlowVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.DOUBLE_SLICES:
                audioSource.clip = doubleSliceVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.ENERGIZED:
                audioSource.clip = energizedVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.OVERTIME:
                audioSource.clip = overtimeVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.RUSH_HOUR:
                audioSource.clip = rushHourVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.TIME_IS_DRAGGING:
                audioSource.clip = timeIsDraggingVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            case SoundType.WINDFALL:
                audioSource.clip = windFallVoiceOver;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            default:
                gameManagerAudioSource.Stop();
                break;
        }
    }

    public void StopMusicPlaying()
    {
        if (gameManagerAudioSource.isPlaying)
        {
            gameManagerAudioSource.Stop();
        }
        else
        {
            Debug.LogWarning("Your trying to turn off music that is not playing!");
        }
    }

    //Sets volume level everything
    public void SetOverallVolumeLevel (float lvl)
    {
        UIManager.instance.OverallSliderVal = lvl;
        mixer.SetFloat("MasterVolume", Mathf.Log10(lvl) * 60);
        if (lvl <= 0)
        {
            //Effectivly mutes the volume.
            mixer.SetFloat("MasterVolume", -80f);

        }
    }
    //Sets music independant level
    public void SetMusicVolumeLevel(float lvl)
    {
        UIManager.instance.MusicSliderVal = lvl;
        mixer.SetFloat("MusicVolume", Mathf.Log10(lvl) * 60);
        if (lvl <= 0)
        {
            mixer.SetFloat("MusicVolume", -80f); //mutes
        }
    }

    //Sets independant soundfx volume.
    public void SetSoundFxVolumeLevel(float lvl)
    {
        UIManager.instance.SfxSliderVal = lvl;
        mixer.SetFloat("SoundEffectsVolume", Mathf.Log10(lvl) * 60);
        if (lvl <= 0)
        {
            mixer.SetFloat("SoundEffectsVolume", -80f); //mutes

        }
    }

}
