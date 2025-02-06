using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerController : TowerController
{

    public override void OnInit(float health, float damage)
    {
        IsDead = false;
        towerAnim.gameObject.SetActive(false);
        towerMesh.gameObject.SetActive(true);
        fireBigRedEffect.gameObject.SetActive(false);
        towerDestroyEffect.gameObject.SetActive(false);
        CurrentHealth = health;
        TotalHealth = health;
    }

    public override void OnTakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, CurrentHealth);
        if(CurrentHealth <= 0)
        {
            IsDead = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.towerDestroyed);
            towerColider.enabled = false;
            towerMesh.gameObject.SetActive(false);
            towerDestroyEffect.gameObject.SetActive(true);
            towerDestroyEffect.Play();
            towerAnim.gameObject.SetActive(true);
            animator.Play(towerDestructionAnim.name);
            StartCoroutine(CRHandleTowerDestroyed());
        }    
    }

    public override void OnBeingBurned(float damagePerSec)
    {
        OnTakeDamage(damagePerSec);
    }



    /// <summary>
    /// Coroutine handle this main tower is destroyed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRHandleTowerDestroyed()
    {
        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            if (towerType == TowerType.PlayerTower)
            {
                IngameManager.Instance.LevelFailed();
            }
            else if (towerType == TowerType.EnemyTower)
            {
                IngameManager.Instance.LevelCompleted();
            }
        }
        yield return new WaitForSeconds(0.5f);
        fireBigRedEffect.gameObject.SetActive(true);
        fireBigRedEffect.Play();
    }
}
