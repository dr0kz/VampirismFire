using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IExecuteAction<Enemy>
{
    public void executeAction(Enemy enemy)
    {
        enemy.GetNavMeshAgent().enabled = false;
        enemy.GetNavMeshObstacle().enabled = true;
        enemy.GetAnimator().SetBool("Run", false);
        enemy.GetAnimator().SetTrigger("Idle");
    }
}
