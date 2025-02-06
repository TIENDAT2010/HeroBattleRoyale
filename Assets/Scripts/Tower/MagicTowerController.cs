using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class MagicTowerController : MainTowerController
{
    [SerializeField] private MagicBallController magicBallPrefab = null;
    [SerializeField] private ParticleSystem chargeEffect = null;

    private Transform targetHero = null;
    private List<MagicBallController> listMagicBall = new List<MagicBallController>();
    private float magicBallDamage = 0;

    public override void OnInit(float health, float damage)
    {
        towerAnim.gameObject.SetActive(false);
        towerMesh.gameObject.SetActive(true);
        fireBigRedEffect.gameObject.SetActive(false);
        towerDestroyEffect.gameObject.SetActive(false);
        chargeEffect.gameObject.SetActive(false);
        CurrentHealth = health;
        magicBallDamage = damage;
        StartCoroutine(CRAttackHero());
    }

    public override void OnTakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, CurrentHealth);
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.towerDestroyed);
            towerColider.enabled = false;
            towerMesh.gameObject.SetActive(false);
            chargeEffect.gameObject.SetActive(false);
            towerDestroyEffect.gameObject.SetActive(true);
            towerDestroyEffect.Play();
            towerAnim.gameObject.SetActive(true);
            animator.Play(towerDestructionAnim.name);
            StopAllCoroutines();
            StartCoroutine(CRHandleTowerDestroyed());
        }
    }

    public override void OnBeingBurned(float damagePerSec)
    {
        OnTakeDamage(damagePerSec);
    }

   

    /// <summary>
    /// Coroutine attack the hero.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRAttackHero()
    {
        while(IsDead == false)
        {
            if(targetHero != null)
            {
                chargeEffect.gameObject.SetActive(true);
                chargeEffect.Play();
                yield return new WaitForSeconds(2f);
                chargeEffect.gameObject.SetActive(false);

                SoundManager.Instance.PlaySound(SoundManager.Instance.magicTowerAttack);
                MagicBallController magicBall = listMagicBall.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
                if (magicBall == null) { magicBall = Instantiate(magicBallPrefab, chargeEffect.transform.position, Quaternion.identity); }
                magicBall.transform.position = chargeEffect.transform.position;
                magicBall.gameObject.SetActive(true);
                magicBall.OnInit(magicBallDamage, targetHero.position);
                targetHero = null;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                float minDis = 10000;
                HeroType heroType = TowerType == TowerType.PlayerTower ? HeroType.EnemyHero : HeroType.PlayerHero;
                List<HeroController> listTargetHero = PoolManager.Instance.FindActiveHeroes(heroType);
                foreach (HeroController heroController in listTargetHero)
                {
                    float range = Vector3.Distance(transform.position, heroController.transform.position);
                    if ((range < 15) && range < minDis)
                    {
                        minDis = range;
                        targetHero = heroController.transform;
                    }
                }
            }
            yield return null;
        }
    }



    /// <summary>
    /// Coroutine handle this magic tower is destroyed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRHandleTowerDestroyed()
    {
        yield return new WaitForSeconds(0.5f);
        fireBigRedEffect.gameObject.SetActive(true);
        fireBigRedEffect.Play();
    }
}
