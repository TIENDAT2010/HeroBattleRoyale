using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetTowerLayerMask = new LayerMask();
    [SerializeField] private Transform collisionPos = null;

    private float damage = 0f;
    private float collisionRadius = 0.15f;

    public void OnInit(float dg)
    {
        damage = dg;
        StartCoroutine(CRMoveForward());
    }    


    private IEnumerator CRMoveForward()
    {
        while(gameObject.activeSelf)
        {
            transform.position += transform.forward * 25f * Time.deltaTime;
            yield return null;


            Collider[] hits = Physics.OverlapSphere(collisionPos.position, collisionRadius, targetHeroLayerMask | targetTowerLayerMask);
            if (hits.Length > 0)
            {
                TargetController onTakeDameTarget = PoolManager.Instance.FindTarget(hits[0].transform);
                if (onTakeDameTarget != null)
                {
                    onTakeDameTarget.OnTakeDamage(damage);
                    gameObject.SetActive(false);
                }
                break;
            }


            Vector3 bosPos = Camera.main.WorldToViewportPoint(transform.position);
            if(bosPos.x < -0.5f || bosPos.x > 1.5f || bosPos.y < -0.5f || bosPos.y > 1.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
