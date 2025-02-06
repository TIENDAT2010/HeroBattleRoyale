using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private HeroController[] playerHeroPrefab = null;
    [SerializeField] private HeroController[] enemyHeroPrefab = null;
    [SerializeField] private TowerController[] towerPrefabs = null;
    [SerializeField] private UnlockedUIHeroItem unlockedHeroItemPrefab = null;
    [SerializeField] private ParticleSystem spawnEffectPrefab = null;
    [SerializeField] RectTransform coinItemPrefab = null;

    private List<HeroController> listHero = new List<HeroController>();
    private List<TowerController> listTower = new List<TowerController>();
    private List<UnlockedUIHeroItem> listUnlockedHero = new List<UnlockedUIHeroItem>();
    private List<ParticleSystem> listSpawnEffect = new List<ParticleSystem>();
    private List<RectTransform> listCoinItem = new List<RectTransform>();

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



    /// <summary>
    /// Find the target with given transform.
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public TargetController FindTarget(Transform trans)
    {
        foreach(HeroController hero in listHero)
        {
            if(hero.transform.Equals(trans))
            {
                return hero;
            }
        }

        foreach(TowerController tower in listTower)
        {
            if(tower.transform.Equals(trans)) { return tower; }
        }
        return null;
    }




    /// <summary>
    /// Find all active heroes with Type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<HeroController> FindActiveHeroes(HeroType type)
    {
        List<HeroController> heroList = new List<HeroController>();
        foreach (HeroController heroController in listHero)
        {
            if(heroController.gameObject.activeSelf && heroController.IsDead == false && heroController.HeroType == type)
            {
                heroList.Add(heroController);
            }    
        }
        return heroList;
    }



    /// <summary>
    /// Find all active towers with Type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<TowerController> FindActiveTowers(TowerType type)
    {
        List<TowerController> towerList = new List<TowerController>();
        foreach (TowerController towerController in listTower)
        {
            if (towerController.gameObject.activeSelf && towerController.IsDead == false && towerController.TowerType == type)
            {
                towerList.Add(towerController);
            }
        }
        return towerList;
    }




    /// <summary>
    /// Get HeroController with hero type and name.
    /// </summary>
    /// <param name="heroType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public HeroController GetHeroController(HeroType heroType, string name)
    {
        HeroController hero = listHero.Where(a => a.HeroType.Equals(heroType) && a.HeroName.Equals(name) && !a.gameObject.activeSelf).FirstOrDefault();
        if(hero == null)
        {
            HeroController prefab = playerHeroPrefab.Where(a => a.HeroType.Equals(heroType) && a.HeroName.Equals(name)).FirstOrDefault();
            if(prefab == null) { prefab = enemyHeroPrefab.Where(a => a.HeroType.Equals(heroType) && a.HeroName.Equals(name)).FirstOrDefault(); }
            hero = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            listHero.Add(hero);
        }
        hero.gameObject.SetActive(true);
        return hero;
    }


    /// <summary>
    /// Get TowerController with tower type and name.
    /// </summary>
    /// <param name="towerType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public TowerController GetTowerController(TowerType towerType, string name)
    {
        TowerController tower = listTower.Where(a => a.TowerType.Equals(towerType) && a.TowerName.Equals(name) && !a.gameObject.activeSelf).FirstOrDefault();
        if (tower == null)
        {
            TowerController prefab = towerPrefabs.Where(a => a.TowerType.Equals(towerType) && a.TowerName.Equals(name)).FirstOrDefault();
            tower = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            listTower.Add(tower);
        }
        tower.gameObject.SetActive(true);
        return tower;
    }



    /// <summary>
    /// Get UnlockedUIHeroItem.
    /// </summary>
    /// <returns></returns>
    public UnlockedUIHeroItem GetUnlockedHeroItem()
    {
        UnlockedUIHeroItem unlockedHeroItem = listUnlockedHero.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
        if (unlockedHeroItem == null)
        {
            unlockedHeroItem = Instantiate(unlockedHeroItemPrefab, Vector3.zero, Quaternion.identity);
            listUnlockedHero.Add(unlockedHeroItem);
        }
        unlockedHeroItem.gameObject.SetActive(true);
        return unlockedHeroItem;
    }




    /// <summary>
    /// Get the spawn effect.
    /// </summary>
    /// <returns></returns>
    public ParticleSystem GetSpawnEffect()
    {
        ParticleSystem spawnEffect = listSpawnEffect.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
        if (spawnEffect == null)
        {
            spawnEffect = Instantiate(spawnEffectPrefab, Vector3.zero, Quaternion.identity);
            listSpawnEffect.Add(spawnEffect);
        }
        spawnEffect.gameObject.SetActive(true);
        return spawnEffect;
    }



    /// <summary>
    /// Get the coin item on UI.
    /// </summary>
    /// <param name="ingameView"></param>
    /// <returns></returns>
    public RectTransform GetCoinItem(RectTransform ingameView)
    {
        RectTransform coinItem = listCoinItem.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
        if (coinItem == null)
        {
            coinItem = Instantiate(coinItemPrefab, Vector3.zero, Quaternion.identity);
            listCoinItem.Add(coinItem);
        }
        coinItem.gameObject.transform.SetParent(ingameView.transform);
        coinItem.gameObject.SetActive(true);
        return coinItem;
    }
}
