using UnityEngine;

public class RunAwayChoice : BaseChoice
{
    [Header("Multipliers multiply the value by the multiplier amount before making the choice.")]
    public float moraleMultiplier = 1;
    public float healthMultiplier = 1;
    public override void Action(Soldier _soldier)
    {
        _soldier.MoveTowards(_soldier.safePosition);
    }

    public override float CalculateWeight(Soldier _soldier)
    {
        choiceWeight = 0;

        float soldierMorale = _soldier.morale;
        if (soldierMorale > 100f) soldierMorale = 100f;
        soldierMorale *= moraleMultiplier;

        float soldierHealth = _soldier.health;
        if (soldierHealth > 100f) soldierHealth = 100f;
        soldierHealth -= 100f;
        soldierHealth *= -1;
        soldierHealth *= healthMultiplier;
        
        choiceWeight = soldierMorale + soldierHealth;
        
        return choiceWeight;
    }
}
