// Import required namespaces

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
// Provides classes for generic collection data structures like List.

// Provides access to Unity's core classes and functionality.

namespace RTS_1333
{
	/// <summary>
	///     The GridManager class is responsible for managing a grid system for the game.
	///     It creates nodes, allows retrieval and modification of node properties, and manages terrain generation.
	/// </summary>
	public class GridManager : MonoBehaviour
	{
		/// <summary>
		///     Stores the configuration settings for the grid system, such as its dimensions, node size,
		///     and orientation. This determines how the grid is initialized and functions in the game.
		/// </summary>
		[SerializeField]
		private GridSettings _gridSettings; // Holds grid parameters like dimensions, node size, and orientation.

		/// <summary>
		///     A list containing references to all GridNode objects in the grid.
		///     This list is primarily used for debugging purposes in the Unity editor.
		/// </summary>
		[Header("Debug for editor playmode only")] // Adds a header in the Unity inspector.
		[SerializeField]
		private List<GridNode> AllNodes = new(); // Contains all grid nodes in a single list for easier debugging.

		/// <summary>
		///     Tracks buildings that occupy grid nodes.
		/// </summary>
		private readonly Dictionary<Vector2Int, BuildingInstance> _buildingOccupancy = new();

		/// <summary>
		///     A 2D private array used to store and organize grid nodes, where each element represents a node
		///     at a specific position defined by its row and column indices.
		/// </summary>
		private GridNode[,] _gridNodes;

		/// <summary>
		///     Provides configuration settings for the grid system used in the game.
		/// </summary>
		public GridSettings GridSettings => _gridSettings;

		/// <summary>
		///     Indicates whether the grid system has been successfully initialized.
		/// </summary>
		public bool IsInitialized { get; private set; } // Indicates if the grid data is ready to use.

		/// <summary>
		///     Draws visual representations of the grid nodes in the Unity Editor Scene view using Gizmos to aid in debugging and
		///     development.
		/// </summary>
		/// <remarks>
		///     Unity calls this method. It renders wireframe cubes representing the grid nodes based on their positions and
		///     walkability within the scene.
		///     It is intended to help developers visualize the grid system during development and make adjustments.
		/// </remarks>
		private void OnDrawGizmos()
		{
			// If grid is not initialized or gridSettings is missing, abort visualization.
			if (_gridNodes == null || _gridSettings == null) return;

			// Loop through each grid cell to draw its representation.
			for (var x = 0; x < _gridSettings.GridSizeX; x++)
			{
				for (var y = 0; y < _gridSettings.GridSizeY; y++)
				{
					// Retrieve the node for the current coordinates.
					var node = _gridNodes[x, y];

					// Change Gizmos color based on walkability: green for walkable, red for non-walkable.
					Gizmos.color = node.GizmoColor;

					// Draw a wireframe cube at the node's world position.
					Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * _gridSettings.NodeSize * 0.9f);
				}
			}

			// Calls the debug list to be created
			PopulateDebugList();
		}

		/// <summary>
		///     Initializes the grid and populates it with GridNode objects using configured grid settings.
		/// </summary>
		public void InitializeGrid()
		{
			// Create a 2D array for the grid using dimensions defined in GridSettings.
			_gridNodes = new GridNode[_gridSettings.GridSizeX, _gridSettings.GridSizeY];

			// Loop through each grid cell in X and Y directions.
			for (var x = 0; x < _gridSettings.GridSizeX; x++) // Iterate over X-axis.
			{
				for (var y = 0; y < _gridSettings.GridSizeY; y++) // Iterate over Y-axis.
				{
					// Calculate the world position of the current node based on grid orientation.
					var worldPos = _gridSettings.UseXZPlane ?
						new Vector3(x, 0, y) * _gridSettings.NodeSize // Position for XZ plane configuration.
						:
						new Vector3(x, y, 0) * _gridSettings.NodeSize; // Position for XY plane configuration.

					// Create a new grid node and set its properties.
					var node = new GridNode
					{
						Name = $"Cell_{x + _gridSettings.GridSizeX * x + y}", // Create a unique name for the node.
						Coordinates =
							Vector2Int.RoundToInt(
								new Vector2(worldPos.x, worldPos.z)), // Calculate coordinates based on world position.
						WorldPosition = worldPos, // Assign calculated world position.
						Walkable = _gridSettings.DefaultTerrainType.Walkable, // Default value: node is walkable.
						Weight = _gridSettings.DefaultTerrainType
							.MovementCost, // Default value: node has neutral weight.
						GizmoColor =
							_gridSettings.DefaultTerrainType.GizmoColor, // Default value: node has default color.
						TerrainType = _gridSettings.DefaultTerrainType // Default value: node has default terrain type.
					};

					// Store the node in the 2D grid array at the correct position.
					_gridNodes[x, y] = node;
				}
			}

			// Mark the grid as initialized upon successful completion.
			IsInitialized = true;
		}

		/// <summary>
		///     Updates the debug list in the Unity inspector to include all nodes in the grid.
		///     This is used for debugging and visualization during play mode.
		/// </summary>
		private void PopulateDebugList()
		{
			// Clear the existing list to avoid duplication of nodes.
			AllNodes.Clear();

			// Loop through each grid cell to populate the list.
			for (var x = 0; x < _gridSettings.GridSizeX; x++)
			{
				for (var y = 0; y < _gridSettings.GridSizeY; y++)
				{
					// Retrieve the node from the grid array and add it to the list.
					var node = _gridNodes[x, y];
					AllNodes.Add(
						new GridNode
						{
							Name = $"Cell_{x}_{y}", // Name the node based on its position.
							Coordinates = node.Coordinates, // Copy coordinates.
							WorldPosition = node.WorldPosition, // Copy world position.
							Walkable = node.Walkable, // Copy walkability status.
							Weight = node.Weight, // Copy weight value.
							GizmoColor = node.GizmoColor, // Copy Gizmo color.
							TerrainType = node.TerrainType // Copy terrain type.
						});
				}
			}
		}

		/// <summary>
		///     Retrieves the GridNode object at the specified grid coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the node to retrieve. Represents the column index in the grid.</param>
		/// <param name="y">The y-coordinate of the node to retrieve. Represents the row index in the grid.</param>
		/// <returns>The GridNode object located at the specified coordinates within the grid.</returns>
		public GridNode GetNode(int x, int y)
		{
			// Ensure the requested coordinates are within grid bounds.
			if (x < 0 || x >= _gridSettings.GridSizeX || y < 0 || y >= _gridSettings.GridSizeY)
				throw new IndexOutOfRangeException(
					"Grid node indices out of range."); // Throw an error if out of bounds.

			// Return the requested node.
			return _gridNodes[x, y];
		}

		public void SaveNode(GridNode newValues, int x, int y)
		{
			// Ensure the requested coordinates are within grid bounds.
			if (x < 0 || x >= _gridSettings.GridSizeX || y < 0 || y >= _gridSettings.GridSizeY)
				throw new IndexOutOfRangeException(
					"Grid node indices out of range."); // Throw an error if out of bounds.

			_gridNodes[x, y] = newValues;
		}

		/// <summary>
		///     Updates the walkable state for a specific node within the grid.
		/// </summary>
		/// <param name="x">The x-coordinate of the grid node to modify.</param>
		/// <param name="y">The y-coordinate of the grid node to modify.</param>
		/// <param name="walkable">A boolean indicating whether the node should be walkable (true) or non-walkable (false).</param>
		public void SetWalkable(int x, int y, bool walkable)
		{
			// Update the walkability state of the node at the given position.
			_gridNodes[x, y].Walkable = walkable;
		}

		/// <summary>
		///     Randomizes the terrain type for all nodes within the grid.
		/// </summary>
		/// <remarks>
		///     This method iterates through each node in the grid and assigns it a random terrain type
		///     chosen from the list of TerrainTypes in the GridSettings.
		/// </remarks>
		public void RandomizeTerrain()
		{
			var gridSizeX = GridSettings.GridSizeX;
			var gridSizeY = GridSettings.GridSizeY;

			for (var x = 0; x < gridSizeX; x++)
			{
				for (var y = 0; y < gridSizeY; y++)
				{
					var randomTerrain =
						GridSettings.TerrainTypes[Random.Range(0, GridSettings.TerrainTypes.Length)];
					SetTerrainType(x, y, randomTerrain);
				}
			}
		}

		/// <summary>
		///     Updates the terrain type for a specific node in the grid, along with its associated properties.
		/// </summary>
		/// <param name="x">
		///     The x-coordinate of the grid node to update. Must be a valid coordinate within the grid.
		/// </param>
		/// <param name="y">
		///     The y-coordinate of the grid node to update. Must be a valid coordinate within the grid.
		/// </param>
		/// <param name="terrainType">
		///     The new TerrainType to assign to the grid node. This determines the node's walkability, movement cost, and visual
		///     representation.
		/// </param>
		private void SetTerrainType(int x, int y, TerrainType terrainType)
		{
			if (!IsValidCoordinate(x, y)) return;

			var node = _gridNodes[x, y];
			node.TerrainType = terrainType;
			node.Walkable = terrainType.Walkable;
			node.Weight = terrainType.MovementCost;
			node.GizmoColor = terrainType.GizmoColor;
			_gridNodes[x, y] = node;
		}


		/// <summary>
		///     Validates if the given x and y coordinates are within the bounds of the grid.
		/// </summary>
		/// <param name="x">The x-coordinate to validate. Must be non-negative and less than GridSizeX.</param>
		/// <param name="y">The y-coordinate to validate. Must be non-negative and less than GridSizeY.</param>
		/// <returns>
		///     Returns true if the provided coordinates are within the valid range of the grid dimensions; otherwise, false.
		/// </returns>
		private bool IsValidCoordinate(int x, int y)
		{
			return x >= 0 && x < _gridSettings.GridSizeX && y >= 0 && y < _gridSettings.GridSizeY;
		}

		/// <summary>
		///     Converts a world position to the nearest valid grid node.
		/// </summary>
		/// <param name="position">The world position to convert.</param>
		/// <returns>The closest valid GridNode, or throws if out of bounds.</returns>
		public GridNode GetNodeFromWorldPosition(Vector3 position)
		{
			// Determine which axes to use based on grid orientation.
			var x = _gridSettings.UseXZPlane ?
				Mathf.RoundToInt(position.x / _gridSettings.NodeSize) :
				Mathf.RoundToInt(position.x / _gridSettings.NodeSize);
			var y = _gridSettings.UseXZPlane ?
				Mathf.RoundToInt(position.z / _gridSettings.NodeSize) :
				Mathf.RoundToInt(position.y / _gridSettings.NodeSize);
			// Clamp coordinates to grid bounds.
			x = Mathf.Clamp(x, 0, _gridSettings.GridSizeX - 1);
			y = Mathf.Clamp(y, 0, _gridSettings.GridSizeY - 1);
			// Return the node at the clamped coordinates.
			return GetNode(x, y);
		}

		/// <summary>
		///     Converts a world position to the nearest valid grid node.
		/// </summary>
		/// <param name="position">The world position to convert.</param>
		/// <param name="building"></param>
		/// <returns>The closest valid GridNode, or throws if out of bounds.</returns>
		public GridNode GetNodeFromWorldPositionRespectingBuildingSize(Vector3 position, BuildingData building)
		{
			// Determine which axes to use based on grid orientation.
			var x = _gridSettings.UseXZPlane ?
				Mathf.RoundToInt(position.x / _gridSettings.NodeSize) :
				Mathf.RoundToInt(position.x / _gridSettings.NodeSize);
			var y = _gridSettings.UseXZPlane ?
				Mathf.RoundToInt(position.z / _gridSettings.NodeSize) :
				Mathf.RoundToInt(position.y / _gridSettings.NodeSize);
			// Clamp coordinates to grid bounds.
			x = Mathf.Clamp(x, 0, _gridSettings.GridSizeX - building.Width);
			y = Mathf.Clamp(y, 0, _gridSettings.GridSizeY - building.Height);
			// Return the node at the clamped coordinates.
			return GetNode(x, y);
		}


		public bool CanPlaceBuilding(BuildingData data, Vector2Int origin)
		{
			for (var dx = 0; dx < data.Width; dx++)
			{
				for (var dy = 0; dy < data.Height; dy++)
				{
					var pos = origin + new Vector2Int(dx, dy);
					if (!IsValidCoordinate(pos.x, pos.y) || _buildingOccupancy.ContainsKey(pos))
						return false;
				}
			}

			return true;
		}

		public void PlaceBuilding(BuildingInstance instance)
		{
			var origin = instance.OriginPoint;
			var data = instance.Data;
			for (var dx = 0; dx < data.Width; dx++)
			{
				for (var dy = 0; dy < data.Height; dy++)
				{
					var pos = origin + new Vector2Int(dx, dy);
					_buildingOccupancy[pos] = instance;
					if (data.IsSolid)
						SetWalkable(pos.x, pos.y, false);
				}
			}
		}

		public void RemoveBuilding(BuildingBase instance)
		{
			var origin = instance.OriginPoint;
			var data = instance.Data;
			for (var dx = 0; dx < data.Width; dx++)
			{
				for (var dy = 0; dy < data.Height; dy++)
				{
					var pos = origin + new Vector2Int(dx, dy);
					_buildingOccupancy.Remove(pos);
					if (data.IsSolid)
						SetWalkable(pos.x, pos.y, true);
				}
			}
		}

		public bool IsOccupied(Vector2Int pos)
		{
			return _buildingOccupancy.ContainsKey(pos);
		}
	}
}
