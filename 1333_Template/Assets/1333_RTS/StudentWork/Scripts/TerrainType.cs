using UnityEngine;

//GridSettings is a ScriptableObject for easy customization of grid dimensions and orientation
[CreateAssetMenu(fileName = "TerrainType", menuName = "Game/TerrainType")]
public class TerrainType : ScriptableObject
{
    [SerializeField] private string _terrainName;
    [SerializeField] private Color _color;
    [SerializeField] private bool _walkable = true;
    [SerializeField] private int _movementCost = 1;
    [SerializeField] private Texture2D _terrainTexture;

    public string TerrainName => _terrainName;
    public Color Color => _color;
    public bool Walkable => _walkable;
    public int MovementCost => _movementCost;
    public Texture2D TerrainTexture => _terrainTexture;
}
