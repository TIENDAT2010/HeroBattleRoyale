using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemHeroRunController : HeroRunController
{
    private GolemHeroController golemHeroController;
    private NavMeshPath navMeshPath = null;
    private NavMeshHit navMeshHit;
    private float movementY = 0;
    private float timer = 0f;

    public override void EnterRunState()
    {
        base.EnterRunState();
        golemHeroController = (GolemHeroController)heroController;

        if (IngameManager.Instance.GameState != GameState.GameStart)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            if (golemHeroController.TargetAttack != null)
            {
                if (heroController.IsInRange(golemHeroController.TargetAttack.transform) == false)
                {
                    navMeshPath = new NavMeshPath();
                    heroController.CapsuleCollider.enabled = false;
                    heroController.CharacterController.enabled = true;
                    heroController.NavMeshObstacle.enabled = false;
                    golemHeroController.SetBlendSpeed(1f, false);
                }
                else
                {
                    golemHeroController.SetBlendSpeed(0f, true);
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
    }


    private void Update()
    {
        if (IsEnterState)
        {
            if (!golemHeroController.TargetAttack.IsDead)
            {
                if (heroController.IsInRange(golemHeroController.TargetAttack.transform))
                {
                    golemHeroController.SetBlendSpeed(0f, true);
                    heroController.OnNextState(HeroState.Attack_State);
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        timer = 0.05f;
                        navMeshPath.ClearCorners();
                        if (NavMesh.SamplePosition(golemHeroController.TargetAttack.transform.position, out navMeshHit, 5f, NavMesh.AllAreas))
                        {
                            NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                        }

                        golemHeroController.FindTarget();
                    }


                    if (navMeshPath.corners.Length >= 2)
                    {
                        Vector3 posNextCorner = navMeshPath.corners[1];
                        posNextCorner.y = transform.position.y;
                        Vector3 targetDir = (posNextCorner - transform.position).normalized;
                        Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 7 * Time.deltaTime);


                        Vector3 movementDir = transform.forward;
                        if (!heroController.CharacterController.isGrounded) { movementY -= Time.deltaTime; }
                        else { movementY = 0f; }
                        movementDir.y = movementY;
                        heroController.CharacterController.Move(movementDir * golemHeroController.SpeedRun * Time.deltaTime);
                    }
                }
            }
            else { golemHeroController.OnNextState(HeroState.Idle_State); }
        }
    }
}
