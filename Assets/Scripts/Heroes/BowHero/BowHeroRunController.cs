using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BowHeroRunController : HeroRunController
{
    private BowHeroController bowHeroController = null;
    private NavMeshPath navMeshPath = null;
    private NavMeshHit navMeshHit;
    private float MovementY = 0f;
    private float timer = 0f;

    public override void EnterRunState()
    {
        base.EnterRunState();
        bowHeroController = (BowHeroController)heroController;

        if (IngameManager.Instance.GameState != GameState.GameStart)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            if (bowHeroController.TargetAttack != null)
            {
                if (heroController.IsInRange(bowHeroController.TargetAttack.transform) == false)
                {
                    navMeshPath = new NavMeshPath();
                    heroController.CapsuleCollider.enabled = false;
                    heroController.CharacterController.enabled = true;
                    heroController.NavMeshObstacle.enabled = false;
                    bowHeroController.SetBlendSpeed(1f, false);
                }
                else
                {
                    bowHeroController.SetBlendSpeed(0f, true);
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
            if (!bowHeroController.TargetAttack.IsDead)
            {
                if (heroController.IsInRange(bowHeroController.TargetAttack.transform))
                {
                    bowHeroController.SetBlendSpeed(0f, true);
                    heroController.OnNextState(HeroState.Attack_State);
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        timer = 0.05f;
                        navMeshPath.ClearCorners();
                        if (NavMesh.SamplePosition(bowHeroController.TargetAttack.transform.position, out navMeshHit, 5f, NavMesh.AllAreas))
                        {
                            NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                        }

                        bowHeroController.FindTarget();
                    }


                    if (navMeshPath.corners.Length >= 2)
                    {
                        Vector3 posNextCorners = navMeshPath.corners[1];
                        posNextCorners.y = transform.position.y;
                        Vector3 targetDir = (posNextCorners - transform.position).normalized;
                        Quaternion quaternion = Quaternion.LookRotation(targetDir, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 7 * Time.deltaTime);


                        Vector3 movementDir = transform.forward;
                        if (!heroController.CharacterController.isGrounded) { MovementY -= Time.deltaTime; }
                        else { MovementY = 0f; }
                        movementDir.y = MovementY;
                        heroController.CharacterController.Move(movementDir * bowHeroController.SpeedRun * Time.deltaTime);
                    }
                }
            }
            else { bowHeroController.OnNextState(HeroState.Idle_State); }
        }
    }
}
