using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArcherTowerController : TowerController
{

    [SerializeField] protected GameObject weaponTower = null;
    [SerializeField] private GameObject arrow = null;
    [SerializeField] private TowerArrowController arrowPrefab = null;

    private Transform targetHero = null;
    private List<TowerArrowController> listTowerArrow = new List<TowerArrowController>();
    private float arrowDamage = 0;

    public override void OnInit(float health, float damage)
    {
        towerAnim.gameObject.SetActive(false);
        towerMesh.gameObject.SetActive(true);
        fireBigRedEffect.gameObject.SetActive(false);
        towerDestroyEffect.gameObject.SetActive(false);
        CurrentHealth = health;
        arrowDamage = damage;
        StartCoroutine(CRRotateToHero());
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
    /// Coroutine rotate to hero.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRRotateToHero()
    {
        while(IsDead == false)
        {
            if(targetHero != null)
            {
                float t = 0;
                float moveTime = 0.5f;
                Vector3 startForward = weaponTower.transform.forward;
                Vector3 targetPos = targetHero.transform.position;
                targetPos.y += 0.1f;
                Vector3 endForward = targetPos - weaponTower.transform.position;
                while (t < moveTime)
                {
                    t += Time.deltaTime;
                    float factor = t / moveTime;
                    weaponTower.transform.forward = Vector3.Lerp(startForward, endForward.normalized, factor);
                    yield return null;
                }
                StartCoroutine(CRAttackHero());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                float minDis = 10000;
                HeroType heroType = TowerType == TowerType.PlayerTower ? HeroType.EnemyHero : HeroType.PlayerHero;
                List<HeroController> listTargetHero = PoolManager.Instance.FindActiveHeroes(heroType);
                foreach(HeroController heroController in listTargetHero)
                {
                    float range = Vector3.Distance(transform.position, heroController.transform.position);
                    if ((range > 5 && range < 15) && range<minDis)
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
    /// Coroutine attack the hero.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRAttackHero()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.archerTowerAttack);
        arrow.SetActive(false);

        TowerArrowController arrowTower = listTowerArrow.Where(a => !a.gameObject.activeSelf).FirstOrDefault();
        if(arrowTower == null) { arrowTower = Instantiate(arrowPrefab, arrow.transform.position, Quaternion.identity); }
        arrowTower.transform.position = arrowTower.transform.position;
        arrowTower.transform.forward = weaponTower.transform.forward;
        arrowTower.gameObject.SetActive(true);
        arrowTower.OnInit(arrowDamage);
        targetHero = null;
        yield return new WaitForSeconds(1f);
        arrow.SetActive(true);
    }



    /// <summary>
    /// Coroutine handle this archer tower is destroyed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRHandleTowerDestroyed()
    {
        yield return new WaitForSeconds(0.5f);
        fireBigRedEffect.gameObject.SetActive(true);
        fireBigRedEffect.Play();
    }

}
