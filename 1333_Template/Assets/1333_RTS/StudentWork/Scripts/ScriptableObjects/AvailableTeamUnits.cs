using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    // SO that tracks what units a army can spawn
    [CreateAssetMenu(fileName = "AvailableTeamUnits", menuName = "Game/Available Team Units")]
    public class AvailableTeamUnits : ScriptableObject
    {
        [SerializeField] public List<UnitType> AvailableUnits = new();
    }
}
