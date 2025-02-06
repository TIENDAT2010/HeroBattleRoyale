using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{

    /// <summary>
    /// Determine this target id dead.
    /// </summary>
    public bool IsDead { get; protected set; }


    /// <summary>
    /// Handle this target take damage.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void OnTakeDamage(float damage) { }



    public virtual void OnReceiveHealth(float hp) { }


    public virtual void OnBeingPoisoned(float damagePerSec) { }


    public virtual void OnBeingBurned(float damagePerSec) { }
}
