using UnityEngine;

namespace RTS_1333
{
	/// <summary>
	/// Represents a specific building instance in the game, derived from BuildingBase.
	/// </summary>
	public class BuildingInstance : BuildingBase
	{
		/// <summary>
		/// Initializes the building with a given BuildingType.
		/// </summary>
		public override void Initialize(BuildingType type, Vector2Int origin)
		{
			BuildingType = type;
			Origin = origin;
			CurrentHp = type.MaxHp;
		}
	}
}
