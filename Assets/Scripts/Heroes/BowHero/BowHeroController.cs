using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowHeroController : HeroController
{
    [SerializeField] private Transform arrowPos = null;
    [SerializeField] private GameObject arrowHolder = null;
    [SerializeField] private ArrowController arrowPrefab = null;


    public Transform ArrowPos => arrowPos;
    public GameObject ArrowHolder => arrowHolder;
    public TargetController TargetAttack { get; private set; }
    public float SpeedRun { get; set; }
    public float Damage { get; set; }
    public int CoinReward { get; private set; }

    private List<ArrowController> listArrowController = new List<ArrowController>();

    private float attackRange = 0;

    public override void OnInit(int level)
    {
        if (heroType == HeroType.PlayerHero)
        {
            PlayerBowHeroConfigSO bowHeroesConfigSO = Resources.Load<PlayerBowHeroConfigSO>("HeroConfig/BowHeroConfig/Player/" + heroName);
            PlayerBowHeroData playerHeroData = bowHeroesConfigSO.GetHeroData(level);
            CurrentHealth = playerHeroData.health;
            InitHealth = playerHeroData.health;
            SpeedRun = playerHeroData.speedRun;
            Damage = playerHeroData.damage;
            attackRange = playerHeroData.attackRange;

            //Disable effects that only the player hero has
            healthEffect.gameObject.SetActive(false);
            poisonEffect.gameObject.SetActive(false);
        }
        else
        {
            EnemyBowHeroConfigSO bowHeroesConfigSO = Resources.Load<EnemyBowHeroConfigSO>("HeroConfig/BowHeroConfig/Enemy/" + heroName);
            EnemyBowHeroData enemyHeroData = bowHeroesConfigSO.GetHeroData(level);
            CurrentHealth = enemyHeroData.health;
            InitHealth = enemyHeroData.health;
            SpeedRun = enemyHeroData.speedRun;
            Damage = enemyHeroData.damage;
            attackRange = enemyHeroData.attackRange;
            CoinReward = enemyHeroData.coinReward;
        }
        IsDead = false;
        IsBurned = false;
        IsPoisoned = false;
        burnEffect.gameObject.SetActive(false);
        OnNextState(HeroState.Idle_State);
    }


    public override void FindTarget()
    {
        TargetAttack = IngameManager.Instance.GetTargetController(heroType, transform);
    }


    public override bool IsInRange(Transform targetPos)
    {
        bool check = false;
        float distance = Vector3.Distance(targetPos.transform.position, transform.position);
        List<TowerController> listTower = PoolManager.Instance.FindActiveTowers(heroType == HeroType.PlayerHero ? TowerType.EnemyTower : TowerType.PlayerTower);
        for (int i = 0; i < listTower.Count; i++)
        {
            if (TargetAttack.Equals(listTower[i]))
            {
                check = true;
            }
        }
        if (check == true)
        {
            if (distance < attackRange + 0.5f) { return true; }
            return false;
        }
        else
        {
            if (distance < attackRange) { return true; }
            return false;
        }
    }

    public override void OnTakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (!IsDead && CurrentHealth <= 0)
        {
            IsDead = true;
            OnNextState(HeroState.Dead_State);

            burnEffect.gameObject.SetActive(false);
            if (heroType == HeroType.PlayerHero)
            {
                healthEffect.gameObject.SetActive(false);
                poisonEffect.gameObject.SetActive(false);
            }
        }
    }

    public override void OnReceiveHealth(float hp)
    {
        base.OnReceiveHealth(hp);
    }

    public override void OnBeingPoisoned(float hp)
    {
        base.OnBeingPoisoned(hp);
    }


    public override void OnBeingBurned(float damage)
    {
        base.OnBeingBurned(damage);
    }


    public override void OnNextState(HeroState nextState)
    {
        if (nextState == HeroState.Idle_State)
        {
            runController.ExitRunState();
            attackController.ExitAttackState();
            deadController.ExitDeadState();
            idleController.EnterIdleState();
        }
        else if (nextState == HeroState.Run_State)
        {
            idleController.ExitHeroIdle();
            attackController.ExitAttackState();
            deadController.ExitDeadState();
            runController.EnterRunState();
        }
        else if (nextState == HeroState.Attack_State)
        {
            idleController.ExitHeroIdle();
            runController.ExitRunState();
            deadController.ExitDeadState();
            attackController.EnterAttackState();
        }
        else if (nextState == HeroState.Dead_State)
        {
            idleController.ExitHeroIdle();
            runController.ExitRunState();
            attackController.ExitAttackState();
            deadController.EnterDeadState();
        }
    }



    /// <summary>
    /// Get an inactive ArrowController object.
    /// </summary>
    /// <returns></returns>
    public ArrowController GetArrowController()
    {
        //Find in the list
        ArrowController bow = listArrowController.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

        if (bow == null)
        {
            //Did not find one -> create new one
            bow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
            bow.gameObject.SetActive(false);
            listArrowController.Add(bow);
        }

        return bow;
    }
}
