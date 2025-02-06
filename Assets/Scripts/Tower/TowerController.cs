using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : TargetController
{
    [SerializeField] protected string towerName = string.Empty;
    [SerializeField] protected TowerType towerType = TowerType.PlayerTower;
    [SerializeField] protected GameObject towerMesh = null;
    [SerializeField] protected GameObject towerAnim = null;
    [SerializeField] protected AnimationClip towerDestructionAnim = null;
    [SerializeField] protected Animator animator = null;
    [SerializeField] protected ParticleSystem fireBigRedEffect = null;
    [SerializeField] protected ParticleSystem towerDestroyEffect = null;
    [SerializeField] protected Collider towerColider = null;


    public TowerType TowerType => towerType;
    public string TowerName => towerName;
    public float TotalHealth { get; protected set; }
    public float CurrentHealth { get; protected set; }


    /// <summary>
    /// Handle init this tower.
    /// </summary>
    /// <param name="health"></param>
    /// <param name="damage"></param>
    public virtual void OnInit(float health, float damage) { }


    /// <summary>
    /// Handle this tower take damage.
    /// </summary>
    /// <param name="damage"></param>
    public override void OnTakeDamage(float damage) { }


    /// <summary>
    /// Handle this tower being burned.
    /// </summary>
    /// <param name="damagePerSec"></param>
    public override void OnBeingBurned(float damagePerSec) { }

}
