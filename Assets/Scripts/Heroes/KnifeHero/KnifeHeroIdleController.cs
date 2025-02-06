using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHeroIdleController : HeroIdleController
{
    private KnifeHeroController knifeHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        knifeHeroController = (KnifeHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            knifeHeroController.FindTarget();
            if (knifeHeroController.TargetAttack != null && heroController.IsInRange(knifeHeroController.TargetAttack.transform))
            {
                knifeHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                knifeHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { knifeHeroController.SetBlendSpeed(0f, false); }
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
