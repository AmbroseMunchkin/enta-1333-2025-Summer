namespace RTS_1333
{
    /// <summary>
    /// Represents a single entry in the army: a unit type, its prefab, and how many to spawn.
    /// </summary>
    [System.Serializable]
    public class UnitEntry
    {
        // The type+prefab pairing for this entry.
        public UnitTypePrefab unitTypePrefab;
        // How many of this type in the army.
        public int count = 1;
    }
}
