 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonerHeroIdleController : HeroIdleController
{
    private PoisonerHeroController poisonerHeroController = null;
    public override void EnterIdleState()
    {
        base.EnterIdleState();
        poisonerHeroController = (PoisonerHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            poisonerHeroController.FindTarget();
            if (poisonerHeroController.TargetAttack != null && heroController.IsInRange(poisonerHeroController.TargetAttack.transform))
            {
                poisonerHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                poisonerHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { poisonerHeroController.SetBlendSpeed(0f, false); }
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
