using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeHeroIdleController : HeroIdleController
{
    private BladeHeroController bladeHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        bladeHeroController = (BladeHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            bladeHeroController.FindTarget();
            if (bladeHeroController.TargetAttack != null && heroController.IsInRange(bladeHeroController.TargetAttack.transform))
            {
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { bladeHeroController.SetBlendSpeed(0f, false); }
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
