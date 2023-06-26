using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Group : MonoBehaviour
{
    public string name;
    public List<BaseUnit> units;
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
            List<BaseUnit> temporaryDuplicate = _targetGroup.units;
            foreach (Soldier s in units)
            {
                if (temporaryDuplicate.Count == 0)
                {
                    temporaryDuplicate = _targetGroup.units;
                }

                s.attackTarget = temporaryDuplicate[0];
                s.inCombat = true;
                temporaryDuplicate.RemoveAt(0);
            }
            noTargets = false;
        }
    }
}