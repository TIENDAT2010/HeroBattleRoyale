using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoisonerHeroController : HeroController
{
    [SerializeField] private ParticleSystem channelEffect = null;
    [SerializeField] private PoisonZoneController poisonZonePrefab = null;

    public ParticleSystem ChannelEffect => channelEffect;
    public HeroController TargetAttack { get; private set; }
    public float SpeedRun { get; private set; }
    public float DamagePerSecond { get; private set; }
    public int CoinReward { get; private set; }


    private List<PoisonZoneController> listPoisonZoneController = new List<PoisonZoneController>();
    private float attackRange = 0;

    public override void OnInit(int level)
    {
        if(heroType == HeroType.EnemyHero)
        {
            EnemyPoisonerHeroConfigSO poisonMageHeroesConfigSO = Resources.Load<EnemyPoisonerHeroConfigSO>("HeroConfig/PoisonerHeroConfig/Enemy/" + heroName);
            EnemyPoisonerHeroData enemyHeroData = poisonMageHeroesConfigSO.GetHeroData(level);
            CurrentHealth = enemyHeroData.health;
            InitHealth = enemyHeroData.health;
            SpeedRun = enemyHeroData.speedRun;
            DamagePerSecond = enemyHeroData.damagePerSecond;
            attackRange = enemyHeroData.attackRange;
            CoinReward = enemyHeroData.coinReward;
        }
        IsDead = false;
        IsBurned = false;
        IsPoisoned = false;
        burnEffect.gameObject.SetActive(false);
        channelEffect.gameObject.SetActive(false);
        OnNextState(HeroState.Idle_State);
    }


    public override void FindTarget()
    {
        TargetAttack = IngameManager.Instance.GetTargetHeroForPoisonerHero(transform);
    }

    public override bool IsInRange(Transform targetPos)
    {
        float distance = Vector3.Distance(targetPos.transform.position, transform.position);
        if (distance <= attackRange) 
        {
            return true; 
        }
        return false;
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


    public PoisonZoneController GetPoisonZoneController()
    {
        //Find in the list
        PoisonZoneController poisonZone = listPoisonZoneController.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

        if (poisonZone == null)
        {
            //Did not find one -> create new one
            poisonZone = Instantiate(poisonZonePrefab, Vector3.zero, Quaternion.identity);
            poisonZone.gameObject.SetActive(false);
            listPoisonZoneController.Add(poisonZone);
        }

        return poisonZone;
    }
}
