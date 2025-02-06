using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnifeHeroRunController : HeroRunController
{
    private KnifeHeroController knifeHeroController = null;
    private NavMeshPath navMeshPath = null;
    private NavMeshHit navMeshHit;
    private float timer = 0f;
    private float MovementY = 0f;

    public override void EnterRunState()
    {
        base.EnterRunState();
        knifeHeroController = (KnifeHeroController)heroController;

        if (IngameManager.Instance.GameState != GameState.GameStart)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            if (knifeHeroController.TargetAttack != null)
            {
                if (heroController.IsInRange(knifeHeroController.TargetAttack.transform) == false)
                {
                    navMeshPath = new NavMeshPath();
                    heroController.CapsuleCollider.enabled = false;
                    heroController.CharacterController.enabled = true;
                    heroController.NavMeshObstacle.enabled = false;
                    knifeHeroController.SetBlendSpeed(1f, false);
                }
                else
                {
                    knifeHeroController.SetBlendSpeed(0f, true);
                    heroController.OnNextState(HeroState.Attack_State);
                }
            }
            else
            {
                heroController.OnNextState(HeroState.Idle_State);
            }
        }
    }

    public override void ExitRunState()
    {
        timer = 0f;
        base.ExitRunState();
        StopAllCoroutines();
    }


    private void Update()
    {
        if (IsEnterState)
        {
            if (!knifeHeroController.TargetAttack.IsDead)
            {
                if (heroController.IsInRange(knifeHeroController.TargetAttack.transform))
                {
                    knifeHeroController.SetBlendSpeed(0f, true);
                    heroController.OnNextState(HeroState.Attack_State);
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        timer = 0.05f;
                        navMeshPath.ClearCorners();
                        if (NavMesh.SamplePosition(knifeHeroController.TargetAttack.transform.position, out navMeshHit, 5f, NavMesh.AllAreas))
                        {
                            NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                        }

                        knifeHeroController.FindTarget();
                    }


                    if (navMeshPath.corners.Length >= 2)
                    {
                        Vector3 posNextCorner = navMeshPath.corners[1];
                        posNextCorner.y = transform.position.y;
                        Vector3 targetDir = (posNextCorner - transform.position).normalized;
                        Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 7 * Time.deltaTime);


                        Vector3 movementDir = transform.forward;
                        if (!heroController.CharacterController.isGrounded) { MovementY -= Time.deltaTime; }
                        else { MovementY = 0f; }
                        movementDir.y = MovementY;
                        heroController.CharacterController.Move(movementDir * knifeHeroController.SpeedRun * Time.deltaTime);
                    }
                }
            }
            else { knifeHeroController.OnNextState(HeroState.Idle_State); }
        }
    }
}
