using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBurnController : MonoBehaviour
{
    [SerializeField] private ParticleSystem trailEffect = null;
    [SerializeField] private ParticleSystem groundSlamEffect = null;
    [SerializeField] private Light explosionLight = null;
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetTowerLayerMask = new LayerMask();



    /// <summary>
    /// Init this fire burn.
    /// </summary>
    /// <param name="damagePerScond"></param>
    /// <param name="targetPos"></param>
    public void OnInit(float damagePerScond, Vector3 targetPos)
    {
        explosionLight.gameObject.SetActive(false);
        groundSlamEffect.gameObject.SetActive(false);
        trailEffect.gameObject.SetActive(true);
        trailEffect.Play();
        StartCoroutine(CRMoveToPos(targetPos, damagePerScond));
    }



    /// <summary>
    /// Calculate position for movement.
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
    /// Coroutine move to the target position with damage
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="damagePerSec"></param>
    /// <returns></returns>
    private IEnumerator CRMoveToPos(Vector3 targetPos, float damagePerSec)
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

        SoundManager.Instance.PlaySound(SoundManager.Instance.fireBurnExplode);

        trailEffect.Stop();
        trailEffect.gameObject.SetActive(false);
        groundSlamEffect.gameObject.SetActive(true);
        groundSlamEffect.Play();
        StartCoroutine(CRPlayLightEffect());
        yield return new WaitForSeconds(0.2f);

        //Apply damage
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f, targetHeroLayerMask | targetTowerLayerMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                TargetController target = PoolManager.Instance.FindTarget(hits[i].transform);
                if (target != null)
                {
                    target.OnBeingBurned(damagePerSec);
                }
            }
        }

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Coroutine play the light effect.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRPlayLightEffect()
    {
        explosionLight.gameObject.SetActive(true);
        float original = explosionLight.intensity;
        float intensity = explosionLight.intensity;
        while (intensity > 0f)
        {
            intensity = Mathf.Clamp(intensity - Time.deltaTime * 5f, 0f, original);
            explosionLight.intensity = intensity;
            yield return null;
        }
        explosionLight.intensity = original;
        explosionLight.gameObject.SetActive(false);
    }
}
