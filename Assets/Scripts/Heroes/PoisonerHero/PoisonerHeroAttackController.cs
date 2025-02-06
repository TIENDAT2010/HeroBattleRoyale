using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonerHeroAttackController : HeroAttackController
{
    private PoisonerHeroController poisonerHeroController = null;

    public override void EnterAttackState()
    {
        base.EnterAttackState();
        poisonerHeroController = (PoisonerHeroController)heroController;

        if(poisonerHeroController.TargetAttack == null)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            heroController.CapsuleCollider.enabled = true;
            heroController.CharacterController.enabled = false;
            heroController.NavMeshObstacle.enabled = true;
            heroController.Animator.SetTrigger("Attack");
            StartCoroutine(CRPoisonSpawn());
            
        }    
    }


    public override void ExitAttackState()
    {
        StopAllCoroutines();
        base.ExitAttackState();
    }


    private IEnumerator CRPoisonSpawn()
    {
        poisonerHeroController.ChannelEffect.gameObject.SetActive(true);
        poisonerHeroController.ChannelEffect.Play();
        SoundManager.Instance.PlaySound(SoundManager.Instance.poisonZoneSpawn);
        PoisonZoneController poisonZone = poisonerHeroController.GetPoisonZoneController();
        poisonZone.transform.position = poisonerHeroController.TargetAttack.transform.position;
        poisonZone.gameObject.SetActive(true);
        poisonZone.OnInit(poisonerHeroController.DamagePerSecond);

        yield return new WaitForSeconds(2f);
        poisonerHeroController.ChannelEffect.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        heroController.OnNextState(HeroState.Idle_State);
    }


    private void Update()
    {
        if (IsEnterState && poisonerHeroController.TargetAttack != null)
        {
            Vector3 targetPos = poisonerHeroController.TargetAttack.transform.position;
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
