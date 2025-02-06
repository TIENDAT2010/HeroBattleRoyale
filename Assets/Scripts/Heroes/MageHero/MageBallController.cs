using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBallController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireballEffect = null;
    [SerializeField] private ParticleSystem explosionEffect = null;
    [SerializeField] private Light explosionLight = null;
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetTowerLayerMask = new LayerMask();


    /// <summary>
    /// Init this mage ball.
    /// </summary>
    /// <param name="damage"></param>
    public void OnInit(float damage)
    {
        explosionLight.gameObject.SetActive(false);
        explosionEffect.gameObject.SetActive(false);
        fireballEffect.gameObject.SetActive(true);
        fireballEffect.Play();
        StartCoroutine(CRMoveForward(damage));
    }


    /// <summary>
    /// Coroutine move forward with damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    private IEnumerator CRMoveForward(float damage)
    {
        while (gameObject.activeSelf)
        {
            transform.position += transform.forward * 6f * Time.deltaTime;
            yield return null;
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f, targetHeroLayerMask | targetTowerLayerMask);
            if (hits.Length > 0)
            {
                TargetController onTakeDameTarget = PoolManager.Instance.FindTarget(hits[0].transform);
                if (onTakeDameTarget != null)
                {
                    onTakeDameTarget.OnTakeDamage(damage);
                }
                break;
            }
        }

        fireballEffect.gameObject.SetActive(false);
        explosionEffect.gameObject.SetActive(true);
        explosionLight.gameObject.SetActive(true);
        explosionEffect.Play();
        SoundManager.Instance.PlaySound(SoundManager.Instance.mageBallExplode);
        yield return new WaitForSeconds(0.5f);

        float original = explosionLight.intensity;
        float intensity = explosionLight.intensity;
        while (intensity > 0f)
        {
            intensity = Mathf.Clamp(intensity - Time.deltaTime * 5f, 0f, original);
            explosionLight.intensity = intensity;
            yield return null;
        }
        explosionLight.intensity = original;
        explosionEffect.gameObject.SetActive(false);
        explosionLight.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
