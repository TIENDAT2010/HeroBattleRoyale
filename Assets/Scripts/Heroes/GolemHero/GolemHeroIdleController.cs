using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHeroIdleController : HeroIdleController
{
    private GolemHeroController golemHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        golemHeroController = (GolemHeroController)heroController;


        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            golemHeroController.FindTarget();
            if (golemHeroController.TargetAttack != null && heroController.IsInRange(golemHeroController.TargetAttack.transform))
            {
                golemHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                golemHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { golemHeroController.SetBlendSpeed(0f, false); }
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
