using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BowHeroConfig", menuName = "EnemyHeroConfig/BowHeroConfig", order = 1)]
public class EnemyBowHeroConfigSO : ScriptableObject
{
    public List<EnemyBowHeroData> listHeroData = new List<EnemyBowHeroData>();


    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EnemyBowHeroData GetHeroData(int level)
    {
        for(int i = 0; i < listHeroData.Count; i++)
        {
            if(listHeroData[i].level == level)
            {
                return listHeroData[i];
            }
        }
        return listHeroData[listHeroData.Count - 1];
    }
}

[System.Serializable]
public class EnemyBowHeroData : HeroBaseData
{
    public float damage = 0f;
    public float attackRange = 0f; 
    public int coinReward = 0;
}

