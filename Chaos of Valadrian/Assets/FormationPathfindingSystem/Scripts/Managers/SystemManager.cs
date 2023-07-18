using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private ChoiceManager choiceManager;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private UIGroupManager uIgroupManager;

    public Vector3 firstSelectionPoint;
    public Vector3 secondSelectionPoint;
    private bool firstPoint = false;
    [SerializeField] private LayerMask raycastLayers;

    public int newFormationTest;

    private void Start()
    {
        SetUpLevel1();
    }
    private void Update()
    {
        unitManager.Execute();
        formationManager.Execute();
        if (choiceManager.soldiers.Count > 0)
        {
            choiceManager.Execute();   
        }

        //When time implement a better input system, this sucks.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hitData;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 1000,Color.cyan, 5f);
            if (Physics.Raycast(ray, out hitData, 1000,raycastLayers))
            {
                foreach (Soldier soldier in unitManager.soldiers)
                {
                    if (Vector3.Distance(soldier.transform.position, hitData.point) < 2f)
                    {
                        if (soldier.isEnemy)
                        {
                            uIgroupManager.selectedGroup.group.Attack(soldier.group);
                            Debug.Log("Attack");
                            return;
                        }
                        soldier.group.uiHolder.MakeThisSelectedGroup();
                        Debug.Log(soldier.group + " is new selected group");

                    }
                }
                
                
                // Collider[] colliders = Physics.OverlapSphere(hitData.point, 1f, raycastLayers);
                //  foreach (var collider in colliders)
                // {
                //     Debug.Log(collider.gameObject.name);
                //     Soldier testSoldier = hitData.collider.GetComponent<Soldier>();
                //     if (testSoldier != null)
                //     {
                //
                //
                //         return;
                //     }
                // }

                    // if (hitData.collider.gameObject.CompareTag("Soldier"))
                    // {
                    //     if (hitData.collider.GetComponent<Soldier>().isEnemy)
                    //     {
                    //         uIgroupManager.selectedGroup.group.Attack(hitData.collider.GetComponent<Soldier>().group);
                    //         Debug.Log("Attack");
                    //         return;
                    //     }
                    //     hitData.collider.GetComponent<Soldier>().group.uiHolder.MakeThisSelectedGroup();
                    //     Debug.Log("new group selected");
                    // }
                    
                        firstSelectionPoint = hitData.point;
                        firstPoint = true;
            }

            if (Input.GetKey(KeyCode.H))
            {
                foreach (Soldier soldier in uIgroupManager.selectedGroup.group.units)
                {
                    soldier.morale += 20f;
                }
            }
            
        }
        if (Input.GetMouseButtonUp(0) && firstPoint)
        {
            Ray ray;
            RaycastHit hitData;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //swap with cam?
            if (Physics.Raycast(ray, out hitData, 1000))//,groundLayer))
            {
                if (hitData.collider.gameObject.layer == 5)
                {
                    Debug.Log("hitUI terminate life");
                    return;
                }
                secondSelectionPoint = hitData.point;
                
                if (hitData.collider.name == "Ground" && uIgroupManager.selectedGroup != null)
                {
                    formationManager.MoveFormation(uIgroupManager.selectedGroup.group, firstSelectionPoint, secondSelectionPoint);    
                }

                firstPoint = false;
            }
        }
    }

    public void UIChangeFormation(Group _group, int _formation)
    {
        if (_formation == (int)formationType.loose)
        {
            formationManager.ChangeFormation(formationManager.formations[1], _group);
        } else if (_formation == (int)formationType.line)
        {
            formationManager.ChangeFormation(formationManager.formations[0], _group);
        }else if (_formation == (int)formationType.circle)
        {
            //Debug.Log("first make the circle formation and put it in the formationManager formations list at the third spot.");
            formationManager.ChangeFormation(formationManager.formations[2], _group);
        }
    }

    public void SetUpLevel1()
    {
        unitManager.teamInt = 1;
        unitManager.spawnEnemy = false;
        unitManager.SpawnUnits();
        formationManager.MoveFormation(unitManager.teams[1], unitManager.safeTeamPositions[0].position, unitManager.safeTeamPositions[1].position);
        
        unitManager.teamInt = 2;
        unitManager.spawnEnemy = true;
        unitManager.SpawnUnits();
        
        formationManager.MoveFormation(unitManager.teams[2], unitManager.safeTeamPositions[2].position, unitManager.safeTeamPositions[3].position);
      }
}