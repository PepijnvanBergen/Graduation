using UnityEngine;

public class FormationPos : MonoBehaviour
{
    public BaseUnit connectedUnit;

    public void ChangePos(Vector3 _newPos)
    {
        transform.localPosition = _newPos;
    }
}
