using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerHeroAttackController : HeroAttackController
{
    private BurnerHeroController burnerHeroController = null;

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        burnerHeroController = (BurnerHeroController)heroController;

        if (burnerHeroController.TargetAttack == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            StartCoroutine(CRHandleAttack());
        }    
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }


    private IEnumerator CRHandleAttack()
    {
        burnerHeroController.ChargeEffect.gameObject.SetActive(true);
        burnerHeroController.ChargeEffect.Play();
        yield return new WaitForSeconds(0.5f);
        heroController.Animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.7f);
        burnerHeroController.ChargeEffect.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.Instance.burnerHeroAttack);

        SoundManager.Instance.PlaySound(SoundManager.Instance.burnerHeroAttack);
        FireBurnController fireBurnController = burnerHeroController.GetFireBurnController();
        fireBurnController.transform.position = burnerHeroController.FireBurnPos.position;
        fireBurnController.transform.forward = burnerHeroController.FireBurnPos.forward;
        fireBurnController.gameObject.SetActive(true);
        fireBurnController.OnInit(burnerHeroController.DamagePerSecond, burnerHeroController.TargetAttack.transform.position);

        yield return new WaitForSeconds(2f);
        heroController.OnNextState(HeroState.Idle_State);
    }


    private void Update()
    {
        if (IsEnterState && burnerHeroController.TargetAttack != null)
        {
            Vector3 targetPos = burnerHeroController.TargetAttack.transform.position;
            targetPos.y = transform.position.y;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            if (!targetDir.Equals(Vector3.zero))
            {
                Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 10 * Time.deltaTime);
            }
        }

    }
}
