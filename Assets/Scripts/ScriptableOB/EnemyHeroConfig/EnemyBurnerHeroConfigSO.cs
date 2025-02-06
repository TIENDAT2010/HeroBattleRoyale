using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnerHeroConfig", menuName = "EnemyHeroConfig/BurnerHeroConfig", order = 1)]
public class EnemyBurnerHeroConfigSO : ScriptableObject
{
    public List<EnemyBurnerHeroData> listHeroData = new List<EnemyBurnerHeroData>();

    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EnemyBurnerHeroData GetHeroData(int level)
    {
        for (int i = 0; i < listHeroData.Count; i++)
        {
            if (listHeroData[i].level == level)
            {
                return listHeroData[i];
            }
        }
        return listHeroData[listHeroData.Count - 1];
    }
}

[System.Serializable]
public class EnemyBurnerHeroData : HeroBaseData
{
    public float damagePerSecond = 0f;
    public float attackRange = 0f; 
    public int coinReward = 0;
}
