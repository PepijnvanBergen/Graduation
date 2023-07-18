using UnityEngine;
using UnityEngine.AI;
public class BaseUnit : MonoBehaviour
{
     public NavMeshAgent agent;
     public bool allowedToMove = true;
     public bool inCombat = false;
     public GameObject formationTarget;
     public float health;
     public float morale;
     public float minDistanceToTarget;
     public unitType unitType;
     
     public Animator animator;
     public bool isEnemy;
     
     public virtual void FollowChoice()
     {

     }
     public void MoveTowards(Vector3 _desiredPos) //Check dit.
     {
         if (Vector3.Distance(_desiredPos, transform.position) < minDistanceToTarget)
         {
             if (inCombat)
             {
                 allowedToMove = false;
                 animator.SetBool("isIdle", false);
                 animator.SetBool("isRunning", false);
                 animator.SetBool("isAttacking", true);
             }
             else
             {
                 allowedToMove = true;
             }
         }
         else
         {
             allowedToMove = true;
         }
         if (allowedToMove)
         {
             if (agent.isStopped)
                 agent.isStopped = false;
             
             agent.SetDestination(_desiredPos);
             
             animator.SetBool("isIdle", false);
             animator.SetBool("isRunning", true);
             animator.SetBool("isAttacking", false);
         }
         else
         {
              agent.isStopped = true;
             
              animator.SetBool("isIdle", true);
              animator.SetBool("isRunning", false);
              animator.SetBool("isAttacking", false);
         }
     }
}