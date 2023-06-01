using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Animations;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private ChoiceManager choiceManager;
    [SerializeField] private FormationManager formationManager;

    public Vector3 firstSelectionPoint;
    public Vector3 secondSelectionPoint;
    private bool firstPoint = false;

    private void Update()
    {
        unitManager.Execute();
        formationManager.Execute();
        if (choiceManager.soldiers.Count > 0)
        {
            choiceManager.Execute();   
        }

        //Parkeer dit ff dit werkt niet
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("first point" + firstSelectionPoint);
            if (!firstPoint)
            {
                firstPoint = true;
                firstSelectionPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }

    private void MoveFormation(Group _group, Transform _newTransform)
    {
        formationManager.MoveFormation(_group, _newTransform);
    }


}