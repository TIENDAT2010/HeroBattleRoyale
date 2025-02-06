using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallController : MonoBehaviour
{
    [SerializeField] private float collisionRadius = 4f;
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private ParticleSystem trailEffect = null;
    [SerializeField] private ParticleSystem groundSlamEffect = null;
    [SerializeField] private Light explosionLight = null;

    private float damageMagicBall = 0f;
    public void OnInit(float damage, Vector3 targetPos)
    {
        damageMagicBall = damage;
        groundSlamEffect.gameObject.SetActive(false);
        trailEffect.gameObject.SetActive(true);
        trailEffect.Play();
        StartCoroutine(CRMoveToPos(targetPos));
    }

    /// <summary>
    /// Calculate position for player movement.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="from"></param>
    /// <param name="middle"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 from, Vector3 middle, Vector3 to)
    {
        return Mathf.Pow((1 - t), 2) * from + 2 * (1 - t) * t * middle + Mathf.Pow(t, 2) * to;
    }

    /// <summary>
    /// Coroutine move this target bullet to the given position.
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator CRMoveToPos(Vector3 targetPos)
    {
        List<Vector3> listPositions = new List<Vector3>();

        //Calculate the list position
        int movePoints = 40;
        Vector3 startPoint = transform.position;
        Vector3 midPoint = Vector3.Lerp(startPoint, targetPos, 0.5f) + Vector3.up * 6;
        listPositions.Add(transform.position);
        for (int i = 1; i <= movePoints; i++)
        {
            float t = i / (float)movePoints;
            listPositions.Add(CalculateQuadraticBezierPoint(t, startPoint, midPoint, targetPos));
        }

        //Moving player to each point
        for (int i = 0; i < listPositions.Count; i++)
        {
            transform.position = listPositions[i];
            yield return null;
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.magicBallExplode);

        trailEffect.Pause();
        trailEffect.gameObject.SetActive(false);
        groundSlamEffect.gameObject.SetActive(true);
        groundSlamEffect.Play();
        StartCoroutine(CRLightEffect());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CRApplyDamage());
        yield return null;
    }


    /// <summary>
    /// Coroutine check and apply damage to heroes.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRApplyDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, collisionRadius, targetHeroLayerMask);
        if (hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                TargetController onTakeDameTarget = PoolManager.Instance.FindTarget(hits[i].transform);
                if (onTakeDameTarget != null)
                {
                    onTakeDameTarget.OnTakeDamage(damageMagicBall);
                }
            }
        }
        yield return null;
    }


    /// <summary>
    /// Coroutine handle the light effect.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRLightEffect()
    {
        float original = explosionLight.intensity;
        float intensity = explosionLight.intensity;
        while (intensity > 0f)
        {
            intensity = Mathf.Clamp(intensity - Time.deltaTime * 5f, 0f, original);
            explosionLight.intensity = intensity;
            yield return null;
        }
    }
}
