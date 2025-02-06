using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHeroIdleController : HeroIdleController
{
    private SwordHeroController swordHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        swordHeroController = (SwordHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            swordHeroController.FindTarget();
            if (swordHeroController.TargetAttack != null && heroController.IsInRange(swordHeroController.TargetAttack.transform))
            {
                swordHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                swordHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { swordHeroController.SetBlendSpeed(0f, false); }
    }


    public override void ExitHeroIdle()
    {
        base.ExitHeroIdle();
        StopAllCoroutines();
    }


    /// <summary>
    /// Coroutine wait and goto run state.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRWaitForRunState()
    {
        yield return new WaitForSeconds(clip.length);
        heroController.OnNextState(HeroState.Run_State);
    }
}
