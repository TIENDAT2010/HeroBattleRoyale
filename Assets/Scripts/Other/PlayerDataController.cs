using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController
{
    private const string PLAYER_DATA = "playerdata";
    public static int CurrentCash { get; private set; }
    public static int CurrentLevel { get; private set; }
    public static float SoundVolume {  get; private set; }
    public static float MusicVolume {  get; private set; }
    public static float Brightness { get; private set; }

    public static void InitPlayerData()
    {
        if (!PlayerPrefs.HasKey(PLAYER_DATA))
        {
            PlayerData playerData = new PlayerData();
            playerData.levelGame = 1;
            CurrentLevel = playerData.levelGame;
            playerData.currentCash = 500;
            CurrentCash = playerData.currentCash;
            playerData.soundVolume = 1f;
            SoundVolume = 1f;
            playerData.musicVolume = 1f;
            MusicVolume = 1f;
            playerData.brightness = 1f;
            Brightness = 1f;
            HeroData heroData = new HeroData();
            heroData.levelHero = 1;
            heroData.heroName = "Richard";
            heroData.heroID = HeroID.SwordHero;
            playerData.listUnlockHero = new List<HeroData> { heroData };
            PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
        }
        else
        {
            string data = PlayerPrefs.GetString(PLAYER_DATA);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
            CurrentLevel = playerData.levelGame;
            CurrentCash = playerData.currentCash;
            SoundVolume = playerData.soundVolume;
            MusicVolume = playerData.musicVolume;
            Brightness = playerData.brightness;
        }
    }


    public static void UpdateLevel(int level)
    {
        int totalLevel = Resources.LoadAll<LevelGameConfigSO>("LevelConfig/").Length;
        if (CurrentLevel < totalLevel)
        {
            string data = PlayerPrefs.GetString(PLAYER_DATA);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
            playerData.levelGame = level;
            CurrentLevel = level;
            PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
        }    
    }


    public static void UpdateCash(int addedCash)
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        int newCash = playerData.currentCash + addedCash;
        playerData.currentCash = newCash;
        CurrentCash = newCash;
        PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
    }


    public static void UpdateSoundVolume(float volume)
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        playerData.soundVolume = volume;
        SoundVolume = volume;
        PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
    }


    public static void UpdateMusicVolume(float volume)
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        playerData.musicVolume = volume;
        MusicVolume = volume;
        PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
    }


    public static void UpdateBrightness(float brightness)
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        playerData.brightness = brightness;
        Brightness = brightness;
        PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
    }


    public static void UpdateUnlockHero(string heroname, HeroID heroID)
    {
        if(!IsUnlockHero(heroname))
        {
            string data = PlayerPrefs.GetString(PLAYER_DATA);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
            HeroData newHero = new HeroData();
            newHero.levelHero = 1;
            newHero.heroName = heroname;
            newHero.heroID = heroID;
            playerData.listUnlockHero.Add(newHero);
            PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
        }
    }


    public static bool IsUnlockHero(string heroname)
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        for(int i = 0; i < playerData.listUnlockHero.Count; i++)
        {
            if(playerData.listUnlockHero[i].heroName == heroname)
            {
                return true;
            }    
        }
        return false;
    }


    public static void UpgradeHero(string heroname)
    {
        if(IsUnlockHero(heroname))
        {
            string data = PlayerPrefs.GetString(PLAYER_DATA);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
            for (int i = 0; i < playerData.listUnlockHero.Count; i++)
            {
                if (playerData.listUnlockHero[i].heroName == heroname)
                {
                    playerData.listUnlockHero[i].levelHero += 1;
                    break;
                }
            }
            PlayerPrefs.SetString(PLAYER_DATA, JsonUtility.ToJson(playerData));
        }    
    }


    public static int GetLevelHero(string heroname)
    {
        int levelHero = 0;
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        for (int i = 0; i < playerData.listUnlockHero.Count; i++)
        {
            if (playerData.listUnlockHero[i].heroName == heroname)
            {
                levelHero = playerData.listUnlockHero[i].levelHero;
            }
        }
        return levelHero;
    }


    public static HeroData GetFirstUnlockHero()
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        return playerData.listUnlockHero[0];
    }


    public static List<HeroData> GetUnlockHero()
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        return playerData.listUnlockHero;
    }


    public static int GetLevelGame()
    {
        string data = PlayerPrefs.GetString(PLAYER_DATA);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
        return playerData.levelGame;
    }
   
}
