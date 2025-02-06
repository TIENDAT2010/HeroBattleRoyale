using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealZoneController : MonoBehaviour
{
    [SerializeField] private ParticleSystem buffEffect = null;
    [SerializeField] private LayerMask playerHeroLayerMask = new LayerMask();


    /// <summary>
    /// Init this heal zone.
    /// </summary>
    /// <param name="healAmount"></param>
    public void OnInit(float healAmount)
    {
        buffEffect.gameObject.SetActive(true);
        buffEffect.Play();
        StartCoroutine(CRHealZone(healAmount));
    }


    /// <summary>
    /// Coroutine handle heal zone effect.
    /// </summary>
    /// <param name="healAmount"></param>
    /// <returns></returns>
    private IEnumerator CRHealZone(float healAmount)
    {
        for (int i = 0; i < 5; i++)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 3f, playerHeroLayerMask);
            foreach (Collider hit in hits)
            {
                TargetController target = PoolManager.Instance.FindTarget(hit.transform);
                if (target != null) { target.OnReceiveHealth(healAmount); }
            }
            yield return new WaitForSeconds(1f);
        }

        ParticleSystem.EmissionModule emission = buffEffect.emission;
        emission.enabled = false;
        yield return new WaitForSeconds(3f);
        emission.enabled = true;
        buffEffect.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
