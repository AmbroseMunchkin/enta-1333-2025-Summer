using UnityEngine;
using System.Collections.Generic;

namespace RTS_1333
{
	/// <summary>
	/// Represents a specific unit instance in the game, derived from UnitBase.
	/// </summary>
	public class UnitInstance : UnitBase
    {
        [Header("Prefab Stuff")]
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private GameObject _unitSkins;
        [SerializeField] private ParticleSystem _hurtParticles;

		public void Initialize(Pathfinder pathfinder)
		{
			_pathfinder = pathfinder;
            
            foreach(SkinnedMeshRenderer skin in _unitSkins.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                // change material to match team
            }
		}

		/// <summary>
		/// Sets a new movement target for the unit (world position).
		/// </summary>
		public void SetTarget(Vector3 worldPosition)
		{
			// Store the target.
			_targetWorldPosition = worldPosition;
			// Request a path from Pathfinder.
			_currentPath = _pathfinder.FindPath(transform.position, worldPosition, Width, Height);
			_pathIndex = 0;
			_isMoving = _currentPath != null && _currentPath.Count > 1;
		}

		/// <summary>
		/// Sets a new movement target for the unit (grid node).
		/// </summary>
		public void SetTarget(GridNode node)
		{
			SetTarget(node.WorldPosition);
		}

		/// <summary>
		/// Moves the unit to the specified grid node (required by base class).
		/// </summary>
		public override void MoveTo(GridNode targetNode)
		{
			SetTarget(targetNode);
		}
	}
}
