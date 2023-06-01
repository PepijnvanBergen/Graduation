using UnityEditor;
using UnityEngine;


public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
	public bool displayGrid;

	public FlowFieldDisplayType curDisplayType;

	private Vector2Int gridSize;
	private float nodeRadius;
	public NodeGrid curGrid;

	private Sprite[] ffIcons;

	private void Start()
	{
		ffIcons = Resources.LoadAll<Sprite>("Sprites/FFicons");
	}

	public void SetFlowField(NodeGrid newFlowField)
	{
		curGrid = newFlowField;
		nodeRadius = newFlowField.nodeRadius;
		Vector2Int betGridSize = new Vector2Int((int)newFlowField.gridWorldSize.x, (int)newFlowField.gridWorldSize.y);
		gridSize = betGridSize;
	}
	
	public void DrawFlowField()
	{
		ClearNodeDisplay();

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.AllIcons:
				DisplayAllNodes();
				break;

			case FlowFieldDisplayType.DestinationIcon:
				DisplayDestinationNode();
				break;

			default:
				break;
		}
	}

	private void DisplayAllNodes()
	{
		if (curGrid == null) { return; }
		foreach (Node curNode in curGrid.nodeGrid)
		{
			DisplayNode(curNode);
		}
	}

	private void DisplayDestinationNode()
	{
		if (curGrid == null) { return; }
		DisplayNode(curGrid.targetNode);
	}

	private void DisplayNode(Node _node)
	{
		GameObject iconGO = new GameObject();
		SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
		iconGO.transform.parent = transform;
		iconGO.transform.position = _node.worldPoint;

		if (_node.cost == 0)
		{
			iconSR.sprite = ffIcons[3];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.cost == byte.MaxValue)
		{
			iconSR.sprite = ffIcons[2];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.North)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.South)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 180, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.East)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 90, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.West)
		{
			iconSR.sprite = ffIcons[0];
			Quaternion newRot = Quaternion.Euler(90, 270, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.NorthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 0, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.NorthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 270, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.SouthEast)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 90, 0);
			iconGO.transform.rotation = newRot;
		}
		else if (_node.bestDirection == GridDirection.SouthWest)
		{
			iconSR.sprite = ffIcons[1];
			Quaternion newRot = Quaternion.Euler(90, 180, 0);
			iconGO.transform.rotation = newRot;
		}
		else
		{
			iconSR.sprite = ffIcons[0];
		}
	}

	public void ClearNodeDisplay()
	{
		foreach (Transform t in transform)
		{
			GameObject.Destroy(t.gameObject);
		}
	}

	private void OnDrawGizmos()
	{
		if (curGrid == null)
		{
			return;
		}

		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.MiddleCenter;

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.CostField:

				foreach (Node curNode in curGrid.nodeGrid)
				{
					Handles.Label(curNode.worldPoint, curNode.cost.ToString(), style);
				}

				break;

			case FlowFieldDisplayType.IntegrationField:

				foreach (Node curNode in curGrid.nodeGrid)
				{
					Handles.Label(curNode.worldPoint, curNode.bestCost.ToString(), style);
				}

				break;

			default:
				break;
		}
	}
}
