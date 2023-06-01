using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class SaveFriendChoice : BaseChoice
{
    [Header("Multipliers multiply the value by the multiplier amount before making the choice.")]
    public float friendDistanceMultiplier = 1;
    public float friendHPMultiplier = 1;
    public override void Action(Soldier _soldier)
    {
        _soldier.attackTarget = _soldier.friend.attackTarget;
        _soldier.MoveTowards(_soldier.attackTarget.transform.position);
        //Ik wil hier de code voor het helpen van de vriend
    }

    public override float CalculateWeight(Soldier _soldier)
    {
        choiceWeight = 0;
        float friendDistance = Vector3.Distance(_soldier.transform.position, _soldier.friend.transform.position);
        float friendHP = _soldier.friend.health;
            
        if (friendDistance > 100f) friendDistance = 100f;
        friendDistance -= 100f;
        friendDistance *= -1;
        friendDistance *= friendDistanceMultiplier;
            
        if (friendHP > 100f) friendHP = 100f;
        friendHP -= 100f;
        friendHP *= -1;
        friendHP *= friendHPMultiplier;
            
        choiceWeight = friendDistance + friendHP;
        return choiceWeight;
    }
}