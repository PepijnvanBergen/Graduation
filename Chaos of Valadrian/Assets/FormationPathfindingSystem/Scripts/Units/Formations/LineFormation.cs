using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineFormation : BaseFormation
{
    public Vector2 minMaxDistance;
    public GameObject[,] lineFormation;
    public float separation;
    public override void StartFormation(Group _group)
    {
        CreateLineFormation(_group, 2);
        //CreateLineFormation(units.ToArray(), 2);
    }

    public void CreateLineFormation(Group _group, int _lineThickness = 1)
    {
        FormationPos workFPos;
        int count = 0;
        Vector3 midpoint = GetMidPoint(startPos, endPos);
        formationMaster = Instantiate(formationMasterPF, midpoint, Quaternion.identity);

        float lineLength = Vector3.Distance(startPos.position, endPos.position);
        float unitsPerLength = _group.units.Count / _lineThickness;
        float distanceBetweenUnits = lineLength / unitsPerLength;

        lineFormation = new GameObject[_lineThickness, (int) unitsPerLength + 1];
        float middle = unitsPerLength / 2;

        Vector3 formationManagerPos = formationMaster.transform.position;

        for (int x = 0; x < _lineThickness; x++)
        {
            for (int y = 0; y < unitsPerLength; y++)
            {

                //lineFormation[x, y] = units[count];
                lineFormation[x, y] = Instantiate(formationPosPF, formationManagerPos, Quaternion.identity);
                lineFormation[x, y].transform.parent = formationMaster.transform;
                workFPos = lineFormation[x, y].AddComponent<FormationPos>();
                workFPos.connectedUnit = _group.units[count];
                _group.units[count].formationTarget = workFPos.gameObject;


                //NewPos = ((y - middle) * distance) + formationMaster
                float calcValue = y;
                // deze moet nu dus 0,0,0 zijn
                Vector3 newPos = new Vector3((calcValue - (middle - 0.5f)) * distanceBetweenUnits,
                    formationManagerPos.y, (thicknessDistance * x));
                workFPos.ChangePos(newPos); //Dit werkt nog niet denk ik

                count++;
            }
        }

        _group.formationMaster = formationMaster;
        _group.formationPositions = lineFormation;
    }

    public override void ResizeFormation()
    {
        
    }
}
