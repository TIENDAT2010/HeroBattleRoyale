using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRunController : MonoBehaviour
{
    [SerializeField] protected HeroController heroController = null;


    protected bool IsEnterState = false;

    
    /// <summary>
    /// Enter run state of the hero.
    /// </summary>
    public virtual void EnterRunState() { IsEnterState = true; }


    /// <summary>
    /// Exit run state of the hero.
    /// </summary>
    public virtual void ExitRunState() { IsEnterState = false; }
}

