using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelGameConfig", menuName = "LevelConfiguration/LevelGameConfig", order = 1)]
public class LevelGameConfigSO : ScriptableObject
{
    [Header("Map ID")]
    public string mapID = "Map_0";
    public AudioClip backgroundMusic = null;

    [Header("Main Tower Config")]
    public float PlayerTowerHealth = 0f;
    public float EnemyTowerHealth = 0f;

    [Header("Archer Tower Config")]
    public float PlayerArcherTowerHealth = 100f;
    public float PlayerArcherTowerDamage = 10f;
    public float EnemyArcherTowerHealth = 100f;
    public float EnemyArcherTowerDamage = 10f;

    [Header("Magic Tower Config")]
    public float PlayerMagicTowerHealth = 100f;
    public float PlayerMagicTowerDamage = 10f;
    public float EnemyMagicTowerHealth = 100f;
    public float EnemyMagicTowerDamage = 10f;

    [Header("Coin Config")]
    public int InitCoin = 0;

    [Header("Waves Config")]
    public float waveDelayTime = 5f;
    public List<WaveConfig> ListWaveConfig = new List<WaveConfig>();
}

[Serializable]
public class WaveConfig
{
    public float enemyDelayTime = 1f;
    public List<EnemyHeroConfig> enemyHeroConfigs = new List<EnemyHeroConfig>();
}


[Serializable]
public class EnemyHeroConfig
{
    public int levelHero = 0;
    public string heroName = string.Empty;
}