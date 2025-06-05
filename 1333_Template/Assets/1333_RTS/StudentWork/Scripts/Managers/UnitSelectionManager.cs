using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace RTS_1333
{
	public class UnitSelectionManager : MonoBehaviour
	{
		[SerializeField] private GridManager _gridManager;
		[SerializeField] private LayerMask _unitLayer;
		[SerializeField] private Camera _camera;

		private UnitInstance _selectedUnit;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				TrySelectUnit();
			}

			if (Input.GetMouseButtonDown(1))
			{
				TryCommandMove();
			}
		}

		private void TrySelectUnit()
		{
			Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
			if (Physics.Raycast(ray, out RaycastHit hit, 100f, _unitLayer))
			{
				UnitInstance unit = hit.collider.GetComponentInParent<UnitInstance>();
				if (unit != null && unit.Hp > 0)
				{
					_selectedUnit = unit;
					Debug.Log($"Selected unit: {_selectedUnit.name}");
				}
			}
		}

		private void TryCommandMove()
		{
			if (_selectedUnit == null)
				return;

			Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

			// Define a plane at y = 0, facing up
			Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

			if (groundPlane.Raycast(ray, out float enter))
			{
				Vector3 hitPoint = ray.GetPoint(enter);

				GridNode targetNode = _gridManager.GetNodeFromWorldPosition(hitPoint);
				_selectedUnit.MoveTo(targetNode);
				Debug.Log($"Commanded move to: {targetNode.WorldPosition}");
			}
		}

	}
}
