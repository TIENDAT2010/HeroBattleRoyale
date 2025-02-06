using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class GolemHeroAttackController : HeroAttackController
{
    private GolemHeroController golemHeroController = null;
    [SerializeField] private LayerMask targetHeroLayerMask = new LayerMask();
    [SerializeField] private LayerMask targetTowerLayerMask = new LayerMask();
    
    private List<TargetController> listTargetTakeDamage = new List<TargetController>();

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        golemHeroController = (GolemHeroController)heroController;


        if (golemHeroController.TargetAttack == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            listTargetTakeDamage.Clear();
            golemHeroController.Animator.SetTrigger("Attack");
            StartCoroutine(CROnAttack());
        }
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }


    private void Update()
    {
        if (IsEnterState && golemHeroController.TargetAttack != null)
        {
            Vector3 targetPos = golemHeroController.TargetAttack.transform.position;
            targetPos.y = transform.position.y;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            if(!targetDir.Equals(Vector3.zero))
            {
                Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 10 * Time.deltaTime);
            }
        }

    }


    private IEnumerator CROnAttack()
    {
        yield return new WaitForSeconds(0.4f);
        golemHeroController.AttackEffect.gameObject.SetActive(true);
        golemHeroController.AttackEffect.Play();

        Vector3 halfExtents = new Vector3(0.5f, 0.5f, 7.5f);
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, halfExtents, transform.forward, Quaternion.identity, 7.5f, targetTowerLayerMask | targetHeroLayerMask);
        foreach (RaycastHit hit in hits)
        {
            TargetController onTakeDameTarget = PoolManager.Instance.FindTarget(hit.transform);
            if (onTakeDameTarget != null && listTargetTakeDamage.Contains(onTakeDameTarget) == false)
            {
                onTakeDameTarget.OnTakeDamage(golemHeroController.Damage);
                listTargetTakeDamage.Add(onTakeDameTarget);
            }
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        golemHeroController.AttackEffect.gameObject.SetActive(false);
        golemHeroController.OnNextState(HeroState.Idle_State);
    }    
}
