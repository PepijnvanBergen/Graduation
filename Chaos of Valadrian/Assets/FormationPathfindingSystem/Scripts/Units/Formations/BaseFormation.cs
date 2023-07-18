using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFormation : MonoBehaviour
{
    public GameObject formationPosPF;
    public List<BaseUnit> units;
    //public BaseUnit leader;
    public float thicknessDistance = 2;
    public GameObject formationMasterPF;
    //public GameObject formationMaster;
    
    public Transform startPos;
    public Transform endPos;
    public formationType thisFormation;
    public virtual void StartFormation(Group _group) //abstract 
    {
    }
    public virtual void MoveFormation(Group _group, Vector3 _firstPoint, Vector3 _secondPoint)
    {
    }
    public Vector3 GetMidPoint(Transform _startPos, Transform _endPos)
    {
        Vector3 startPos = _startPos.position;
        Vector3 endPos = _endPos.position;

        return new Vector3((startPos.x + endPos.x)/2, (startPos.y + endPos.y)/2, (startPos.z + endPos.z)/2);
    }

    public virtual void EndFormation(Group _group)
    {
    }
}
