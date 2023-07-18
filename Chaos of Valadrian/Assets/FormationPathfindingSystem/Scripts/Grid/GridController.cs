using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public NodeGrid curFlowField;
	public GridDebug gridDebug;

    private void InitializeFlowField()
	{
		if (gridDebug)
			gridDebug.SetFlowField(curFlowField);
	}

	// private void Update()
	// {
	// 	if (Input.GetMouseButtonDown(0))
	// 	{
	// 		InitializeFlowField();
	//
	// 		curFlowField.CreateCostField();
	//
	// 		Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
	// 		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
	// 		//Cell destinationCell = curFlowField.GetCellFromWorldPos(worldMousePos);
	// 		//curFlowField.CreateIntegrationField(destinationCell);
	//
	// 		curFlowField.CreateFlowField();
	//
	// 		if(gridDebug)
	// 			gridDebug.DrawFlowField();
	// 	}
	// }
}
