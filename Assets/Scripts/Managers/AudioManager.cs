using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    private float defaultMusicVolume = 0.1f;
    private float defaultSFXVolume = 0.8f;
    public static AudioManager instance
    {
        get { return _instance; }
    }

    private AudioSource sfxAudioSource;
    private AudioSource musicAudioSource; //There to solely handle the music everything else is to be handled via audio mgr

    [SerializeField] AudioMixer mixer;

    private AudioClip
        menuMusic,
        gameMusic,
        menuHover,
        menuClick,
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
        MENU_HOVER,
        MENU_CLICK,
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
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuMusic = Resources.Load<AudioClip>("Sounds/Main_Menu_Track");
        gameMusic = Resources.Load<AudioClip>("Sounds/Game_Play_Track");

        menuHover = Resources.Load<AudioClip>("Sounds/Menu_Highlight_SFX");
        menuClick = Resources.Load<AudioClip>("Sounds/Menu_Click_SFX");

        deliveryCrateSound = Resources.Load<AudioClip>("Sounds/Delivery_Crate_Sound");
        customerFedSound = Resources.Load<AudioClip>("Sounds/Eating");
        cashFlowVoiceOver = Resources.Load<AudioClip>("Sounds/Cash_Flow_Voice_Over");
        doubleSliceVoiceOver = Resources.Load<AudioClip>("Sounds/Double_Slices_Voice_Over");
        energizedVoiceOver = Resources.Load<AudioClip>("Sounds/Energized_Voice_Over");
        overtimeVoiceOver = Resources.Load<AudioClip>("Sounds/OverTime_Voice_Over");
        rushHourVoiceOver = Resources.Load<AudioClip>("Sounds/Rush_Hour_Voice_Over");
        timeIsDraggingVoiceOver = Resources.Load<AudioClip>("Sounds/Time_Is_Slow_Voice_Over");
        windFallVoiceOver = Resources.Load<AudioClip>("Sounds/Windfall_Voice_Over");
        //Used for SFX track
        sfxAudioSource = gameObject.GetComponent<AudioSource>();
        //Used for music track. 
        musicAudioSource = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }
    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.MENU_MUSIC:
                PlayAudioClip(menuMusic, musicAudioSource, false, defaultMusicVolume);
                break;
            case SoundType.GAME_MUSIC:
                PlayAudioClip(gameMusic, musicAudioSource, false, defaultMusicVolume);
                break;
            case SoundType.MENU_HOVER:
                PlayAudioClip(menuHover, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.MENU_CLICK:
                PlayAudioClip(menuClick, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.DELIVERY_CRATE:
                PlayAudioClip(deliveryCrateSound, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.CUSTOMER_FED:
                PlayAudioClip(customerFedSound, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.CASHFLOW:
                PlayAudioClip(cashFlowVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.DOUBLE_SLICES:
                PlayAudioClip(doubleSliceVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.ENERGIZED:
                PlayAudioClip(energizedVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.OVERTIME:
                PlayAudioClip(overtimeVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.RUSH_HOUR:
                PlayAudioClip(rushHourVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.TIME_IS_DRAGGING:
                PlayAudioClip(timeIsDraggingVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            case SoundType.WINDFALL:
                PlayAudioClip(windFallVoiceOver, sfxAudioSource, true, defaultSFXVolume);
                break;
            default:
                musicAudioSource.Stop();
                break;
        }
    }

    //Method call to play the given audio clip on the given audio source. 
    private void PlayAudioClip(AudioClip clipName, AudioSource sourceName, bool isOneShot, float volume = 1f)
    {
        sourceName.clip = clipName;
        sourceName.volume = volume;
        if (isOneShot)
        {
            sourceName.PlayOneShot(sourceName.clip);
        } else
        {
            sourceName.Play();
        }

    }

    public void StopMusicPlaying()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
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
        Debug.Log(lvl);
        mixer.SetFloat("SoundEffectsVolume", Mathf.Log10(lvl) * 60);
        if (lvl <= 0)
        {
            mixer.SetFloat("SoundEffectsVolume", -80f); //mutes

        }
    }

}
