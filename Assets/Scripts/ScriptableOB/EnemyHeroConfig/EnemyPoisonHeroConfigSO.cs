
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PoisonerHeroConfig", menuName = "EnemyHeroConfig/PoisonerHeroConfig", order = 1)]
public class EnemyPoisonerHeroConfigSO : ScriptableObject
{
    public List<EnemyPoisonerHeroData> listHeroData = new List<EnemyPoisonerHeroData>();


    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EnemyPoisonerHeroData GetHeroData(int level)
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
public class EnemyPoisonerHeroData : HeroBaseData
{
    public float damagePerSecond = 0f;
    public float attackRange = 0f;
    public int coinReward = 0;
}
