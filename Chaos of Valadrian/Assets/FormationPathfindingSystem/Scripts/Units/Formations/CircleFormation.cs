using System.Collections.Generic;
using UnityEngine;

public class CircleFormation : BaseFormation
{
    public float formationMoraleImpact = 20f;
    public float maxRadius;
    public float minRadius;
    public float maxPerCircle;
    public override void StartFormation(Group _group)
    {
        SetupCircleFormation(_group);
        FormationStartImpact(_group);
        //CreateLineFormation(units.ToArray(), 2);
    }
    public void FormationStartImpact(Group _group)  
    {
        foreach (Soldier soldier in _group.units)
        {
            soldier.morale += formationMoraleImpact;
        }
    }
    public void FormationEndImpact(Group _group)  
    {
        foreach (Soldier soldier in _group.units)
        {
            soldier.morale -= formationMoraleImpact;
        }
    }
    public override void EndFormation(Group _group)
    {
        FormationEndImpact(_group);
    }

    public void SetupCircleFormation(Group _group)
    {
        if(!_group.formationMaster) _group.formationMaster = Instantiate(formationMasterPF, GetMidPoint(startPos, endPos), Quaternion.identity);
        if(!_group.startPos) _group.startPos = Instantiate(startPos, startPos.transform.position, Quaternion.identity);
        if(!_group.endPos) _group.endPos = Instantiate(endPos, endPos.transform.position, Quaternion.identity);
        float radius = Vector3.Distance(startPos.position, endPos.position) / 2;
        if (radius > maxRadius) radius = maxRadius;
        if (radius < minRadius) radius = minRadius;
        MakeCircle(_group, radius);
    }

    private void MakeCircle(Group _group, float _radius)
    {
        Vector3 midPoint = _group.formationMaster.transform.position;
        Vector3 workPos = midPoint;
        List<GameObject> circlePositions;
        int count = 0;

        foreach (BaseUnit unit in _group.units)
        {
            int angle = (360 / _group.units.Count) * count;
            count++;
            
            workPos.x = midPoint.x + _radius * Mathf.Cos(angle * Mathf.PI / 180f);
            workPos.z = midPoint.z + _radius * Mathf.Sin(angle * Mathf.PI / 180f);

            unit.formationTarget = Instantiate(formationPosPF, workPos, Quaternion.identity);
            unit.formationTarget.transform.parent = _group.formationMaster.transform;
            _group.formationPositions.Add(unit.formationTarget);
        }
    }
    public override void MoveFormation(Group _group, Vector3 _startPos, Vector3 _endPos)
    {
        _group.startPos.position = _startPos;
        _group.endPos.position = _endPos;
        Vector3 midpoint = GetMidPoint(_group.startPos, _group.endPos);
        _group.formationMaster.transform.position = midpoint;
        int lineThickness = 2; //Verzin hier nog een betere oplossing voor.
        float radius = Vector3.Distance(_group.startPos.position, _group.endPos.position) / 2;
        MakeCircle(_group, radius);
    }
}
