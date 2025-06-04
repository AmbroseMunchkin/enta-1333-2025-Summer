using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class CurrentTeamArmyManager : MonoBehaviour
    {
        private AvailableTeamUnits _spawnableUnits;
        
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
        public List<UnitBase> CurrentlyActiveUnits = new ();

        public void Initialize(AvailableTeamUnits spawnableUnits)
        {
            _spawnableUnits = spawnableUnits;
        }
        
        /// <summary>
        /// Tells all units to tick
        /// </summary>
        public void UpdateAllUnits()
        {
            foreach (UnitBase unit in CurrentlyActiveUnits)
            {
                unit.Tick();
            }
        }

        public void SpawnUnit()
        {
            Instantiate(
                _spawnableUnits.AvailableUnits[Random.Range(0, _spawnableUnits.AvailableUnits.Count - 1)].Prefab,
                transform);
        }
    }
}
