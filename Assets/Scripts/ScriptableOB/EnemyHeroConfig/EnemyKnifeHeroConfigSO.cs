using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "KnifeHeroConfig", menuName = "EnemyHeroConfig/KnifeHeroConfig", order = 1)]
public class EnemyKnifeHeroConfigSO : ScriptableObject
{
    public List<EnemyKnifeHeroData> listHeroData = new List<EnemyKnifeHeroData>();


    /// <summary>
    /// Get the hero data of this hero config based on given level number.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public EnemyKnifeHeroData GetHeroData(int level)
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
public class EnemyKnifeHeroData : HeroBaseData
{
    public float damage = 0f;
    public float defense = 0f;
    public int coinReward = 0;
}
