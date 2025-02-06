using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDeadController : MonoBehaviour
{
    [SerializeField] protected HeroController heroController = null;

    protected bool IsEnterState = false;


    /// <summary>
    /// Enter Dead state of the hero.
    /// </summary>
    public virtual void EnterDeadState() { IsEnterState = true; SoundManager.Instance.PlaySound(SoundManager.Instance.heroDead); }


    /// <summary>
    /// Exit Dead state of the hero.
    /// </summary>
    public virtual void ExitDeadState() { IsEnterState = false; }
}

