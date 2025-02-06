 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerHeroIdleController : HeroIdleController
{
    private HealerHeroController healerHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        healerHeroController = (HealerHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            healerHeroController.FindTarget();
            if (healerHeroController.TargetHero != null && heroController.IsInRange(healerHeroController.TargetHero.transform))
            {
                healerHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                healerHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { healerHeroController.SetBlendSpeed(0f, false); }
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
