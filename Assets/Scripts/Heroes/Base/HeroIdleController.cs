using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIdleController : MonoBehaviour
{
    [SerializeField] protected HeroController heroController = null;
    [SerializeField] protected AnimationClip clip = null;

    protected bool IsEnterState = false;


    /// <summary>
    /// Enter Idle state of the hero.
    /// </summary>
    public virtual void EnterIdleState() { IsEnterState = true; }


    /// <summary>
    /// Exit Idle state of the hero.
    /// </summary>
    public virtual void ExitHeroIdle() { IsEnterState = false; }


}

