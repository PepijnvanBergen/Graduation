using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private List<BaseChoice> choices = new List<BaseChoice>();
    [SerializeField] private BaseChoice defaultChoice;
    public List<Soldier> soldiers = new List<Soldier>(); //The only thing changed to the soldiers in this list should be the choices!
    public float makeChoiceTime;
    [SerializeField] private float time;
    
    public void Execute()
    {
       // if (soldiers.Count > 0)
       // {
              if (time > makeChoiceTime)
              {
                  time = 0;
                  MakeChoices();
              }
              else
              {
                  time += Time.deltaTime;
              }   
       // }
    }

    public void GiveDefaultChoice(Soldier _soldier)
    {
        _soldier.choice = defaultChoice;
    }
    private void MakeChoices()
    {
        float highestWeight = 0;
        float workWeight = 0;
        BaseChoice bestChoice = null;
        
        foreach (Soldier soldier in soldiers)
        {
            highestWeight = 0;
            bestChoice = null;
            
            foreach (BaseChoice choice in choices)
            {
                workWeight = choice.CalculateWeight(soldier);
                if (workWeight > choice.choiceThreshold)
                {
                    if (workWeight > highestWeight)
                    {
                        highestWeight = workWeight;
                        bestChoice = choice;
                    }
                }
            }
            soldier.choice = bestChoice;
        }
    }
}


