using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RTS_1333
{
    /// <summary>
    /// Handles player-controlled placement of buildings, with ghost snapping and visual feedback.
    /// </summary>
    public class BuildingPlacementManager : MonoBehaviour
    {
		[SerializeField] private GridManager _gridManager;
        [SerializeField] private Material _validMaterial;
        [SerializeField] private Material _invalidMaterial;
        [SerializeField] private LayerMask _placementMask;
		
		private static BuildingPlacementManager _instance;
		
        private BuildingType _pendingBuilding;
        private GameObject _ghostObject;
        private MeshRenderer[] _ghostRenderers;
		private ArmyData _playerArmy;
		
		public static BuildingPlacementManager Instance => _instance;

		private void Awake()
		{
			
		}

		/// <summary>
		/// todo Call this from UI
		/// </summary>
		/// <param name="buildingType"></param>
		/// <param name="playerArmy"></param>
		/// <param name="grid"></param>
        public void BeginPlacement(BuildingType buildingType, ArmyData playerArmy)
        {
            _pendingBuilding = buildingType;
            _playerArmy = playerArmy;

            if (_ghostObject != null) Destroy(_ghostObject);
            _ghostObject = Instantiate(buildingType.Prefab.gameObject);
            _ghostObject.SetActive(true);

            _ghostRenderers = _ghostObject.GetComponentsInChildren<MeshRenderer>();
        }

        private void Update()
        {
            if (_pendingBuilding == null || _ghostObject == null) return;

            // Raycast from mouse to grid
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit, 100f, _placementMask)) return;

            Vector2Int snappedCoords = _gridManager.GetNodeFromWorldPosition(hit.point).Coordinates;
            Vector3 snappedWorld = _gridManager.GetNode(snappedCoords.x, snappedCoords.y).WorldPosition;

            _ghostObject.transform.position = snappedWorld;

            bool canPlace = _gridManager.CanPlaceBuilding(_pendingBuilding, snappedCoords);
            SetGhostMaterial(canPlace ? _validMaterial : _invalidMaterial);

            if (canPlace && Mouse.current.leftButton.wasPressedThisFrame)
            {
                _playerArmy.SpawnBuilding(_pendingBuilding, snappedWorld);
                CancelPlacement();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                CancelPlacement();
            }
        }

        private void SetGhostMaterial(Material mat)
        {
            foreach (var r in _ghostRenderers)
            {
                var materials = r.materials;
                for (int i = 0; i < materials.Length; i++)
                    materials[i] = mat;
                r.materials = materials;
            }
        }

        private void CancelPlacement()
        {
            _pendingBuilding = null;
            if (_ghostObject != null)
                Destroy(_ghostObject);
        }
    }
}
