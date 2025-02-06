using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer = new LayerMask();
    [SerializeField] private LayerMask playerLayer = new LayerMask();
    [SerializeField] private LayerMask towerEnemyLayer = new LayerMask();
    [SerializeField] private LayerMask towerPlayerLayer = new LayerMask();
    [SerializeField] private SpriteRenderer spawn = null;



    /// <summary>
    /// Check if the current position is able to spawn hero.
    /// </summary>
    /// <returns></returns>
    public bool CanSpawnHero()
    {
        spawn.gameObject.SetActive(true);
        if (Physics.CheckSphere(transform.position, 0.8f, enemyLayer | towerEnemyLayer | towerPlayerLayer | playerLayer))
        {
            spawn.color = Color.red;
            return false;
        }
        spawn.color = Color.white;
        return true;
    }
}
