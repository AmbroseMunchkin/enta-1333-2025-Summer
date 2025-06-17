using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace RTS_1333
{
	/// <summary>
	///     This is the controller for placing buildings.
	///     It receives input via the <see cref="BuildingPlacementUI" />
	/// </summary>
	public class BuildingPlacementManager : MonoBehaviour
	{
		public static BuildingPlacementManager Instance;
		[SerializeField] private BuildingTypesSo AllBuildingData;

		[SerializeField] private ParticleSystem PlacementParticle;

		[SerializeField] private Material CanPlaceMaterial;

		[SerializeField] private Material CannotPlaceMaterial;

		[SerializeField] private LayerMask BuildingMask;

		[SerializeField] private DismantleBuildingUI DismantleUI;

		[FormerlySerializedAs("_gameManager")] [SerializeField]
		private GameManager GameManager;

		private bool _allowPlace;

		private BuildingData _buildingToPlace;

		/// <summary>
		///     If we have a <see cref="_buildingToPlace" /> then show the ghost for it at the mouse position
		///     This will need to calculate where ground is.
		/// </summary>
		private readonly Dictionary<string, GameObject> _ghostObjects = new();

		private ArmyData _localPlayerArmy;
		private GameObject _placementGhost;

		public BuildingTypesSo AllBuildings => AllBuildingData;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				return;
			}

			Debug.LogError("Too Many BuildingPlacementManagers!");
			Destroy(gameObject);
		}

		private void Update()
		{
			if (!_allowPlace) return;

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (_buildingToPlace == null) return;
			/*
				if (Input.GetMouseButtonDown(0))
				{
					if (Physics.Raycast(ray, out hitInfo, 20000, BuildingMask))
					{
						Debug.Log(hitInfo.collider.name);
						var buildingClicked = hitInfo.transform.GetComponentInParent<BuildingBase>();
						DismantleUI.Open(buildingClicked);
					}
				}*/
			// Define a plane at y = 0, facing up
			var groundPlane = new Plane(Vector3.up, Vector3.zero);

			if (groundPlane.Raycast(ray, out var enter))
			{
				if (_placementGhost != null) _placementGhost.SetActive(false);

				var hitPoint = ray.GetPoint(enter);
				var pos = GameManager.Grid.GetNodeFromWorldPositionRespectingBuildingSize(hitPoint, _buildingToPlace);

				if (_ghostObjects.TryGetValue(_buildingToPlace.GhostPrefab.name, out var existingGhost))
				{
					_placementGhost = existingGhost;
					_placementGhost.SetActive(true);
					CheckValidPlacement(pos);
				}
				else
				{
					_placementGhost = Instantiate(_buildingToPlace.GhostPrefab, transform);
					_ghostObjects.Add(_buildingToPlace.GhostPrefab.name, _placementGhost);
					CheckValidPlacement(pos);
				}

				_placementGhost.transform.position = pos.WorldPosition;

				if (Input.GetMouseButtonDown(0))
					if (IsValidLocation(pos))
						PlaceBuilding(pos.WorldPosition);
				// todo play error sfx
			}
		}

		private void OnDrawGizmos()
		{
			if (_buildingToPlace == null) return;

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var groundPlane = new Plane(Vector3.up, Vector3.zero);
			if (groundPlane.Raycast(ray, out var enter))
			{
				var hitPoint = ray.GetPoint(enter);
				var pos = GameManager.Grid.GetNodeFromWorldPositionRespectingBuildingSize(hitPoint, _buildingToPlace);

				for (var xOffset = 0; xOffset < _buildingToPlace.Width; xOffset++)
				{
					for (var yOffset = 0; yOffset < _buildingToPlace.Height; yOffset++)
					{
						var checkX = pos.Coordinates.x + xOffset;
						var checkY = pos.Coordinates.y + yOffset;
						// calculates the node + building width + height
						// If this runs once, then there is only one node to check for a 1x1 building
						// for a 3x3 building, this would run a total of 9 times, checking 9 nodes.

						var checkedNode = GameManager.Grid.GetNode(checkX, checkY);
						// even if just one node is occupied by a building, then the placement is invalid

						Gizmos.color = checkedNode.OccupyingBuilding ? Color.red : Color.green;
						Gizmos.DrawWireCube(checkedNode.WorldPosition, Vector3.one);
					}
				}
			}
		}

		public void Initialize(ArmyData localPlayerArmy)
		{
			if (localPlayerArmy == null)
			{
				Debug.LogError("LocalPlayerArmy is null!");
				return;
			}

			_localPlayerArmy = localPlayerArmy;
			Debug.Log("Initializing BuildingPlacementManager");
			_allowPlace = true;
		}

		/// <summary>
		///     Called by the <see cref="BuildingPlacementUI" />
		/// </summary>
		public void OnNewBuildingSelected(BuildingData building)
		{
			if (!_allowPlace)
			{
				Debug.LogError(
					"Tried to set a building to place but BuildingPlacementManager has not been initialized!");
				return;
			}

			_buildingToPlace = building;
		}

		/// <summary>
		///     Places a building at the designated location
		/// </summary>
		/// <param name="position"></param>
		private void PlaceBuilding(Vector3 position)
		{
			PlacementParticle.transform.position = position;
			PlacementParticle.Play();

			var placedBuilding = Instantiate<BuildingBase>(
				_buildingToPlace.Prefab, position, Quaternion.identity);

			_localPlayerArmy.AddBuilding(placedBuilding);

			var pos = GameManager.Grid.GetNodeFromWorldPositionRespectingBuildingSize(position, _buildingToPlace);
			for (var xOffset = 0; xOffset < _buildingToPlace.Width; xOffset++)
			{
				for (var yOffset = 0; yOffset < _buildingToPlace.Height; yOffset++)
				{
					var checkX = pos.Coordinates.x + xOffset;
					var checkY = pos.Coordinates.y + yOffset;
					// calculates the node + building width + height
					// If this runs once, then there is only one node to check for a 1x1 building
					// for a 3x3 building, this would run a total of 9 times, checking 9 nodes.

					var checkedNode = GameManager.Grid.GetNode(checkX, checkY);
					checkedNode.OccupyingBuilding = placedBuilding;
					GameManager.Grid.SaveNode(checkedNode, checkX, checkY);
				}
			}

			_placementGhost.SetActive(false);
			_placementGhost = null;
			_buildingToPlace = null;
		}

		private void CheckValidPlacement(GridNode node)
		{
			if (IsValidLocation(node))
				ValidPlacement();
			else
				InvalidPlacement();
		}

		private bool IsValidLocation(GridNode node)
		{
			var isValidLocation = true;
			for (var xOffset = 0; xOffset < _buildingToPlace.Width; xOffset++)
			{
				for (var yOffset = 0; yOffset < _buildingToPlace.Height; yOffset++)
				{
					var checkX = node.Coordinates.x + xOffset;
					var checkY = node.Coordinates.y + yOffset;
					// calculates the node + building width + height
					// If this runs once, then there is only one node to check for a 1x1 building
					// for a 3x3 building, this would run a total of 9 times, checking 9 nodes.

					var checkedNode = GameManager.Grid.GetNode(checkX, checkY);
					// even if just one node is occupied by a building, then the placement is invalid
					if (checkedNode.OccupyingBuilding != null) return false;
				}
			}

			if (isValidLocation)
			{
				// todo check for units in those spaces
			}

			return isValidLocation;
		}

		private void InvalidPlacement()
		{
			if (_placementGhost != null)
			{
				var mrList = _placementGhost.GetComponentsInChildren<MeshRenderer>();
				foreach (var mr in mrList)
				{
					var mats = mr.materials;
					for (var i = 0; i < mats.Length; i++) mats[i] = CannotPlaceMaterial;

					mr.SetMaterials(mats.ToList());
				}
			}
		}

		private void ValidPlacement()
		{
			if (_placementGhost != null)
			{
				var mrList = _placementGhost.GetComponentsInChildren<MeshRenderer>();
				foreach (var mr in mrList)
				{
					var mats = mr.materials;
					for (var i = 0; i < mats.Length; i++) mats[i] = CanPlaceMaterial;

					mr.SetMaterials(mats.ToList());
				}
			}
		}

		public void SetLocalBuildingManager(ArmyData playerBuildingManager)
		{
			_localPlayerArmy = playerBuildingManager;
		}

		public void TogglePlacement(bool canPlace)
		{
			_allowPlace = canPlace;
		}

		internal void SetGameManager(GameManager gameManager)
		{
			GameManager = gameManager;
		}


		public void PlaceBuilding(BuildingData typeToPlace, ArmyData arny)
		{
		}
	}
}
