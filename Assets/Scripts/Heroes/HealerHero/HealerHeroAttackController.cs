using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerHeroAttackController : HeroAttackController
{
    private HealerHeroController healerHeroController = null;

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        healerHeroController = (HealerHeroController)heroController;

        if (healerHeroController.TargetHero == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            heroController.Animator.SetTrigger("Attack");
            StartCoroutine(CRHealForHero());
        }
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }


    private IEnumerator CRHealForHero()
    {
        healerHeroController.ChannelEffect.gameObject.SetActive(true);
        healerHeroController.ChannelEffect.Play();
        SoundManager.Instance.PlaySound(SoundManager.Instance.healZoneSpawn);
        HealZoneController healZone = healerHeroController.GetHealZoneController();
        healZone.transform.position = healerHeroController.TargetHero.transform.position;
        healZone.gameObject.SetActive(true);
        healZone.OnInit(healerHeroController.HealPerSecond);

        yield return new WaitForSeconds(1f);
        healerHeroController.ChannelEffect.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        heroController.OnNextState(HeroState.Idle_State);
    }


    private void Update()
    {
        if (IsEnterState && healerHeroController.TargetHero != null)
        {
            Vector3 targetPos = healerHeroController.TargetHero.transform.position;
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
