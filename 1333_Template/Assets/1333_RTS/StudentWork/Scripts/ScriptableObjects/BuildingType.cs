using UnityEngine;

namespace RTS_1333
{
	[CreateAssetMenu(fileName = "BuildingType", menuName = "Game/Building Type")]
	public class BuildingType : ScriptableObject
	{
		[SerializeField] private int _width = 1;
		[SerializeField] private int _height = 1;
		[SerializeField] private int _maxHp = 100;
		[SerializeField] private int _moveCost = -1;
		[SerializeField] private GameObject _ghostPrefab;
		[SerializeField] private BuildingInstance _buildingPrefab;

		public int Width => _width;
		public int Height => _height;
		public int MaxHp => _maxHp;
		public bool IsSolid => _moveCost < 0;
		public GameObject GhostPrefab => _ghostPrefab;
		public BuildingInstance Prefab => _buildingPrefab;
	}
}