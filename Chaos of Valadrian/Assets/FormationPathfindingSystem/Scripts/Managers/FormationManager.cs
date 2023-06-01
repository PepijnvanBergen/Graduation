using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    public bool start = false;
    public GameObject formationPosPF;
    public GameObject formationMasterPF;
    public List<BaseFormation> formations = new List<BaseFormation>();

    public BaseFormation defaultFormation;
    
    //public Dictionary<int, Group> teams = new Dictionary<int, Group>();

    public float formationThicknessDistance;

    private void Start() //Give all the formations references
    {
        foreach (var formation in formations)
        {
            formation.formationPosPF = formationPosPF;
            formation.thicknessDistance = formationThicknessDistance;
            formation.formationMasterPF = formationMasterPF;
        }
    }

    public void StartDefaultFormation(Group _group)
    {
        _group.formation = defaultFormation;
        _group.formation.StartFormation(_group);
    }
    public void ChangeFormation(BaseFormation _requestedFormation, Group _group)
    {
        _group.formation = _requestedFormation;
        _group.formation.StartFormation(_group);
    }

    public void PlaceFormation(Vector3 _firstPos, Vector3 _secondPos)
    {
        // currentFormation.startPos.position = _firstPos;
        // currentFormation.endPos.position = _secondPos;
        
        //  ?
        //currentFormation.StartFormation(units);
    }
    public void MoveFormation(Group _group, Transform _newTransform)
    {
        _group.formation.MoveFormation(_group, _newTransform);
    }

    public void Execute()
    {
        
    }
}
