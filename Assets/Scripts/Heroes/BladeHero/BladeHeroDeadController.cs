using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeHeroDeadController : HeroDeadController
{
    [SerializeField] private AnimationClip animationClip = null;
    private BladeHeroController bladeHeroController = null;
    public override void EnterDeadState()
    {
        base.EnterDeadState();
        bladeHeroController = (BladeHeroController)heroController;
        heroController.Animator.SetBool("Dead", true);
        StartCoroutine(WaitAnimationDead());
        ViewManager.Instance.IngameView.OnEnemyHeroDead(transform.position, bladeHeroController.CoinReward);
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
