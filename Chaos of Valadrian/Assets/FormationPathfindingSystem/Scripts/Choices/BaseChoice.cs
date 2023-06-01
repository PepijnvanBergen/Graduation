using UnityEngine;

public class BaseChoice : MonoBehaviour
{
    [Header("Choice threshold has to be 0")]
    public float choiceThreshold;  //The treshold has to be reached in order for this choice to be able to made.
    [Header("if the choiceWeight = the choice threshold then the choice can be picked, don't change the weight in editor.")]
    public float choiceWeight;
    public virtual void EnterAction()
    {
        
    }
    public virtual void Action(Soldier _soldier)
    {
        return;
    }
    public virtual float CalculateWeight(Soldier _soldier)
    {
        float choiceWeight = 0;
        return choiceWeight;
    }
    public virtual void ExitAction()
    {
        
    }
}
