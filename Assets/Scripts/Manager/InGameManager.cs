using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance { get; private set; }
    public GameState GameState { private set; get; }
    public int CurrentLevel { private set; get; }
    public int CurrentCoin { private set; get; }
    public MainTowerController PlayerMainTower => playerMainTower;
    public MainTowerController EnemyMainTower => enemyMainTower;


    [SerializeField] private Transform playerMainTowerTransform = null;
    [SerializeField] private Transform enemyMainTowerTransform = null;

    [SerializeField] private Transform playerAcherTowerTransform = null;
    [SerializeField] private Transform enemyArcherTowerTransform = null;

    [SerializeField] private Transform playerMagicTowerTransform = null;
    [SerializeField] private Transform enemyMagicTowerTransform = null;

    [SerializeField] private LayerMask groundLayer = new LayerMask();
    [SerializeField] LayerMask enemyHeroLayer = new LayerMask();
    [SerializeField] LayerMask playerHeroLayer = new LayerMask();
    [SerializeField] private HeroSpawner heroSpawner = null;
    [SerializeField] Transform[] listEnemyHeroPos = null;
    [SerializeField] CameraParentController cameraController = null;


    private LevelGameConfigSO levelGameConfig = null;
    private MainTowerController playerMainTower = null;
    private MainTowerController enemyMainTower = null;
    private ArcherTowerController playerArcherTower = null;
    private ArcherTowerController enemyArcherTower = null;
    private MagicTowerController playerMagicTower = null;
    private MagicTowerController enemyMagicTower = null;


    private void Awake()
    {
        if (Instance != null)
        {
            Instance = null;
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private IEnumerator Start()
    {
        GameState = GameState.GameStart;
        PlayerDataController.InitPlayerData();
        StartCoroutine(CRShowViewWithDelay(ViewType.IngameView, 0.05f));
        CurrentLevel = PlayerDataController.GetLevelGame();


        LevelGameConfigSO levelGameConfigSO = Resources.Load<LevelGameConfigSO>("LevelConfig/" + CurrentLevel);
        levelGameConfig = levelGameConfigSO;
        CurrentCoin = levelGameConfigSO.InitCoin;
        SoundManager.Instance.PlayMusic(levelGameConfigSO.backgroundMusic);

        playerMainTower = (MainTowerController)PoolManager.Instance.GetTowerController(TowerType.PlayerTower, "MainTower");
        playerMainTower.transform.position = playerMainTowerTransform.position;
        playerMainTower.transform.forward = playerMainTowerTransform.forward;
        playerMainTower.OnInit(levelGameConfigSO.PlayerTowerHealth, 0);


        enemyMainTower = (MainTowerController)PoolManager.Instance.GetTowerController(TowerType.EnemyTower, "MainTower");
        enemyMainTower.transform.position = enemyMainTowerTransform.position;
        enemyMainTower.transform.forward = enemyMainTowerTransform.forward;
        enemyMainTower.OnInit(levelGameConfigSO.EnemyTowerHealth, 0);


        playerArcherTower = (ArcherTowerController)PoolManager.Instance.GetTowerController(TowerType.PlayerTower, "ArcherTower");
        playerArcherTower.transform.position = playerAcherTowerTransform.position;
        playerArcherTower.transform.forward = playerAcherTowerTransform.forward;
        playerArcherTower.OnInit(levelGameConfig.PlayerArcherTowerHealth, levelGameConfig.PlayerArcherTowerDamage);

        enemyArcherTower = (ArcherTowerController)PoolManager.Instance.GetTowerController(TowerType.EnemyTower, "ArcherTower");
        enemyArcherTower.transform.position = enemyArcherTowerTransform.position;
        enemyArcherTower.transform.forward = enemyArcherTowerTransform.forward;
        enemyArcherTower.OnInit(levelGameConfig.EnemyArcherTowerHealth, levelGameConfig.EnemyArcherTowerDamage);


        playerMagicTower = (MagicTowerController)PoolManager.Instance.GetTowerController(TowerType.PlayerTower, "MagicTower");
        playerMagicTower.transform.position = playerMagicTowerTransform.position;
        playerMagicTower.transform.forward = playerMagicTowerTransform.forward;
        playerMagicTower.OnInit(levelGameConfig.PlayerMagicTowerHealth, levelGameConfig.PlayerMagicTowerDamage);

        enemyMagicTower = (MagicTowerController)PoolManager.Instance.GetTowerController(TowerType.EnemyTower, "MagicTower");
        enemyMagicTower.transform.position = enemyMagicTowerTransform.position;
        enemyMagicTower.transform.forward = enemyMagicTowerTransform.forward;
        enemyMagicTower.OnInit(levelGameConfig.EnemyMagicTowerHealth, levelGameConfig.EnemyArcherTowerDamage);


        yield return null;

        List<HeroData> listPlayerData = PlayerDataController.GetUnlockHero();
        for (int i = 0; i < listPlayerData.Count; i++)
        {
            ViewManager.Instance.IngameView.CreateUnlockedHeroItem(listPlayerData[i].heroName, listPlayerData[i].heroID, listPlayerData[i].levelHero);
        }
        ViewManager.Instance.IngameView.SetScrollView();
        StartCoroutine(CRSpawnEnemy());
    }


    /// <summary>
    /// Coroutine spawn the enemyHero heros based on the level config.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRSpawnEnemy()
    {
        List<WaveConfig> waveConfig = levelGameConfig.ListWaveConfig;
        for(int i = 0; i < waveConfig.Count; i++)
        {
            List<EnemyHeroConfig> enemyHeroConfigs = new List<EnemyHeroConfig>(waveConfig[i].enemyHeroConfigs);

            while(enemyHeroConfigs.Count > 0)
            {
                int rdIndex = Random.Range(0, enemyHeroConfigs.Count - 1);

                //Check the positon
                Transform enemyPos = listEnemyHeroPos[Random.Range(0, listEnemyHeroPos.Length)];
                bool hasHero = Physics.CheckSphere(enemyPos.position, 1f, enemyHeroLayer | playerHeroLayer);
                while (hasHero)
                {
                    enemyPos = listEnemyHeroPos[Random.Range(0, listEnemyHeroPos.Length)];
                    hasHero = Physics.CheckSphere(enemyPos.position, 1f, enemyHeroLayer | playerHeroLayer);
                    yield return new WaitForSeconds(0.1f);
                }

                //Spawn the hero
                HeroController enemyHero = PoolManager.Instance.GetHeroController(HeroType.EnemyHero, enemyHeroConfigs[rdIndex].heroName);
                enemyHero.transform.position = enemyPos.position;
                enemyHero.transform.forward = Vector3.back;
                enemyHero.gameObject.SetActive(true);
                enemyHero.OnInit(enemyHeroConfigs[rdIndex].levelHero);
                enemyHeroConfigs.RemoveAt(rdIndex);

                //Play the spawn effect
                ParticleSystem spawnEffect = PoolManager.Instance.GetSpawnEffect();
                spawnEffect.transform.position = enemyPos.position;
                StartCoroutine(CRPlayParticle(spawnEffect));

                yield return new WaitForSeconds(waveConfig[i].enemyDelayTime);
            }

            yield return new WaitForSeconds(levelGameConfig.waveDelayTime);
        }
    }



    /// <summary>
    /// Play the given particle then disable it.
    /// </summary>
    /// <param name="particle"></param>
    /// <returns></returns>
    private IEnumerator CRPlayParticle(ParticleSystem particle)
    {
        var main = particle.main;
        particle.Play();
        yield return new WaitForSeconds(main.startLifetimeMultiplier + 0.5f);
        particle.gameObject.SetActive(false);
    }


    /// <summary>
    /// Coroutine show the view with delay time and callback.
    /// </summary>
    /// <param name="viewType"></param>
    /// <param name="delayTime"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator CRShowViewWithDelay(ViewType viewType, float delayTime, System.Action callback = null)
    {
        yield return new WaitForSeconds(delayTime);
        ViewManager.Instance.SetActiveView(viewType);
        yield return null;
        callback?.Invoke();
    }


    


    /// <summary>
    /// Level failed.
    /// </summary>
    public void LevelFailed()
    {
        GameState = GameState.LevelFailed;
        cameraController.RotateCamera(playerMainTowerTransform);
        StartCoroutine(CRShowViewWithDelay(ViewType.EndgameView, 0f));
        SoundManager.Instance.PlaySound(SoundManager.Instance.levelFailed);
        SoundManager.Instance.StopMusic();
    }



    /// <summary>
    /// Level failed.
    /// </summary>
    public void LevelCompleted()
    {
        GameState = GameState.LevelCompleted;
        cameraController.RotateCamera(enemyMainTowerTransform);
        StartCoroutine(CRShowViewWithDelay(ViewType.EndgameView, 0f));
        PlayerDataController.UpdateLevel(CurrentLevel + 1);
        SoundManager.Instance.PlaySound(SoundManager.Instance.levelCompleted);
        SoundManager.Instance.StopMusic();
    }



    /// <summary>
    /// Update the CurrentCoin with amount.
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateCoin(int amount)
    {
        CurrentCoin += amount;
    }


    public void OnHeroDrag(Vector3 mousePos ,string heroName)
    {   
        RaycastHit raycastHit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray,out raycastHit, 999f, groundLayer))
        {
            heroSpawner.gameObject.SetActive(true);
            heroSpawner.transform.position = raycastHit.point;
            heroSpawner.CanSpawnHero();
        }
        else
        {
            heroSpawner.gameObject.SetActive(false);
        }    
    }    

    public void OnHeroEndDrag(Vector3 mousePos, string heroName, int coinToDeploy, UnlockedUIHeroItem heroItem)
    {
        RaycastHit raycastHit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out raycastHit, 999f, groundLayer))
        {
            if(heroSpawner.CanSpawnHero())
            {
                if(coinToDeploy <= CurrentCoin)
                {
                    HeroController hero = PoolManager.Instance.GetHeroController(HeroType.PlayerHero, heroName);
                    hero.transform.position = raycastHit.point;
                    hero.OnInit(PlayerDataController.GetLevelHero(heroName));

                    ParticleSystem spawnEffect = PoolManager.Instance.GetSpawnEffect();
                    spawnEffect.transform.position = raycastHit.point;
                    StartCoroutine(CRPlayParticle(spawnEffect));

                    heroItem.Cooldown();
                    UpdateCoin(-coinToDeploy);

                    SoundManager.Instance.PlaySound(SoundManager.Instance.spawnHero);
                }    


            }
            heroSpawner.gameObject.SetActive(false);
        }
    }    




    public TargetController GetTargetController(HeroType type, Transform Pos)
    {
        List<HeroController> heroes = PoolManager.Instance.FindActiveHeroes(type == HeroType.PlayerHero ? HeroType.EnemyHero : HeroType.PlayerHero);
        float minDistance = 999f;
        HeroController targetHero = null;
        foreach (HeroController hero in heroes)
        {
            float distance = Vector3.Distance(Pos.position, hero.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetHero = hero;
            }
        }
          

        if(targetHero != null)
        {
            return targetHero;
        }   
        else
        {
            minDistance = 999f;
            List<TowerController> listTower = PoolManager.Instance.FindActiveTowers(type == HeroType.PlayerHero ? TowerType.EnemyTower : TowerType.PlayerTower);
            TowerController targetTower = null;

            foreach (TowerController tower in listTower)
            {
                if(tower.TowerName == "ArcherTower" || tower.TowerName == "MagicTower")
                {
                    float dis = Vector3.Distance(Pos.position, tower.transform.position);
                    if (dis < minDistance)
                    {
                        minDistance = dis;
                        targetTower = tower;
                    }
                }
            }

            if (targetTower != null)
            {
                return targetTower;
            }
            else
            {
                foreach (TowerController tower in listTower)
                {
                    if (tower.TowerName == "MainTower")
                    {
                        float dis = Vector3.Distance(Pos.position, tower.transform.position);
                        if (dis < minDistance)
                        {
                            minDistance = dis;
                            targetTower = tower;
                        }
                    }
                    return targetTower;
                }
            }

            return targetTower;
        }    
    }




    /// <summary>
    /// Get the target hero for healer hero.
    /// Target target hero will not be the healer calling this function.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="healerSeeker"></param>
    /// <returns></returns>
    public HeroController GetTargetHeroForHealerHero(GameObject healerSeeker)
    {
        List<HeroController> heroes = PoolManager.Instance.FindActiveHeroes(HeroType.PlayerHero);
        float pourcentageHealth = 1f;
        HeroController targetHero = null;
        foreach (HeroController hero in heroes)
        {
            if (hero.gameObject != healerSeeker)
            {
                if ((hero.CurrentHealth / hero.InitHealth) < pourcentageHealth)
                {
                    pourcentageHealth = hero.CurrentHealth / hero.InitHealth;
                    targetHero = hero;
                }
            }
        }
        return targetHero;
    }



    /// <summary>
    /// Get target hero for poisoner hero.
    /// </summary>
    /// <param name="Pos"></param>
    /// <returns></returns>
    public HeroController GetTargetHeroForPoisonerHero(Transform Pos)
    {
        List<HeroController> heroes = PoolManager.Instance.FindActiveHeroes(HeroType.PlayerHero);
        float minDistance = 999f;
        HeroController targetHero = null;
        foreach (HeroController hero in heroes)
        {
            float distance = Vector3.Distance(Pos.position, hero.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetHero = hero;
            }
        }
        return targetHero;
    }



    /// <summary>
    /// Check if the given target is a tower.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsTargetATower(TargetController target)
    {
        List<TowerController> listPlayerTower = PoolManager.Instance.FindActiveTowers(TowerType.PlayerTower);
        List<TowerController> listEnemyTower = PoolManager.Instance.FindActiveTowers(TowerType.PlayerTower);
        foreach(TowerController tower in listPlayerTower)
        {
            if (tower.gameObject.Equals(target.gameObject)) { return true; }
        }
        foreach (TowerController tower in listEnemyTower)
        {
            if (tower.gameObject.Equals(target.gameObject)) { return true; }
        }
        return false;
    }
}
