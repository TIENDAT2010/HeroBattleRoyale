 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerHeroIdleController : HeroIdleController
{
    private BurnerHeroController burnerHeroController = null;
    public override void EnterIdleState()
    {
        base.EnterIdleState();
        burnerHeroController = (BurnerHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            burnerHeroController.FindTarget();
            if (burnerHeroController.TargetAttack != null && heroController.IsInRange(burnerHeroController.TargetAttack.transform))
            {
                burnerHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                burnerHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { burnerHeroController.SetBlendSpeed(0f, false); }
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
