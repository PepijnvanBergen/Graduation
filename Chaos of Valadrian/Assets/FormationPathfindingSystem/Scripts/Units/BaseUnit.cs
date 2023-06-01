using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
     public NavMeshAgent agent;
     public bool allowedToMove = true;
     public GameObject formationTarget;
     //public Transform targetPos;
     public float moveSpeed;
     public float health;
     public float morale;
     public unitType unitType;
     //public traits unitTrait;
     public virtual void FollowChoice()
     {
         
     }
     public void MoveTowards(Vector3 _desiredPos)
     {
         if (allowedToMove)
         {
             agent.SetDestination(_desiredPos);
             //agent.
         }
         else
         {
             
         }
     }
}