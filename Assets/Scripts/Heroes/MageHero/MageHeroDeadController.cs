using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHeroDeadController : HeroDeadController
{
    [SerializeField] private AnimationClip animationClip = null;
    private MageHeroController mageHeroController = null;
    public override void EnterDeadState()
    {
        base.EnterDeadState();
        mageHeroController = (MageHeroController)heroController;
        heroController.Animator.SetBool("Dead", true);
        StartCoroutine(WaitAnimationDead());
        ViewManager.Instance.IngameView.OnEnemyHeroDead(transform.position, mageHeroController.CoinReward);
    }


    public override void ExitDeadState()
    {
        base.ExitDeadState();
    }


    private IEnumerator WaitAnimationDead()
    {
        heroController.CharacterController.enabled = false;
        heroController.CapsuleCollider.enabled = false;
        yield return new WaitForSeconds(animationClip.length + 2f);
        heroController.Animator.SetBool("Dead", false);
        heroController.CharacterController.enabled = true;
        heroController.CapsuleCollider.enabled = true;
        gameObject.SetActive(false);
    }    
}
