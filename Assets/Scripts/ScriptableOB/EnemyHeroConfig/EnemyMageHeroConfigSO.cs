using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MageHeroConfig", menuName = "EnemyHeroConfig/MageHeroConfig", order = 1)]
public class EnemyMageHeroConfigSO : ScriptableObject
{
    public List<EnemyMageHeroData> listHeroData = new List<EnemyMageHeroData>();


    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EnemyMageHeroData GetHeroData(int level)
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
public class EnemyMageHeroData : HeroBaseData
{
    public float damage = 0f;
    public float attackRange = 0f;
    public int coinReward = 0;
}
