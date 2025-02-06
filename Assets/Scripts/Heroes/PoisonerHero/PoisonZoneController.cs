using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonZoneController : MonoBehaviour
{
    [SerializeField] private ParticleSystem poisonEffect = null;

    [SerializeField] private float collisionRadius = 1.5f;
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();


    /// <summary>
    /// Init this poison zone.
    /// </summary>
    /// <param name="damagePerSecond"></param>
    public void OnInit(float damagePerSecond)
    {
        poisonEffect.gameObject.SetActive(true);
        poisonEffect.Play();
        StartCoroutine(CRPoisonZone(damagePerSecond));
    }


    /// <summary>
    /// Coroutine handle poison zone effect.
    /// </summary>
    /// <param name="damagePerSec"></param>
    /// <returns></returns>
    private IEnumerator CRPoisonZone(float damagePerSec)
    {
        for (int i = 0; i < 50; i++)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, collisionRadius, targetHeroLayerMask);
            foreach (Collider hit in hits)
            {
                TargetController target = PoolManager.Instance.FindTarget(hit.transform);
                if(target != null) { target.OnBeingPoisoned(damagePerSec / 10f); }  
            }
            yield return new WaitForSeconds(0.1f);
        }
        ParticleSystem.EmissionModule emission = poisonEffect.emission;
        emission.enabled = false;
        yield return new WaitForSeconds(3f);
        emission.enabled = true;
        poisonEffect.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
