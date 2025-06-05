using IngameDebugConsole;
using UnityEngine;

namespace RTS_1333
{
	public class DebugCommands : MonoBehaviour
	{
		private void OnEnable()
		{
			DebugLogConsole.AddCommand<string, int, float, float>("SpawnUnit", "Spawn a unit of a type, into a specific army, at a given x,z coordinate", ArmySpawn, "Unit Type", "Army ID", "X", "Z");
			DebugLogConsole.AddCommand<int>("NewGame", "Begins a new game, initializing armies and units", StartNewGame, "Number of armies");
		}

		private void OnDisable()
		{
			DebugLogConsole.RemoveCommand("SpawnUnit");
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

		private static void StartNewGame(int numOfArmies)
		{
			GameManager.Instance.StartNewGame(numOfArmies);
		}
	}
}