using UnityEngine;

namespace RTS_1333
{
	public class BarracksInstance : MonoBehaviour
	{
		[SerializeField] private BuildingInstance BuildingInstance;
		[SerializeField] private UnitType UnitData;

		[SerializeField] private Transform SpawnLocator;

		// todo allow the player to left click to select building, right click to set the rally point locator position
		[SerializeField] private Transform RallyPointLocator;
		[SerializeField] private float SpawnTime = 5f;

		private float _spawnTimer;

		/// <summary>
		///     TODO
		///     Allow user to queue units and select their type
		///     Only build units / advance timer if there are enough resources to pay for unit
		/// </summary>
		private void Update()
		{
			_spawnTimer += Time.deltaTime;
			if (_spawnTimer >= SpawnTime)
			{
				SpawnUnit();
				_spawnTimer = 0f;
			}
		}

		private void SpawnUnit()
		{
			if (!AllArmiesManager.Instance.TryGetArmy(
					BuildingInstance.ArmyId, out var army))
			{
				Debug.Log("Army not found");
				return;
			}

			var unit = army.SpawnNewUnit(UnitData, SpawnLocator.position, BuildingInstance.ArmyId);
			unit.SetTarget(RallyPointLocator.position);
		}
	}
}
