using System;
using UnityEngine;

namespace RTS_1333
{
    /// <summary>
    /// Abstract base class for all buildings in the game.
    /// </summary>
    public abstract class BuildingBase : MonoBehaviour
    {
        /// <summary>
        /// The type of this building (ScriptableObject containing building data).
        /// </summary>
        [SerializeField] protected BuildingType BuildingType;
		[SerializeField] protected int CurrentHp;
		[SerializeField] protected Vector2Int Origin;
		protected IArmyData OwningArmy;
		
		public BuildingType Type => BuildingType;
		
		public int Hp => CurrentHp;
		public int ArmyId => OwningArmy.ArmyID;
		public Vector2Int OriginPoint => Origin;
		
		public abstract void Initialize(BuildingType buildingType, Vector2Int origin);
		
		public IArmyData ParentArmy => OwningArmy;

		public virtual void AssignToArmy(IArmyData army)
		{
			OwningArmy = army;
		}

		public virtual void OnDestroy()
		{
			ParentArmy.RemoveBuilding(this);
		}
	}
} 