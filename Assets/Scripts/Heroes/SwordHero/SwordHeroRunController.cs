using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordHeroRunController : HeroRunController
{
    private SwordHeroController swordHeroController;
    private NavMeshPath navMeshPath = null;
    private NavMeshHit navMeshHit;
    private float movementY = 0;
    private float timer = 0f;


    public override void EnterRunState()
    {
        base.EnterRunState();
        swordHeroController = (SwordHeroController)heroController;

        if (IngameManager.Instance.GameState != GameState.GameStart)
        {
            heroController.OnNextState(HeroState.Idle_State);
        }
        else
        {
            if (swordHeroController.TargetAttack != null)
            {
                if (heroController.IsInRange(swordHeroController.TargetAttack.transform) == false)
                {
                    navMeshPath = new NavMeshPath();
                    heroController.CapsuleCollider.enabled = false;
                    heroController.CharacterController.enabled = true;
                    heroController.NavMeshObstacle.enabled = false;
                    swordHeroController.SetBlendSpeed(1f, false);
                }
                else
                {
                    swordHeroController.SetBlendSpeed(0f, true);
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
            if (!swordHeroController.TargetAttack.IsDead)
            {
                if (heroController.IsInRange(swordHeroController.TargetAttack.transform))
                {
                    swordHeroController.SetBlendSpeed(0f, true);
                    heroController.OnNextState(HeroState.Attack_State);
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        timer = 0.05f;
                        navMeshPath.ClearCorners();
                        if (NavMesh.SamplePosition(swordHeroController.TargetAttack.transform.position, out navMeshHit, 50f, NavMesh.AllAreas))
                        {
                            NavMesh.CalculatePath(transform.position, navMeshHit.position, NavMesh.AllAreas, navMeshPath);
                        }

                        swordHeroController.FindTarget();
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
                        heroController.CharacterController.Move(movementDir * swordHeroController.SpeedRun * Time.deltaTime);
                    }
                }
            }
            else { swordHeroController.OnNextState(HeroState.Idle_State); }
        }
    }
}
