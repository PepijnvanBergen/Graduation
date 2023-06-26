using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Animations;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private ChoiceManager choiceManager;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private UIGroupManager uIgroupManager;

    public Vector3 firstSelectionPoint;
    public Vector3 secondSelectionPoint;
    private bool firstPoint = false;
    [SerializeField] private LayerMask groundLayer;

    public int newFormationTest;

    private void Update()
    {
        unitManager.Execute();
        formationManager.Execute();
        if (choiceManager.soldiers.Count > 0)
        {
            choiceManager.Execute();   
        }

        //When time implement a better input system, this sucks.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hitData;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitData, 1000))//,groundLayer))
                {
                    Debug.Log(hitData.collider.gameObject.name);
                    if (hitData.collider.gameObject.CompareTag("Soldier"))
                    {
                        hitData.collider.GetComponent<Soldier>().group.uiHolder.MakeThisSelectedGroup();
                        Debug.Log("new group selected");
                    }

                    if (hitData.collider.name == "Ground")
                    {
                        firstSelectionPoint = hitData.point;
                        firstPoint = true;
                    }
                }
        }
        if (Input.GetMouseButtonUp(0) && firstPoint)
        {
            Ray ray;
            RaycastHit hitData;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitData, 1000))//,groundLayer))
            {
                if (hitData.collider.gameObject.layer == 5)
                {
                    Debug.Log("hitUI terminate life");
                    return;
                }
                secondSelectionPoint = hitData.point;
                
                if (hitData.collider.name == "Ground" && uIgroupManager.selectedGroup != null)
                {
                    formationManager.MoveFormation(uIgroupManager.selectedGroup.group, firstSelectionPoint, secondSelectionPoint);    
                }

                firstPoint = false;
            }

            if (uIgroupManager.selectedGroup != null)
            {
                //formationManager.;
            }
        }
    }

    public void UIChangeFormation(Group _group, int _formation)
    {
        if (_formation == (int)formationType.loose)
        {
            formationManager.ChangeFormation(formationManager.formations[1], _group);
        } else if (_formation == (int)formationType.line)
        {
            formationManager.ChangeFormation(formationManager.formations[0], _group);
        }else if (_formation == (int)formationType.circle)
        {
            //Debug.Log("first make the circle formation and put it in the formationManager formations list at the third spot.");
            formationManager.ChangeFormation(formationManager.formations[2], _group);
        }
    }
}