using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHeroIdleController : HeroIdleController
{
    private BowHeroController bowHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        bowHeroController = (BowHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            bowHeroController.FindTarget();
            if (bowHeroController.TargetAttack != null && heroController.IsInRange(bowHeroController.TargetAttack.transform))
            {
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                bowHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { bowHeroController.SetBlendSpeed(0f, false); }
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
