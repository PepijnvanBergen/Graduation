using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Group
{
    public List<BaseUnit> units;
    public int teamInt;
    public BaseFormation formation;
    public Group targetGroup;
    public bool attacking = false;
    private bool noTargets = true;
    
    //Formation Positions
    public GameObject formationMaster;
    public GameObject[,] formationPositions;
    
    public Group(int _teamInt, List<BaseUnit> _units)
    {
        teamInt = _teamInt;
        units = _units;
        foreach (Soldier s in _units)
        {
            s.group = this;
        }
    }

    public void Execute()
    {
        foreach(BaseUnit unit in units)
        {
            unit.FollowChoice();
        }

        //All the units are can move/do their action
    }

    public void Attack(Group _targetGroup)
    {
        if (noTargets)
        {
            List<BaseUnit> temporaryDuplicate = _targetGroup.units;
            foreach (Soldier s in units)
            {
                if (temporaryDuplicate.Count == 0)
                {
                    temporaryDuplicate = _targetGroup.units;
                }

                s.attackTarget = temporaryDuplicate[0];
                s.charge = true;
                temporaryDuplicate.RemoveAt(0);
            }
            noTargets = false;
        }
    }
}