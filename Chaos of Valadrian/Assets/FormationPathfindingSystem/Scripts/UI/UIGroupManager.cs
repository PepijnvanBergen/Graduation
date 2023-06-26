using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroupManager : MonoBehaviour
{
    [Header("This is the ammount of groups in the HUD keep it at 10")]
    [SerializeField] private float maxGroups = 10;
    [SerializeField] private Vector2 minPos = new Vector2(-357.6f, 182.8f);
    [SerializeField] private List<Group> groupsInHud = new List<Group>();
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject UIGroupHolderPF;
    public UIGroupHolder selectedGroup;
    
    [SerializeField] private SystemManager systemManager; //Find a way to fix this!

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color originalColor;
    
    public void AddGroup(Group _groupForHud)
    {
        float yValue = minPos.y;
        yValue *= groupsInHud.Count + 1;
        GameObject workGO = Instantiate(UIGroupHolderPF,Vector3.zero , Quaternion.identity);
        workGO.transform.SetParent(canvas.transform);
        workGO.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        workGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(minPos.x, yValue);
        //Debug.Log(workGO.GetComponent<RectTransform>().position);
        //Debug.Log(workGO.GetComponent<RectTransform>().anchoredPosition);
        UIGroupHolder workUIGH = workGO.GetComponent<UIGroupHolder>();
        workUIGH.ChangeVariables(_groupForHud.name, _groupForHud.averageMorale, (int)_groupForHud.currentFormation.thisFormation);
        workUIGH.groupManager = this;
        _groupForHud.uiHolder = workUIGH;
        groupsInHud.Add(_groupForHud);
        workUIGH.group = _groupForHud;
    }
    public void ChangeToFormation(int _selectedForation)
    {
        systemManager.UIChangeFormation(selectedGroup.group, _selectedForation);
        selectedGroup.ChangeVariables("name", selectedGroup.@group.averageMorale, _selectedForation);
    }

    public void ChangeSelectedGroup(UIGroupHolder _newUIGroupHolder)
    {
        if (selectedGroup != null)
        {
            selectedGroup.image.color = originalColor;
        }

        _newUIGroupHolder.image.color = selectedColor;
        selectedGroup = _newUIGroupHolder;

        //prob give a call to the SystemManager that the group is now selected and need something.
    }

    public void DeselectGroup()
    {
        selectedGroup.image.color = originalColor;
        selectedGroup = null;
    }
}