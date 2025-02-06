using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "KnifeHeroConfigSO", menuName = "PlayerHeroConfig/KnifeHeroConfig", order = 1)]
public class PlayerKnifeHeroConfigSO : ScriptableObject
{
    public List<PlayerKnifeHeroData> listHeroData = new List<PlayerKnifeHeroData>();



    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public PlayerKnifeHeroData GetHeroData(int level)
    {
        PlayerKnifeHeroData data = new PlayerKnifeHeroData();
        for (int i = 0; i < listHeroData.Count; i++)
        {
            if (listHeroData[i].level == level)
            {
                data = listHeroData[i];
            }
        }
        return data;
    }



    /// <summary>
    /// Get the max level of this hero config.
    /// </summary>
    /// <returns></returns>
    public int GetMaxLevel()
    {
        int maxLevel = 0;
        for (int i = 0; i < listHeroData.Count; i++)
        {
            if (listHeroData[i].level > maxLevel)
            {
                maxLevel = listHeroData[i].level;
            }
        }
        return maxLevel;
    }
}

[System.Serializable]
public class PlayerKnifeHeroData : HeroBaseData
{
    public float damage = 0f;
    public float defense = 0f;
    public float cooldown = 0f;
    public int priceUnlock = 50;
    public int coinToDeploy = 0;
}
