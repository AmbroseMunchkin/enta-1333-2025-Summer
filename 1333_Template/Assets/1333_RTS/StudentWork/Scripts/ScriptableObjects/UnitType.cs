using UnityEngine;

namespace RTS_1333
{
	[CreateAssetMenu(fileName = "UnitType", menuName = "Game/Unit Type")]
	public class UnitType : ScriptableObject
	{
		/// <summary>
		///     The width of the unit in grid cells.
		/// </summary>
		[SerializeField] private int _width = 1;

		/// <summary>
		///     The height of the unit in grid cells.
		/// </summary>
		[SerializeField] private int _height = 1;

		[SerializeField] private int _maxHp = 1;
		[SerializeField] private float _moveSpeed = 1;
		[SerializeField] private int _damage = 1;
		[SerializeField] private int _defence = 1;
		[SerializeField] private AttackType _attackType = AttackType.Melee;
		[SerializeField] private int _range = 1;

		[SerializeField] private UnitInstance _unitPrefab;

		/// <summary>
		///     Public property to get the width of the unit.
		/// </summary>
		public int Width => _width;

		/// <summary>
		///     Public property to get the height of the unit.
		/// </summary>
		public int Height => _height;

		public int MaxHp => _maxHp;
		public float MoveSpeed => _moveSpeed;
		public int Damage => _damage;
		public int Defence => _defence;

		public UnitInstance Prefab => _unitPrefab;
	}
}
