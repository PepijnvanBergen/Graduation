using UnityEngine;

public class BaseChoice : MonoBehaviour
{
    public float choiceThreshold;  //The treshold has to be reached in order for this choice to be able to made.
    public float choiceWeight;     //The weight is how likely this choice is to be chosen if no choice treshold has been reached.
    public GameObject choiceSignalPF;

    public virtual void EnterAction(Soldier _soldier)
    {
        _soldier.choiceSignal = Instantiate(choiceSignalPF, Vector3.zero, Quaternion.identity);
    }
    public virtual void Action(Soldier _soldier)
    {
    }
    public virtual float CalculateWeight(Soldier _soldier)
    {
        float choiceWeight = 0;
        return choiceWeight;
    }
    public virtual void ExitAction(Soldier _soldier)
    {
        if (_soldier.choiceSignal)
        {
            Destroy(_soldier.choiceSignal);
            _soldier.choiceSignal = null;
        }
    }
}
