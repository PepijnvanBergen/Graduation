using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LooseFormation : BaseFormation
{
    public float maxLooseRadius = 7;
    [SerializeField] private LayerMask layerMask;
    private float radius;

    public float formationMoraleImpact = -5;
    public float formationSpeedImpact = 1.2f;//Percentages
    
    private float speedStat = 0;
    public override void StartFormation(Group _group)
    {
        Vector3 midPoint = GetMidPoint(startPos, endPos);
        
        radius = Vector3.Distance(startPos.position, endPos.position) / 2;
        if (radius > maxLooseRadius) radius = maxLooseRadius;
        
        if(!_group.formationMaster) _group.formationMaster = Instantiate(formationMasterPF, midPoint, Quaternion.identity);
        if(!_group.startPos) _group.startPos = Instantiate(startPos, startPos.transform.position, Quaternion.identity);
        if(!_group.endPos) _group.endPos = Instantiate(endPos, endPos.transform.position, Quaternion.identity);
        
        foreach (BaseUnit unit in _group.units)
        {
            unit.formationTarget = Instantiate(formationPosPF, FindGoodPosition(_group.startPos.position, _group.endPos.position), Quaternion.identity);
            unit.formationTarget.transform.parent = _group.formationMaster.transform;
            _group.formationPositions.Add(unit.formationTarget);
        }

        FormationStartImpact(_group);
    }
    public void FormationStartImpact(Group _group)  
    {
        foreach (BaseUnit unit in _group.units)
        {
            unit.morale += formationMoraleImpact;
            speedStat = unit.agent.speed;
            unit.agent.speed = speedStat * formationMoraleImpact;
        }
    }
    public void FormationEndImpact(Group _group)  
    {
        foreach (BaseUnit unit in _group.units)
        {
            unit.morale -= formationMoraleImpact;
            //If different speeds this needs change
            unit.agent.speed = speedStat; //Maybe weird reaction because of multiplication
        }
    }

    public override void EndFormation(Group _group)
    {
        FormationEndImpact(_group);
    }
    private Vector3 FindGoodPosition(Vector3 _startPos, Vector3 _endPos)
    {
        //Vector3 midpoint = _midPoint;
        bool clearPos = false;
        Vector3 randomPoint = new Vector3();
        bool startXSmaller = true;
        bool startZSmaller = true;
         
         if (_startPos.x < _endPos.x)
         {
             startXSmaller = true;
         }
         else
         {
             startXSmaller = false;
         }

         if (_startPos.z < _endPos.z)
         {
             startZSmaller = true;
         }
         else
         {
             startZSmaller = false;
         }
         
        while (!clearPos)
        {
            randomPoint.y = _startPos.y;
            if (startXSmaller)
            {
                randomPoint.x = Random.Range(_startPos.x, _endPos.x);
            }
            else
            {
                randomPoint.x = Random.Range(_endPos.x, _startPos.x);
            }
            if (startZSmaller)
            {
                randomPoint.z = Random.Range(_startPos.z, _endPos.z);
            }
            else
            {
                randomPoint.z = Random.Range(_endPos.z, _startPos.z);
            }
            // midpoint.x += Random.Range(-_radius, _radius);
            // midpoint.z += Random.Range(-_radius, _radius);
            
            
            //Voor efficientie heb ik even random punt in een vierkant er van gemaakt.
            
            
            /*
            float r = Random.Range(0.00f, 1.00f);
            float random = radius * r; //Mathf.Sqrt(r);
            float theta = (random * 2) * 3.14f;

            midpoint.x += r * Mathf.Cos(theta);
            midpoint.z += r * Mathf.Sin(theta);
            float r1 = Random.Range();

            */
            if (Physics.OverlapSphere(randomPoint, 1f, layerMask) != null)
            {
                clearPos = true;
            }
        }
        return randomPoint;
    }
    
    public override void MoveFormation(Group _group, Vector3 _startPos, Vector3 _endPos) //Deze verplaatst niet.
    {
        _group.startPos.position = _startPos;
        _group.endPos.position = _endPos;
        Vector3 midPoint = GetMidPoint(_group.startPos, _group.endPos);
        _group.formationMaster.transform.position = midPoint;
        
        foreach (BaseUnit unit in _group.units)
        {
            unit.formationTarget = Instantiate(formationPosPF, FindGoodPosition(_group.startPos.position, _group.endPos.position), Quaternion.identity);
            unit.formationTarget.transform.parent = _group.formationMaster.transform;
        }
    }

    private void GetPositions(Group _group, Transform _newTransform)
    {
        _group.formationMaster.transform.position = _newTransform.position;
        _group.formationMaster.transform.rotation = _newTransform.rotation;
        List<GameObject> looseFormation = new List<GameObject>();
        List<BaseUnit> uncheckedUnits = _group.units;
        Vector3 midPoint = _group.formationMaster.transform.position;
        for (int i = uncheckedUnits.Count; i > 0; i--)
        {
            if (Physics.OverlapSphere(uncheckedUnits[i].transform.position, 1f, layerMask) != null)
            {
                bool clearPos = false;
                
                while (!clearPos)
                {
                    Vector3 randPoint = midPoint;
                    // randPoint.x += Random.Range(-radius, radius);
                    // randPoint.z += Random.Range(-radius, radius);
                    
                    // random = radius * sqrt(r)
                    // theta = random() * 2 * PI

                    float r = Random.Range(0f, 1f);
                    float random = radius * Mathf.Sqrt(r);
                    float theta = (random * 2) * 3.14f;

                    randPoint.x += r * Mathf.Cos(theta);
                    randPoint.z += r * Mathf.Sin(theta);
                    
                
                    if (Physics.OverlapSphere(randPoint, 1f, layerMask) != null)
                    {
                        looseFormation.Add(Instantiate(formationPosPF,randPoint, Quaternion.identity));
                        uncheckedUnits[i].formationTarget = looseFormation.Last(); //Ik denk dat dit werkt dunno zeker
                        uncheckedUnits[i].formationTarget.transform.parent = _group.formationMaster.transform;
                        clearPos = true;
                    }

                }
            }

        }
    }
}
