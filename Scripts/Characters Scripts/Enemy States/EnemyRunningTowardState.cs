using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunningTowardState : MonoBehaviour, IExecuteAction<Enemy>
{
    public void executeAction(Enemy enemy)
    {
        if (enemy.GetDestination().GetHealth() <= 0f)
        {
            enemy.SetState(enemy.GetIdleState());
            return;
        }
        enemy.GetAnimator().SetBool("Attack", false);
        enemy.GetNavMeshObstacle().enabled = false;
        enemy.GetNavMeshAgent().enabled = true;
        enemy.GetNavMeshAgent().SetDestination(enemy.GetDestination().transform.position);
        enemy.GetNavMeshAgent().isStopped = false;
    }
}
