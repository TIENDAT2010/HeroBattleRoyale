using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerHeroController : HeroController
{
    [SerializeField] private ParticleSystem channelEffect = null;
    [SerializeField] private HealZoneController healZonePrefab = null;


    public ParticleSystem ChannelEffect => channelEffect;
    public HeroController TargetHero { get; private set; }
    public float SpeedRun { get; private set; }
    public float HealPerSecond { get; private set; }


    private List<HealZoneController> listHealZoneController = new List<HealZoneController>();
    private float attackRange = 0;

    public override void OnInit(int level)
    {
        if(heroType == HeroType.PlayerHero)
        {
            PlayerHealerHeroConfigSO healerHeroesConfigSO = Resources.Load<PlayerHealerHeroConfigSO>("HeroConfig/HealerHeroConfig/Player/" + heroName);
            PlayerHealerHeroData playerHeroData = healerHeroesConfigSO.GetHeroData(level);
            CurrentHealth = playerHeroData.health;
            InitHealth = playerHeroData.health;
            SpeedRun = playerHeroData.speedRun;
            HealPerSecond = playerHeroData.healPerSecond;
            attackRange = playerHeroData.healRange;
        }
        IsDead = false;
        IsBurned = false;
        IsPoisoned = false;
        burnEffect.gameObject.SetActive(false);
        OnNextState(HeroState.Idle_State);
    }


    public override void FindTarget()
    {
        TargetHero = IngameManager.Instance.GetTargetHeroForHealerHero(gameObject);
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


    public override void OnBeingPoisoned(float hp)
    {
        base.OnBeingPoisoned(hp);
    }


    public override void OnBeingBurned(float damage)
    {
        base.OnBeingBurned(damage);
    }


    public override void OnReceiveHealth(float hp)
    {
        base.OnReceiveHealth(hp);
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


    public HealZoneController GetHealZoneController()
    {
        //Find in the list
        HealZoneController healZone = listHealZoneController.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

        if (healZone == null)
        {
            //Did not find one -> create new one
            healZone = Instantiate(healZonePrefab, Vector3.zero, Quaternion.identity);
            healZone.gameObject.SetActive(false);
            listHealZoneController.Add(healZone);
        }

        return healZone;
    }
}
