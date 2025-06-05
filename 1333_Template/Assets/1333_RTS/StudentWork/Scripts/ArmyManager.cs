using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
	/// <summary>
	/// Manages all units and buildings belonging to a single army.
	/// </summary>
	public class ArmyManager
	{
		/// <summary>
		/// The unique ID of this army. Player is always army 0.
		/// </summary>
		public int ArmyID;

		/// <summary>
		/// Returns true if this army is the player army (ID 0).
		/// </summary>
		public bool IsPlayer => ArmyID == 0;

		/// <summary>
		/// List of all units in this army. Uses UnitBase for polymorphism.
		/// </summary>
		public List<UnitInstance> Units = new List<UnitInstance>();

		/// <summary>
		/// List of all buildings in this army. Uses BuildingBase for polymorphism.
		/// </summary>
		public List<BuildingBase> Buildings = new List<BuildingBase>();

		/// <summary>
		/// Reference to the grid manager for node lookups.
		/// </summary>
		public GridManager GridManager;
	}
}