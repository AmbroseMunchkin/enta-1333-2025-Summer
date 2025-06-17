using IngameDebugConsole;
using UnityEngine;

namespace RTS_1333
{
	public class DebugCommands : MonoBehaviour
	{
		[SerializeField] private GameManager _gameManager;

		private void OnEnable()
		{
			DebugLogConsole.AddCommand<int>(
				"StartNewGame", "Begins a new game, initializing armies and units", StartNewGame, "Number of armies");
			DebugLogConsole.AddCommand<string, int, float, float>(
				"SpawnUnit", "Spawn a unit of a type, into a specific army, at a given x,z coordinate", ArmySpawn,
				"Unit Type", "Army ID", "X", "Z");
			DebugLogConsole.AddCommand<string, int>(
				"PlaceBuilding", "Goes into building placement mode", PlaceBuilding, "Type of building to place",
				"Army to place it for.");


			DebugLogConsole.AddCommand("HelloWorld", "Prints a message to the console.", HelloWorld);
			DebugLogConsole.AddCommand<int>("NewGame", "Begins a new game, initializing armies and units", NewGame);
		}

		private void OnDisable()
		{
			DebugLogConsole.RemoveCommand("StartNewGame");
			DebugLogConsole.RemoveCommand("SpawnUnit");
			DebugLogConsole.RemoveCommand("PlaceBuilding");


			DebugLogConsole.RemoveCommand("HelloWorld");
			DebugLogConsole.RemoveCommand("NewGame");
		}


		private void NewGame(int numOfArmies)
		{
			_gameManager.StartNewGame(numOfArmies);
		}

		private void HelloWorld()
		{
			Debug.Log("Hello World");
		}

		private static void StartNewGame(int numOfArmies)
		{
			GameManager.Instance.StartNewGame(numOfArmies);
		}

		private static void ArmySpawn(string unitTypeName, int armyId, float x, float z)
		{
			var unitType = Resources.Load<UnitType>($"UnitTypes/{unitTypeName}");
			if (unitType == null)
			{
				Debug.LogError($"Invalid unit type: {unitTypeName}");
				return;
			}

			if (!AllArmiesManager.Instance.TryGetArmy(armyId, out var army))
			{
				Debug.LogError($"No army registered with ID {armyId}");
				return;
			}

			var data = new UnitData
			{
				UnitType = unitType,
				Position = new Vector3(x, 0, z),
				Health = unitType.MaxHp,
				ArmyId = armyId
			};

			army.SpawnUnit(data);

			Debug.Log($"Spawned {unitTypeName} at ({x}, {z}) in Army {armyId}");
		}


		private static void PlaceBuilding(string buildingTypeName, int armyId)
		{
			var buildingType = Resources.Load<BuildingData>($"BuildingTypes/{buildingTypeName}");
			if (buildingType == null)
			{
				Debug.LogError($"Invalid building type: {buildingTypeName}");
				return;
			}

			if (!AllArmiesManager.Instance.TryGetArmy(armyId, out var army))
			{
				Debug.LogError($"No army registered with ID {armyId}");
				return;
			}

			BuildingPlacementManager.Instance.PlaceBuilding(buildingType, army);
		}
	}
}
