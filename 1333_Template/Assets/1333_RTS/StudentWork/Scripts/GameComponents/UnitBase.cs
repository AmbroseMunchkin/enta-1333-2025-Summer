using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
	/// <summary>
	///     Abstract base class for all units in the game.
	/// </summary>
	public abstract class UnitBase : MonoBehaviour
	{
		/// <summary>
		///     The type of this unit (ScriptableObject containing unit data).
		/// </summary>
		[SerializeField] protected UnitType _unitType;

		// Public property to check if the unit is currently moving.
		public bool IsMoving;
		public List<GridNode> CurrentPath = new(); // The current path to follow.
		protected int CurrentHp;

		protected Pathfinder Pathfinder; // Reference to the Pathfinder.
		protected int PathIndex; // Current waypoint index.

		protected UnitState State;

		/// <summary>
		///     The width of the unit in grid cells (for large units).
		/// </summary>
		public virtual int Width => _unitType != null ? _unitType.Width : 1;

		/// <summary>
		///     The height of the unit in grid cells (for large units).
		/// </summary>
		public virtual int Height => _unitType != null ? _unitType.Height : 1;

		public int MaxHp => _unitType.MaxHp;
		public int Hp => CurrentHp;
		public bool IsDead => CurrentHp <= 0;

		/// <summary>
		///     Moves the unit to the specified grid node.
		/// </summary>
		/// <param name="targetNode">The target node to move to.</param>
		public abstract void MoveTo(GridNode targetNode);

		public virtual void Tick()
		{
			CheckState();
			switch (State)
			{
				case UnitState.Moving:
					DoMove();
					break;
				case UnitState.Attacking:

					break;
			}
		}

		protected virtual void CheckState()
		{
			if (CurrentPath == null || CurrentPath.Count == 0 || PathIndex >= CurrentPath.Count)
			{
				State = UnitState.Nothing; // Reset state when not moving
				return;
			}

			// Set state to moving
			State = UnitState.Moving;
		}

		public virtual void DoMove()
		{
			// If not moving or no path, do nothing.
			if (!IsMoving || CurrentPath == null || CurrentPath.Count == 0 || PathIndex >= CurrentPath.Count)
			{
				State = UnitState.Nothing; // Reset state when not moving
				return;
			}

			// Set state to moving
			State = UnitState.Moving;

			// Get the next waypoint
			var nextWaypoint = CurrentPath[PathIndex].WorldPosition;

			// Calculate direction to waypoint
			var direction = (nextWaypoint - transform.position).normalized;

			// Calculate rotation
			var targetRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(
				transform.rotation, targetRotation, _unitType.MoveSpeed * 5f * Time.deltaTime);

			// Move towards waypoint
			var step = _unitType.MoveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, step);

			// Check if reached the waypoint
			if (Vector3.Distance(transform.position, nextWaypoint) < 0.05f)
			{
				PathIndex++;
				// If reached the end of the path, stop moving
				if (PathIndex >= CurrentPath.Count)
				{
					IsMoving = false;
					State = UnitState.Nothing;
				}
			}
		}

		/// <summary>
		///     Sets a new movement target for the unit (world position).
		/// </summary>
		public virtual void SetTarget(Vector3 worldPosition)
		{
			// Request a path from Pathfinder.
			CurrentPath = Pathfinder.FindPath(transform.position, worldPosition, Width, Height);
			PathIndex = 0;
			IsMoving = CurrentPath != null && CurrentPath.Count > 1;
		}
	}
}
