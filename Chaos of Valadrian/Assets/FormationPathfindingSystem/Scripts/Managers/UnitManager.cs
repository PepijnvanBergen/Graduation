using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : SerializedMonoBehaviour
{
    [SerializeField] private ChoiceManager choiceManager;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private UIGroupManager uiGroupManager;
    public bool spawnUnits = false;
    public bool newOrders = false;
    public int spawnAmmount = 1;
    
    public GameObject unitPF;
    public float moveSpeed;
    public float health;
    public float soldierStartMorale;
    
    public List<Soldier> soldiers = new List<Soldier>();
    private List<BaseUnit> workTeamList = new List<BaseUnit>();
    [SerializeField] private int teamInt;
    [OdinSerialize] private Dictionary<int, Group> teams = new Dictionary<int, Group>();
    [Header("Add the two Transforms for each team in order of the teams")]
    [SerializeField] private Transform[] safeTeamPositions;
    public void Execute()
    {
        if (spawnUnits)
        {
            SpawnUnits();
        }

        if (teams != null)
        {
            foreach (KeyValuePair<int, Group> key in teams)
            {
                key.Value.Execute();
            }
        }
    }

    public Group FindGroup(Vector3 _mousePos)
    {
        Group workGroup = null;
        //FindUnit(_mousePos).
        return workGroup;
        //find the closest group

    } //Not finished
    public BaseUnit FindUnit(Vector3 _mousePos)
    {
        BaseUnit workUnit = null;
        return workUnit;
        //find the closest group
    } //Not finished
    public void SpawnUnits()
    {
        int friendready = 1;
        for (int s = 0; s < spawnAmmount; s++)
        {
            int listLength = soldiers.Count;
                
            spawnUnits = false;
            GameObject workObj = Instantiate(unitPF, Vector3.zero, Quaternion.identity);
            Soldier workSoldier = workObj.AddComponent<Soldier>();
            workSoldier.agent = workSoldier.GetComponent<NavMeshAgent>();
            workSoldier.agent.speed = moveSpeed;
            workSoldier.health = health;
            workSoldier.unitType = unitType.indifferent;
            workSoldier.name = listLength + " Team " + teamInt;
            //workSoldier.name += " Team " + teamInt;
            workSoldier.morale = soldierStartMorale;
                
            if (friendready == 2)
            {
                workSoldier.friend = soldiers[listLength - 1];
                soldiers[listLength - 1].friend = workSoldier;
                friendready = 0;
            }
                
            soldiers.Add(workSoldier);
            workTeamList.Add((BaseUnit)workSoldier);
            choiceManager.GiveDefaultChoice(workSoldier);
            choiceManager.soldiers.Add(workSoldier);

            if (teamInt == 1)
            {
                workSoldier.transform.position = CalculateSafePosition(safeTeamPositions[0], safeTeamPositions[1]);
            } else if (teamInt == 2)
            {
                workSoldier.transform.position = CalculateSafePosition(safeTeamPositions[2], safeTeamPositions[3]);
            } else if (teamInt == 3)
            {
                workSoldier.transform.position = CalculateSafePosition(safeTeamPositions[4], safeTeamPositions[5]);
            }
            friendready++;
        }
        if (teams.ContainsKey(teamInt))
        {
            teams[teamInt].units.AddRange(workTeamList);  
        }
        else
        {
            GameObject workGroup = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            workGroup.AddComponent<Group>();
            teams.Add(teamInt, workGroup.GetComponent<Group>());
            
            teams[teamInt].teamInt = teamInt;
            teams[teamInt].units = workTeamList;
            teams[teamInt].StartUp();
            workGroup.name = teams[teamInt].name;
            
            formationManager.StartDefaultFormation(teams[teamInt]);
            uiGroupManager.AddGroup(teams[teamInt]);
            //SendEvent add team to UI
        }
    }

    public void Fight(Group _attackGroup, Group _defendGroup)
    {
        _attackGroup.Attack(_defendGroup);
    }
    public Vector3 CalculateSafePosition(Transform _firstPos, Transform _secondPos)
    {
        Vector3 safePos = Vector3.zero;
        float x1 = _firstPos.position.x;
        float x2 = _secondPos.position.x;
        float z1 = _firstPos.position.z;
        float z2 = _secondPos.position.z;
        
        
        if (x1 > x2)
        {
            safePos.x = Random.Range(x2, x1);
        }
        else
        {
            safePos.x = Random.Range(x1, x2);
        }

        if (z1 > z2)
        {
            safePos.z = Random.Range(z2, z1);
        }else 
        {
            safePos.z = Random.Range(z1, z2);
        }
        
        return safePos;
    }
}
