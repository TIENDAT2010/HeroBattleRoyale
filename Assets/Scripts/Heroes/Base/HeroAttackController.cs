using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour
{
    [SerializeField] protected HeroController heroController = null;
    [SerializeField] protected AnimationClip attackClip = null;

    protected bool IsEnterState = false;

    /// <summary>
    /// Enter Attack state of the hero.
    /// </summary>
    public virtual void EnterAttackState() { IsEnterState = true; }

    /// <summary>
    /// Exit Attack state of the hero.
    /// </summary>
    public virtual void ExitAttackState() { IsEnterState = false; }
}

