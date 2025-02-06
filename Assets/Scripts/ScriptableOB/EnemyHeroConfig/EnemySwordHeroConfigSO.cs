using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SwordHeroConfig", menuName = "EnemyHeroConfig/SwordHeroConfig", order = 1)]
public class EnemySwordHeroConfigSO : ScriptableObject
{
    public List<EnemySwordHeroData> listHeroData = new List<EnemySwordHeroData>();


    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EnemySwordHeroData GetHeroData(int level)
    {
        EnemySwordHeroData data = new EnemySwordHeroData();
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
public class EnemySwordHeroData : HeroBaseData
{
    public float damage = 0f;
    public float defense = 0f;
    public int coinReward = 0;
}

