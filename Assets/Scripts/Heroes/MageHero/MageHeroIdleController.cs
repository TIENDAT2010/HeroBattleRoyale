 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHeroIdleController : HeroIdleController
{
    private MageHeroController mageHeroController = null;

    public override void EnterIdleState()
    {
        base.EnterIdleState();
        mageHeroController = (MageHeroController)heroController;

        if (IngameManager.Instance.GameState == GameState.GameStart)
        {
            mageHeroController.FindTarget();
            if (mageHeroController.TargetAttack != null && heroController.IsInRange(mageHeroController.TargetAttack.transform))
            {
                mageHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
            else
            {
                heroController.CapsuleCollider.enabled = true;
                heroController.CharacterController.enabled = false;
                heroController.NavMeshObstacle.enabled = true;
                mageHeroController.SetBlendSpeed(0f, false);
                StartCoroutine(CRWaitForRunState());
            }
        }
        else { mageHeroController.SetBlendSpeed(0f, false); }
    }

    public override void ExitHeroIdle()
    {
        StopAllCoroutines();
        base.ExitHeroIdle();
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
