using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public string name;
    public List<Soldier> units = new List<Soldier>();
    public int teamInt;
    public BaseFormation currentFormation;
    public Group targetGroup;
    public bool attacking = false;
    private bool noTargets = true;
    public float averageMorale;
    
    //Formation Positions
    public GameObject formationMaster;
    public Transform startPos;
    public Transform endPos;
    public List<GameObject> formationPositions = new List<GameObject>();

    public UIGroupHolder uiHolder;
    public bool isEnemy;
    

    public void StartUp()
    {
        foreach (Soldier s in units)
        {
            s.group = this;
        }
    }
    public void Execute()
    {
        foreach(BaseUnit unit in units)
        {
            averageMorale += unit.morale;
            unit.FollowChoice();
        }
        averageMorale /= units.Count;
        
        //uiHolder.ChangeVariables(name, averageMorale, currentFormation.thisFormation);
        //All the units are can move/do their action
    }

    public void Attack(Group _targetGroup)
    {
        if (noTargets)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (_targetGroup.units[i] == null)
                {
                    units[i].attackTarget = _targetGroup.units[Random.Range(0, _targetGroup.units.Count)];
                }
                else
                {
                    units[i].attackTarget = _targetGroup.units[i];
                }

                units[i].inCombat = true;
            }
            noTargets = false;
        }

        targetGroup = _targetGroup;
        if (_targetGroup.targetGroup == null)
        {
            _targetGroup.Attack(this);
        }
    }
}