using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseUnit
{
    public Soldier friend;
    [SerializeField] private Transform friendShowerPos;
    public BaseUnit attackTarget;
    public BaseChoice choice;
    public Group group;

    public bool charge = false;
    // public Transform[] rayCastPositions;
    // public float raycastDistance;
    // public float raycastDistanceForward;
    // public LayerMask collisionMask;
    
    public override void FollowChoice()
    {
        choice.Action(this);
    }
}