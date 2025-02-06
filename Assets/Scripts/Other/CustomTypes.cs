using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameStart = 1,
    GamePause = 2,
    LevelFailed = 3,
    LevelCompleted = 4,
}


public enum ViewType
{
    HomeView = 0,
    IngameView = 1,
    EndgameView = 2,
    HerosView = 3,
    LoadingView = 4,
}


public enum HeroState
{
    Idle_State = 0,
    Run_State = 1,
    Attack_State = 2,
    Dead_State = 3,
}

public enum HeroType
{
    PlayerHero = 0,
    EnemyHero = 1,
}

public enum TowerType
{
    PlayerTower = 0,
    EnemyTower = 1,
}

[System.Serializable]
public enum HeroID
{
    BladeHero = 0,
    BowHero = 1,
    KnifeHero = 2,
    MageHero = 3,
    SwordHero = 4,
    HealerHero = 5,
    PoisonerHero = 6,
    BurnerHero = 7,
    GolemHero = 8,
}

public enum ViewMode
{
    Horizontal = 0,
    Vertical = 1,
}