using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    /// <summary>
    /// Abstract base class for all units in the game.
    /// </summary>
    public abstract class UnitBase : MonoBehaviour
    {
        /// <summary>
        /// The type of this unit (ScriptableObject containing unit data).
        /// </summary>
        [SerializeField] protected UnitType _unitType;

        /// <summary>
        /// The width of the unit in grid cells (for large units).
        /// </summary>
        public virtual int Width => _unitType != null ? _unitType.Width : 1;

        /// <summary>
        /// The height of the unit in grid cells (for large units).
        /// </summary>
        public virtual int Height => _unitType != null ? _unitType.Height : 1;
        
        protected Pathfinder _pathfinder; // Reference to the Pathfinder.
        protected List<GridNode> _currentPath = new(); // The current path to follow.
        protected int _pathIndex = 0; // Current waypoint index.
        
        protected Vector3? _targetWorldPosition = null; // The current target position.
        protected bool _isMoving = false; // Is the unit currently moving?

        // Public property to check if the unit is currently moving.
        public bool IsMoving => _isMoving;

        // Public property to expose the current path for visualization.
        public List<GridNode> CurrentPath => _currentPath;

        protected UnitState State;
        
        /// <summary>
        /// Moves the unit to the specified grid node.
        /// </summary>
        /// <param name="targetNode">The target node to move to.</param>
        public abstract void MoveTo(GridNode targetNode);

        public virtual void Tick()
        {
            switch (State)
            {
                case UnitState.Moving:
                    DoMove();
                    break;
                case UnitState.Attacking:
                    
                    break;
            }
        }

        public virtual void DoMove()
        {
            // If not moving or no path, do nothing.
            if (!_isMoving || _currentPath == null || _currentPath.Count == 0 || _pathIndex >= _currentPath.Count)
                return;

            // Get the next waypoint.
            Vector3 nextWaypoint = _currentPath[_pathIndex].WorldPosition;
            // Move towards the waypoint.
            Vector3 direction = (nextWaypoint - transform.position).normalized;
            float step = _unitType.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, step);

            // Check if reached the waypoint.
            if (Vector3.Distance(transform.position, nextWaypoint) < 0.05f)
            {
                _pathIndex++;
                // If reached the end of the path, stop moving.
                if (_pathIndex >= _currentPath.Count)
                {
                    _isMoving = false;
                }
            }
        }
    }
} 