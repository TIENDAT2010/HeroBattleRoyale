using UnityEngine;
using System.Collections;

public class TowerArrowController : MonoBehaviour
{
    
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private LayerMask terrainLayer = new LayerMask();
    [SerializeField] private ParticleSystem explosionEffect = null;
    [SerializeField] private ParticleSystem trailEffect = null;
    [SerializeField] private Transform arrowObject = null;
    

    /// <summary>
    /// Init this tower arrow with damage.
    /// </summary>
    /// <param name="dg"></param>
    public void OnInit(float dg)
    {
        arrowObject.gameObject.SetActive(true);
        trailEffect.gameObject.SetActive(true);
        explosionEffect.gameObject.SetActive(false);
        trailEffect.Play();
        StartCoroutine(CRMoveForward(dg));
    }



    /// <summary>
    /// Coroutine move this arrow along its forward direction.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRMoveForward(float damage)
    {
        RaycastHit hit;
        while (gameObject.activeSelf)
        {
            transform.position += transform.forward * 75f * Time.deltaTime;
            yield return null;

            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.15f, out hit, 1.2f, targetHeroLayerMask | terrainLayer))
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.archerArrowExplode);
                TargetController onTakeDameTarget = PoolManager.Instance.FindTarget(hit.collider.transform);
                if (onTakeDameTarget != null)
                {
                    onTakeDameTarget.OnTakeDamage(damage);
                }
                break;
            }

            Vector3 bosPos = Camera.main.WorldToViewportPoint(transform.position);
            if (bosPos.x < -0.5f || bosPos.x > 1.5f || bosPos.y < -0.5f || bosPos.y > 1.5f)
            {
                gameObject.SetActive(false);
            }
        }

        arrowObject.gameObject.SetActive(false);
        trailEffect.gameObject.SetActive(false);
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.Play();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
