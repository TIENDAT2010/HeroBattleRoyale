using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwordHeroController : HeroController
{
    public TargetController TargetAttack { get; private set; }
    public float SpeedRun { get; private set; }
    public float Damage { get; private set; }
    public int CoinReward { get; private set; }

    private float defense = 0;

    public override void OnInit(int level)
    {
        if(heroType == HeroType.PlayerHero)
        {
            PlayerSwordHeroConfigSO swordHeroesConfigSO = Resources.Load<PlayerSwordHeroConfigSO>("HeroConfig/SwordHeroConfig/Player" + "/" + heroName);
            PlayerSwordHeroData playerHeroData = swordHeroesConfigSO.GetHeroData(level);
            CurrentHealth = playerHeroData.health;
            InitHealth = playerHeroData.health;
            defense = playerHeroData.defense;
            SpeedRun = playerHeroData.speedRun;
            Damage = playerHeroData.damage;

            //Disable effects that only the player hero has
            healthEffect.gameObject.SetActive(false);
            poisonEffect.gameObject.SetActive(false);
        }
        else
        {
            EnemySwordHeroConfigSO swordHeroesConfigSO = Resources.Load<EnemySwordHeroConfigSO>("HeroConfig/SwordHeroConfig/Enemy" + "/" + heroName);
            EnemySwordHeroData enemyHeroData = swordHeroesConfigSO.GetHeroData(level);
            CurrentHealth = enemyHeroData.health;
            InitHealth = enemyHeroData.health;
            defense = enemyHeroData.defense;
            SpeedRun = enemyHeroData.speedRun;
            Damage = enemyHeroData.damage;
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
            if (distance < 2.5f) { return true; }
            return false;
        }
        else
        {
            if (distance < 2f) { return true; }
            return false;
        }
    }

    public override void OnTakeDamage(float damage)
    {
        CurrentHealth = CurrentHealth - (damage - damage * defense);
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

}
