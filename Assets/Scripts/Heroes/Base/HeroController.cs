using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : TargetController
{
    [Header("Enemy References")]
    [SerializeField] protected string heroName = string.Empty;
    [SerializeField] protected HeroType heroType = HeroType.PlayerHero;
    [SerializeField] protected Animator animator = null;
    [SerializeField] protected NavMeshObstacle navMeshObstacle = null;
    [SerializeField] protected CharacterController characterController = null;
    [SerializeField] protected CapsuleCollider capsuleCollider = null;
    [SerializeField] protected HeroIdleController idleController = null;
    [SerializeField] protected HeroRunController runController = null;
    [SerializeField] protected HeroAttackController attackController = null;
    [SerializeField] protected HeroDeadController deadController = null;
    [SerializeField] protected ParticleSystem healthEffect = null;
    [SerializeField] protected ParticleSystem poisonEffect = null;
    [SerializeField] protected ParticleSystem burnEffect = null;

    public string HeroName => heroName;
    public HeroType HeroType => heroType;
    public bool IsPoisoned { protected set; get; }
    public bool IsBurned { protected set; get; }
    public float InitHealth { protected set; get; }
    public float CurrentHealth { protected set; get; }
    public Animator Animator => animator;
    public NavMeshObstacle NavMeshObstacle => navMeshObstacle;
    public CharacterController CharacterController => characterController;
    public CapsuleCollider CapsuleCollider => capsuleCollider;
    private float blendSpeed = 0;



    public virtual void OnInit(int level) { }    

    public virtual void OnNextState(HeroState nextState) { }

    public virtual void FindTarget() { }
    public virtual bool IsInRange(Transform targetPos) { return false; }

    public override void OnTakeDamage(float damage) { }

    private Coroutine blenspeedCoroutine = null;



    /// <summary>
    /// Handle this hero receive health.
    /// </summary>
    /// <param name="hp"></param>
    public override void OnReceiveHealth(float hp)
    {
        CurrentHealth += hp;
        if(CurrentHealth > InitHealth) { CurrentHealth = InitHealth; }
        if(healthEffect.gameObject.activeSelf == false)
        {
            StartCoroutine(CRPlayHealEffect());
        }
    }

    /// <summary>
    /// Coroutine play the heal effect.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRPlayHealEffect()
    {
        healthEffect.gameObject.SetActive(true);
        healthEffect.Play();
        yield return new WaitForSeconds(2f);
        healthEffect.gameObject.SetActive(false);
    }




    /// <summary>
    /// Handle this hero being poisoned.
    /// </summary>
    /// <param name="damagePerSec"></param>
    public override void OnBeingPoisoned(float damagePerSec)
    {
        if(IsPoisoned == false && IsDead == false)
        {
            IsPoisoned = true;
            poisonEffect.gameObject.SetActive(true);
            poisonEffect.Play();
            StartCoroutine(CRApplyDamagePerSecond(damagePerSec));
        }
    }


    /// <summary>
    /// Handle this hero being burned.
    /// </summary>
    /// <param name="damagePerSec"></param>
    public override void OnBeingBurned(float damagePerSec)
    {
        if(IsBurned == false && IsDead == false)
        {
            IsBurned = true;
            burnEffect.gameObject.SetActive(true);
            burnEffect.Play();
            StartCoroutine(CRApplyDamagePerSecond(damagePerSec));
        }
    }


    /// <summary>
    /// Coroutine apply damage per second
    /// </summary>
    /// <param name="damagePerSec"></param>
    /// <returns></returns>
    private IEnumerator CRApplyDamagePerSecond(float damagePerSec)
    {
        while (IsDead == false)
        {
            OnTakeDamage(damagePerSec / 2f);
            yield return new WaitForSeconds(0.5f);
        }
    }






    /// <summary>
    /// Set the "speed" factor in the animator.
    /// S
    /// </summary>
    /// <param name="newSpeed"></param>
    /// <param name="hardSet"></param>
    public void SetBlendSpeed(float newSpeed, bool hardSet)
    {
        if (hardSet) 
        { 
            animator.SetFloat("Speed", newSpeed);
            if(blenspeedCoroutine != null)
            {
                StopCoroutine(blenspeedCoroutine);
                blenspeedCoroutine = null;
            }    
        }
        else 
        {
            if (blenspeedCoroutine != null)
            {
                StopCoroutine(blenspeedCoroutine);
            }
            blenspeedCoroutine = StartCoroutine(CRBlendSpeed(newSpeed)); 
        }
    }


    /// <summary>
    /// Coroutine blend the "speed" factor in animator to newSpeed.
    /// </summary>
    /// <param name="newSpeed"></param>
    /// <returns></returns>
    private IEnumerator CRBlendSpeed(float newSpeed)
    {
        blendSpeed = animator.GetFloat("Speed");
        if (blendSpeed > newSpeed)
        {
            while (blendSpeed > newSpeed)
            {
                blendSpeed = Mathf.Clamp(blendSpeed - 5f * Time.deltaTime, newSpeed, blendSpeed);
                animator.SetFloat("Speed", blendSpeed);
                yield return null;
            }
        }
        else
        {
            while (blendSpeed < newSpeed)
            {
                blendSpeed = Mathf.Clamp(blendSpeed + 5f * Time.deltaTime, blendSpeed, newSpeed);
                animator.SetFloat("Speed", blendSpeed);
                yield return null;
            }
        }
    }   
}
