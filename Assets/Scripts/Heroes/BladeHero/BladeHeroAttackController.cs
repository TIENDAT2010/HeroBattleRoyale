using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BladeHeroAttackController : HeroAttackController
{
    [SerializeField] private List<Transform> collisionPoints;
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetTowerLayerMask = new LayerMask();

    private BladeHeroController bladeHeroController = null;
    private List<TargetController> listTargetTakeDamage = new List<TargetController>();
    private float collisionRadius = 0.5f;

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        bladeHeroController = (BladeHeroController)heroController;

        if (bladeHeroController.TargetAttack == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            listTargetTakeDamage.Clear();
            bladeHeroController.Animator.SetTrigger("Attack");
            StartCoroutine(CRWaitAndApplyDamage());
            StartCoroutine(CRWaitAndReturnIdle(attackClip));
            SoundManager.Instance.PlaySound(SoundManager.Instance.bladeHeroAttack);
        }
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }




    /// <summary>
    /// Coroutine check and apply damage for target.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRWaitAndApplyDamage()
    {
        while(IsEnterState && !bladeHeroController.TargetAttack.IsDead)
        {
            Vector3 targetPos = bladeHeroController.TargetAttack.transform.position;
            targetPos.y = transform.position.y;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            if (!targetDir.Equals(Vector3.zero))
            {
                Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 10 * Time.deltaTime);
            }

            foreach (Transform point in collisionPoints)
            {
                Collider[] hits = Physics.OverlapSphere(point.position, collisionRadius, targetHeroLayerMask | targetTowerLayerMask);
                foreach (Collider hit in hits)
                {
                    TargetController onTakeDameTarget = PoolManager.Instance.FindTarget(hit.transform);
                    if (onTakeDameTarget != null && listTargetTakeDamage.Contains(onTakeDameTarget) == false)
                    {
                        onTakeDameTarget.OnTakeDamage(bladeHeroController.Damage);
                        listTargetTakeDamage.Add(onTakeDameTarget);
                    }
                }
            }
            yield return null;
        }       
    }


    /// <summary>
    /// Coroutine wait and return to idel state.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    private IEnumerator CRWaitAndReturnIdle(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        bladeHeroController.OnNextState(HeroState.Idle_State);
    }
}
