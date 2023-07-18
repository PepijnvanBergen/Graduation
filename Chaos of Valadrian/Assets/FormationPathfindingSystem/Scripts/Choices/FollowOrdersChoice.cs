using UnityEngine;

public class FollowOrdersChoice : BaseChoice
{
    [Header("Multipliers multiply the value by the multiplier amount before making the choice.")]
    public float moraleMultiplier = 1;

    public float attackRange;
    public override void Action(Soldier _soldier)
    {
        if (_soldier.inCombat)
        {
            //Debug.Log(_soldier.transform.gameObject.name + " Distance to target = " +Vector3.Distance(_soldier.transform.position, _soldier.attackTarget.transform.position));
            if (Vector3.Distance(_soldier.transform.position, _soldier.attackTarget.transform.position) < attackRange)
            {
                _soldier.Attack();
                _soldier.agent.isStopped = true;
            }
            else
            {
                _soldier.MoveTowards(_soldier.attackTarget.transform.position);
            }
        }
        else
        {
            _soldier.MoveTowards(_soldier.formationTarget.transform.position);
        }
    }

    public override float CalculateWeight(Soldier _soldier)
    {
        choiceWeight = 0;

        float soldierMorale = _soldier.morale;
        if (soldierMorale > 100f) soldierMorale = 100f;
        soldierMorale *= moraleMultiplier;
        choiceWeight = soldierMorale;
        
        return choiceWeight;
    }
}
