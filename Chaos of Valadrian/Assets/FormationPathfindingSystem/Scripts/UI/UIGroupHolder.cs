using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGroupHolder : MonoBehaviour
{
    public string name;
    public float morale;
    public formationType currentFormation;
    public Group group;

    public TMP_InputField groupName;
    public TMP_InputField formationName;
    public Slider moraleSlider;

    public UIGroupManager groupManager;
    public Image image;
    
    public void ChangeVariables(string _groupName, float _morale, int _newFormation)
    {
        groupName.text = name = _groupName;
        moraleSlider.value = morale = _morale;
        currentFormation = (formationType) _newFormation; //Als dit niet werkt helaas lelijke if statements.
        formationName.text = currentFormation.ToString();
    }

    public void MakeThisSelectedGroup() //Naar de input manager/system manager en daar de changeFormation opdracht geven.... Events?
    {
        groupManager.ChangeSelectedGroup(this);
    }
}
