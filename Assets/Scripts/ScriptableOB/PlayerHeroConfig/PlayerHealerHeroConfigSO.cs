using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealerHeroConfig", menuName = "PlayerHeroConfig/HealerHeroConfig", order = 1)]
public class PlayerHealerHeroConfigSO : ScriptableObject
{
    public List<PlayerHealerHeroData> listHeroData = new List<PlayerHealerHeroData>();


    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public PlayerHealerHeroData GetHeroData(int level)
    {
        PlayerHealerHeroData data = new PlayerHealerHeroData();
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
public class PlayerHealerHeroData : HeroBaseData
{
    public float healPerSecond = 0f;
    public float healRange = 0f;
    public float cooldown = 0f;
    public int priceUnlock = 50;
    public int coinToDeploy = 0;
}
