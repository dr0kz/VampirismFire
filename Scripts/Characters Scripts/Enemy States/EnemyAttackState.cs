using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAttackState : MonoBehaviour, IExecuteAction<Enemy>
{
    public void executeAction(Enemy enemy)
    {
        if (enemy.GetAnimator().GetBool("Attack")) return;
        #region Smart AI
        if(enemy.GetIsSmartAI())
        {
            if (enemy.GetNavMeshAgent().pathStatus == NavMeshPathStatus.PathComplete && !enemy.GetCollidedWithBuilding().Equals(enemy.GetDestination())) return;
        }
        #endregion
        if (enemy.GetNavMeshAgent().isActiveAndEnabled) enemy.GetNavMeshAgent().isStopped = true;
        enemy.GetNavMeshAgent().enabled = false;
        enemy.GetNavMeshObstacle().enabled = true;
        enemy.GetAnimator().SetBool("Attack", true);
        enemy.SetAttackBuilding(enemy.GetCollidedWithBuilding());
    }
}
