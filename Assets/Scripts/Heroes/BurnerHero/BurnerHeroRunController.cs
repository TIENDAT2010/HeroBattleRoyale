using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BurnerHeroRunController : HeroRunController
{
    private BurnerHeroController burnerHeroController = null;
    private NavMeshPath navMeshPath = null;
    private NavMeshHit navMeshHit;
    private float movementY = 0;
    private float timer = 0f;

    public override void EnterRunState()
    {
        base.EnterRunState();
        burnerHeroController = (BurnerHeroController)heroController;

        if (IngameManager.Instance.GameState != GameState.GameStart)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }


        if (burnerHeroController.TargetAttack != null)
        {
            if (heroController.IsInRange(burnerHeroController.TargetAttack.transform) == false)
            {
                navMeshPath = new NavMeshPath();
                heroController.CapsuleCollider.enabled = false;
                heroController.CharacterController.enabled = true;
                heroController.NavMeshObstacle.enabled = false;
                burnerHeroController.SetBlendSpeed(1f, false);
            }
            else
            {
                burnerHeroController.SetBlendSpeed(0f, true);
                heroController.OnNextState(HeroState.Attack_State);
            }
        }
        else
        {
            heroController.OnNextState(HeroState.Idle_State);
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
            if (!burnerHeroController.TargetAttack.IsDead)
            {
                if (heroController.IsInRange(burnerHeroController.TargetAttack.transform))
                {
                    burnerHeroController.SetBlendSpeed(0f, true);
                    heroController.OnNextState(HeroState.Attack_State);
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        timer = 0.05f;
                        navMeshPath.ClearCorners();
                        if (NavMesh.SamplePosition(burnerHeroController.TargetAttack.transform.position, out navMeshHit, 5f, NavMesh.AllAreas))
                        {
                            NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                        }

                        burnerHeroController.FindTarget();
                    }


                    if (navMeshPath.corners.Length >= 2)
                    {
                        Vector3 posNextCorners = navMeshPath.corners[1];
                        posNextCorners.y = transform.position.y;
                        Vector3 targetDir = (posNextCorners - transform.position).normalized;
                        Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 7 * Time.deltaTime);


                        Vector3 movementDir = transform.forward;
                        if (!heroController.CharacterController.isGrounded) { movementY -= Time.deltaTime; }
                        else { movementY = 0f; }
                        movementDir.y = movementY;
                        heroController.CharacterController.Move(movementDir * burnerHeroController.SpeedRun * Time.deltaTime);
                    }
                }
            }
            else { burnerHeroController.OnNextState(HeroState.Idle_State); }
        }
    }
}
