using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
public class LineFormation : BaseFormation
{
    public GameObject[,] lineFormation;
    public float separation;
    public float formationMoraleImpact = 10f;
    public float formationHealthImpact = 10f;
    public override void StartFormation(Group _group)
    {
        CreateLineFormation(_group, 2);

        FormationStartImpact(_group);
    }

    public void FormationStartImpact(Group _group)  
    {
        foreach (BaseUnit unit in _group.units)
        {
            unit.morale += formationMoraleImpact;
            unit.health += formationHealthImpact; //Dit vervangen met Defence
        }
    }
    public void FormationEndImpact(Group _group)  
    {
        foreach (BaseUnit unit in _group.units)
        {
            unit.morale -= formationMoraleImpact;
            if (unit.health < formationHealthImpact)
            {
                unit.health = 1f;
            }

        }
    }
    public void CreateLineFormation(Group _group, int _lineThickness = 1)
    {
        int count = 0;
        Vector3 midPoint = GetMidPoint(startPos, endPos);
        if(!_group.formationMaster) _group.formationMaster = Instantiate(formationMasterPF, midPoint, Quaternion.identity);
        if(!_group.startPos) _group.startPos = Instantiate(startPos, startPos.transform.position, Quaternion.identity);
        if(!_group.endPos) _group.endPos = Instantiate(endPos, endPos.transform.position, Quaternion.identity);

        float lineLength = Vector3.Distance(_group.startPos.position, _group.endPos.position);
        float unitsPerLength = _group.units.Count / _lineThickness;
        float distanceBetweenUnits = lineLength / unitsPerLength;

        lineFormation = new GameObject[_lineThickness, (int) unitsPerLength + 1];
        float middle = unitsPerLength / 2;

        //_group.formationPositions = 
        GetPositions(_group, _lineThickness, unitsPerLength, distanceBetweenUnits, midPoint);
    }
    private void GetPositions(Group _group, int _lineThickness, float _unitsPerLength, float _distanceBetweenUnits, Vector3 _midPoint)
    {
        FormationPos workFPos;
        lineFormation = new GameObject[_lineThickness, (int) _unitsPerLength + 1];
        int count = 0;
        float middle = _unitsPerLength / 2;
        
        for (int x = 0; x < _lineThickness; x++)
        {
            for (int y = 0; y < _unitsPerLength; y++)
            {

                //lineFormation[x, y] = units[count];
                lineFormation[x, y] = Instantiate(formationPosPF, _midPoint, Quaternion.identity);
                lineFormation[x, y].transform.parent = _group.formationMaster.transform;
                workFPos = lineFormation[x, y].AddComponent<FormationPos>();
                workFPos.connectedUnit = _group.units[count];
                _group.units[count].formationTarget = workFPos.gameObject;
                _group.formationPositions.Add(lineFormation[x,y]);

                //NewPos = ((y - middle) * distance) + formationMaster
                float calcValue = y;
                // deze moet nu dus 0,0,0 zijn
                Vector3 newPos = new Vector3((calcValue - (middle - 0.5f)) * _distanceBetweenUnits,
                    _midPoint.y, (thicknessDistance * x));
                workFPos.ChangePos(newPos); //Dit werkt nog niet denk ik

                count++;
            }
        }
    }
    public override void MoveFormation(Group _group, Vector3 _startPos, Vector3 _endPos) //Hij verplaats de formatie naar precies het laatste punt waar geklikt is.
    {
        _group.startPos.position = _startPos;
        _group.endPos.position = _endPos;
        Vector3 midPoint = GetMidPoint(_group.startPos, _group.endPos);
        _group.formationMaster.transform.position = midPoint;
        int lineThickness = 2; //Verzin hier nog een betere oplossing voor.
        float unitsPerLength = _group.units.Count / lineThickness;
        float lineLength = Vector3.Distance(_group.startPos.position, _group.endPos.position);
        float distanceBetweenUnits = lineLength / unitsPerLength;

        //_group.formationPositions =
        GetPositions(_group, lineThickness, unitsPerLength, distanceBetweenUnits, midPoint);
    }
    public override void EndFormation(Group _group)
    {
        FormationEndImpact(_group);
    }
}
