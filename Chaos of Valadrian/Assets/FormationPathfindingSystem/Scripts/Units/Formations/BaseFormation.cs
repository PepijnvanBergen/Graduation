using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFormation : MonoBehaviour
{
    public GameObject formationPosPF;
    public List<BaseUnit> units;
    //public BaseUnit leader;
    public float thicknessDistance = 2;
    public GameObject formationMasterPF;
    public GameObject formationMaster;
    
    public Transform startPos;
    public Transform endPos;

    // public void FilterLeader()
    // {
    //     foreach (BaseUnit bu in units)
    //     {
    //         if (bu == leader)
    //         {
    //             units.Remove(bu);
    //             return;
    //         }
    //     }
    // }

    public virtual void StartFormation(Group _group) //abstract 
    {
        
    }
    public virtual void MoveFormation(Group _group, Transform _newTransform)
    {
        _group.formationMaster.transform.position = _newTransform.position;
        _group.formationMaster.transform.rotation = _newTransform.rotation;
    }
    public virtual void ResizeFormation()
    {
        //Hier ervoor zorgen dat je de units loskoppelt van de formatie, dan de formatie opnieuw maken en dan de units de dichtbijzijnste plek claimen
    }
    public Vector3 GetMidPoint(Transform _startPos, Transform _endPos)
    {
        Vector3 startPos = _startPos.position;
        Vector3 endPos = _endPos.position;

        return new Vector3((startPos.x + endPos.x)/2, (startPos.y + endPos.y)/2, (startPos.z + endPos.z)/2);
    }
}
