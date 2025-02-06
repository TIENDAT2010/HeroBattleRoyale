using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source References")]
    [SerializeField] private AudioSource soundSource = null;
    [SerializeField] private AudioSource musicSource = null;


    [Header("Audio Clips References")]
    [SerializeField] public AudioClip tick = null;
    [SerializeField] public AudioClip button = null;
    [SerializeField] public AudioClip addCoin = null;
    [SerializeField] public AudioClip spawnHero = null;
    [SerializeField] public AudioClip heroDead = null;
    [SerializeField] public AudioClip heroUnlock = null;
    [SerializeField] public AudioClip heroUpgrade = null;
    [SerializeField] public AudioClip towerDestroyed = null;
    [SerializeField] public AudioClip levelFailed = null;
    [SerializeField] public AudioClip levelCompleted = null;

    [Header("Archer Tower")]
    [SerializeField] public AudioClip archerTowerAttack = null;
    [SerializeField] public AudioClip archerArrowExplode = null;

    [Header("Magic Tower")]
    [SerializeField] public AudioClip magicTowerAttack = null;
    [SerializeField] public AudioClip magicBallExplode = null;

    [Header("Sword Hero")]
    [SerializeField] public AudioClip swordHeroAttack = null;

    [Header("Blade Hero")]
    [SerializeField] public AudioClip bladeHeroAttack = null;

    [Header("Knife Hero")]
    [SerializeField] public AudioClip knifeHeroAttack = null;

    [Header("Bow Hero")]
    [SerializeField] public AudioClip bowHeroAttack = null;
    [SerializeField] public AudioClip heroBowExplode = null;


    [Header("Mage Hero")]
    [SerializeField] public AudioClip mageHeroAttack = null;
    [SerializeField] public AudioClip mageBallExplode = null;

    [Header("Healer Hero")]
    [SerializeField] public AudioClip healZoneSpawn = null;

    [Header("Poisoner Hero")]
    [SerializeField] public AudioClip poisonZoneSpawn = null;

    [Header("Burner Hero")]
    [SerializeField] public AudioClip burnerHeroAttack = null;
    [SerializeField] public AudioClip fireBurnExplode = null;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        UpdateVolumes();
    }


    /// <summary>
    /// Play the button sound.
    /// </summary>
    public void PlayButtonSound()
    {
        PlaySound(button);
    }


    /// <summary>
    /// Play the given audio clip as sound.
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlaySound(AudioClip audioClip)
    {
        soundSource.PlayOneShot(audioClip);
    }


    /// <summary>
    /// Play the given audio clip as music.
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.clip = audioClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Stop music
    /// </summary>
    public void StopMusic()
    {
        musicSource.clip = null;
        musicSource.Stop();
    }    


    /// <summary>
    /// Update the sound volume and music volume.
    /// </summary>
    public void UpdateVolumes()
    {
        soundSource.volume = PlayerDataController.SoundVolume;
        musicSource.volume = PlayerDataController.MusicVolume;
    }
}
