using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData 
{
    public int levelGame;
    public int currentCash;
    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public float brightness = 1f;
    public List<HeroData> listUnlockHero = new List<HeroData>();
}



[System.Serializable]
public class HeroData
{
    public string heroName;
    public int levelHero;
    public HeroID heroID;
}
