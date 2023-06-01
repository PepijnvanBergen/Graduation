using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseFornation : BaseFormation
{
    public float maxLooseRadius = 7;
    [SerializeField] private LayerMask layerMask;
    private float radius;
    public override void StartFormation(Group _group)
    {
        Vector3 midPoint = GetMidPoint(startPos, endPos);
        radius = Vector3.Distance(startPos.position, endPos.position) / 2;
        if (radius > maxLooseRadius) radius = maxLooseRadius;
        _group.formationMaster = Instantiate(formationMasterPF, midPoint, Quaternion.identity);
        foreach (BaseUnit unit in _group.units)
        {
            unit.formationTarget = Instantiate(formationPosPF, FindGoodPosition(midPoint, radius), Quaternion.identity);
            unit.formationTarget.transform.parent = _group.formationMaster.transform;
        }
    }

    private Vector3 FindGoodPosition(Vector3 _midPoint, float _radius)
    {
        Vector3 randPoint = _midPoint;
        bool clearPos = false;
        
        while (!clearPos)
        {
            randPoint.x += Random.Range(-_radius, _radius);
            randPoint.z += Random.Range(-_radius, _radius);
            if (Physics.OverlapSphere(randPoint, 1f, layerMask) != null)
            {
                clearPos = true;
            }
        }
        return randPoint;
    }

    public override void MoveFormation(Group _group, Transform _newTransform)
    {
        _group.formationMaster.transform.position = _newTransform.position;
        _group.formationMaster.transform.rotation = _newTransform.rotation;
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
                    
                    float r = Random.Range(0, 1);
                    float random = radius * Mathf.Sqrt(r);
                    float theta = (random * 2) * 3.14f;

                    randPoint.x += r * Mathf.Cos(theta);
                    randPoint.z += r * Mathf.Sin(theta);
                    
                
                    if (Physics.OverlapSphere(randPoint, 1f, layerMask) != null)
                    {
                        uncheckedUnits[i].formationTarget.transform.position = randPoint;
                        clearPos = true;
                    }

                }
            }

        }

        

    }
}
