using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHeroAttackController : HeroAttackController
{
    private MageHeroController mageHeroController = null;

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        mageHeroController = (MageHeroController)heroController;

        if (mageHeroController.TargetAttack == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            mageHeroController.ChargeEffect.gameObject.SetActive(false);
            heroController.Animator.SetTrigger("Attack");
            StartCoroutine(CRHandleAttack());
            SoundManager.Instance.PlaySound(SoundManager.Instance.mageHeroAttack);
        } 
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }


    private IEnumerator CRHandleAttack()
    {
        float timeCount = 0;
        while (true)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= 0.15f && !mageHeroController.ChargeEffect.gameObject.activeSelf)
            {
                mageHeroController.ChargeEffect.gameObject.SetActive(true);
                mageHeroController.ChargeEffect.Play();
            }

            if (timeCount >= 1.2f && mageHeroController.ChargeEffect.gameObject.activeSelf)
            {
                mageHeroController.ChargeEffect.gameObject.SetActive(false);
                break;
            }

            yield return null;
        }

        MageBallController calenFireball = mageHeroController.GetMageBallController();
        calenFireball.transform.position = mageHeroController.FireballPos.position;
        calenFireball.transform.forward = mageHeroController.FireballPos.forward;
        calenFireball.gameObject.SetActive(true);
        calenFireball.OnInit(mageHeroController.Damage);

        yield return new WaitForSeconds(1f);
        heroController.OnNextState(HeroState.Idle_State);
    }


    private void Update()
    {
        if (IsEnterState && mageHeroController.TargetAttack != null)
        {
            Vector3 targetPos = mageHeroController.TargetAttack.transform.position;
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
