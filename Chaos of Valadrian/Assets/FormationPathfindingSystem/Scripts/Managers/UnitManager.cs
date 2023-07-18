using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private ChoiceManager choiceManager;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private UIGroupManager uiGroupManager;
    
    [SerializeField] private GameObject vikingPF;
    [SerializeField] private GameObject romanPF;
    
    [Header("Add the two Transforms for each team in order of the teams")]
    public Transform[] safeTeamPositions;
    
    public bool spawnUnits = false;
    [Header("Make this an even number")]
    public int spawnAmmount = 1;
    public bool spawnEnemy;
    
    public float moveSpeed;
    public float health;
    public float soldierStartMorale;
    
    public List<Soldier> soldiers = new List<Soldier>();
    private List<BaseUnit> workTeamList = new List<BaseUnit>();
    public int teamInt;
    public Dictionary<int, Group> teams = new Dictionary<int, Group>();

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
        Group selectedGroup = null;
        if (teams.ContainsKey(teamInt))
        {
            selectedGroup = teams[teamInt];
        }
        else
        {
            GameObject workGroup = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            selectedGroup = workGroup.AddComponent<Group>();
            teams.Add(teamInt, workGroup.GetComponent<Group>());
            teams[teamInt].name = "group: " + teamInt;
            teams[teamInt].teamInt = teamInt;
            //L2 = L1.Select(x => x.Copy()).ToList();
            //teams[teamInt].units = workTeamList.Select(x => x).ToList();
            workGroup.name = teams[teamInt].name;
        }
        
        int friendready = 1;
        for (int s = 0; s < spawnAmmount; s++)
        {
            int listLength = soldiers.Count;
            GameObject workPF = null;
            spawnUnits = false;
            if (teamInt == 1)
            {
                workPF = Instantiate(romanPF, CalculateSafePosition(safeTeamPositions[0],safeTeamPositions[1]), Quaternion.identity);
            }else if (teamInt == 2)
            {
                workPF = Instantiate(vikingPF,  CalculateSafePosition(safeTeamPositions[2],safeTeamPositions[3]), Quaternion.identity);
            }
            Soldier workSoldier = workPF.AddComponent<Soldier>();
            workSoldier.agent = workSoldier.GetComponent<NavMeshAgent>();
            workSoldier.agent.speed = moveSpeed;
            workSoldier.health = health;
            workSoldier.unitType = unitType.indifferent;
            workSoldier.minDistanceToTarget = 1f;
            workSoldier.name = listLength + " Team " + teamInt;
            //workSoldier.name += " Team " + teamInt;
            workSoldier.morale = soldierStartMorale;

            if (friendready == 2)
            {
                workSoldier.friend = soldiers[listLength - 1];
                soldiers[listLength - 1].friend = workSoldier;
                friendready = 0;
            }

            workSoldier.animator = workPF.GetComponent<Animator>();
            soldiers.Add(workSoldier);
            selectedGroup.units.Add((Soldier)workSoldier);
            choiceManager.GiveDefaultChoice(workSoldier);
            choiceManager.soldiers.Add(workSoldier);
            if (spawnEnemy)
            {
                workSoldier.isEnemy = true;
            }

            if (teamInt == 1)
            {
                workSoldier.transform.position = CalculateSafePosition(safeTeamPositions[0], safeTeamPositions[1]);
                workSoldier.safePosition = transform.position;
            } else if (teamInt == 2)
            {
                workSoldier.transform.position = CalculateSafePosition(safeTeamPositions[2], safeTeamPositions[3]);
                workSoldier.safePosition = transform.position;
            } else if (teamInt == 3)
            {
                workSoldier.transform.position = CalculateSafePosition(safeTeamPositions[4], safeTeamPositions[5]);
                workSoldier.safePosition = transform.position;
            }
            friendready++;
        }

        if (spawnEnemy)
        {
            teams[teamInt].isEnemy = true;
        }
        teams[teamInt].StartUp();
        formationManager.StartDefaultFormation(teams[teamInt]);
        uiGroupManager.AddGroup(teams[teamInt]);
        
        //workTeamList.Clear();
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
