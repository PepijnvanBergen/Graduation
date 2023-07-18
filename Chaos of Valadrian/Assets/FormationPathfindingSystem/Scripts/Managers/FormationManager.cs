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
        _group.currentFormation = defaultFormation;
        _group.currentFormation.StartFormation(_group);
    }
    public void ChangeFormation(BaseFormation _requestedFormation, Group _group)
    {
        if (_group.formationPositions.Count != 0)
        {
            for (int i = 0; i < _group. formationPositions.Count; i++)
            {
                Destroy(_group.formationPositions[i].gameObject);
            }
            _group.formationPositions.Clear();
        }
        _group.currentFormation.EndFormation(_group);
        _group.currentFormation = _requestedFormation;
        _group.currentFormation.StartFormation(_group);
    }
    public void MoveFormation(Group _group, Vector3 _firstPoint, Vector3 _secondPoint)
    {
        if (_group.formationPositions.Count != 0)
        {
            for (int i = 0; i < _group. formationPositions.Count; i++)
            {
                Destroy(_group.formationPositions[i].gameObject);
            }
            _group.formationPositions.Clear();
        }
        _group.currentFormation.MoveFormation(_group, _firstPoint, _secondPoint);
    }

    public void Execute()
    {
        
    }
}
