using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHeroAttackController : HeroAttackController
{
    private BowHeroController bowHeroController = null;

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        bowHeroController = (BowHeroController)heroController;


        if (bowHeroController.TargetAttack == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            bowHeroController.ArrowHolder.SetActive(false);
            heroController.Animator.SetTrigger("GetArrow");
            StartCoroutine(CRGetArrow());
        }
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }



    private void Update()
    {
        if(IsEnterState && bowHeroController.TargetAttack != null)
        {        
            Vector3 targetDir = (bowHeroController.TargetAttack.transform.position - transform.position).normalized;
            Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 7 * Time.deltaTime);
        }
    }


    private IEnumerator CRGetArrow()
    {
        yield return new WaitForSeconds(0.2f);
        bowHeroController.ArrowHolder.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        heroController.Animator.SetTrigger("Attack");
        StartCoroutine(CRBowShotAttack());
    }



    private IEnumerator CRBowShotAttack()
    {
        yield return new WaitForSeconds(1.7f);
        bowHeroController.ArrowHolder.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.Instance.bowHeroAttack);
        ArrowController arrow = bowHeroController.GetArrowController();
        arrow.transform.position = bowHeroController.ArrowPos.position;
        arrow.transform.forward = bowHeroController.ArrowPos.forward;
        arrow.gameObject.SetActive(true);
        arrow.OnInit(bowHeroController.Damage);

        yield return new WaitForSeconds(1f);
        heroController.OnNextState(HeroState.Idle_State);
    }
}
