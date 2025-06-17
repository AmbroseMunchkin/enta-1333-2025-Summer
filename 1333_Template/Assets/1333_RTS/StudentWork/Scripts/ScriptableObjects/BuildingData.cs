using UnityEngine;

namespace RTS_1333
{
	[CreateAssetMenu(fileName = "BuildingData", menuName = "Game/Building Data")]
	public class BuildingData : ScriptableObject
	{
		[SerializeField] private int _width = 1;
		[SerializeField] private int _height = 1;
		[SerializeField] private int _maxHp = 100;
		[SerializeField] private int _armor = 10;
		[SerializeField] private int _moveCost = -1;
		[SerializeField] private BuildingType _function;
		[SerializeField] private GameObject _ghostPrefab;
		[SerializeField] private BuildingInstance _buildingPrefab;
		[SerializeField] private Sprite _buildingSprite;
		[SerializeField] private string _buildingName;

		public int Width => _width;
		public int Height => _height;
		public int MaxHp => _maxHp;
		public int CurrentArmor => _armor;
		public bool IsSolid => _moveCost < 0;
		public GameObject GhostPrefab => _ghostPrefab;
		public BuildingInstance Prefab => _buildingPrefab;
		public BuildingType Function => _function;
		public Sprite BuildingSprite => _buildingSprite;
		public string BuildingName => _buildingName;
	}
}
