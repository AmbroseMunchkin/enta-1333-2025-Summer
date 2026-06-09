using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GridSettings is a ScriptableObject for easy customization of grid dimensions and orientation
[CreateAssetMenu(fileName = "GridSettings", menuName = "Game/GridSettings")]
public class GridSettings : ScriptableObject
{
    [SerializeField] private int _gridSizeX = 10;
    [SerializeField] private int _gridSizeY = 10;
    [SerializeField] private float _nodeSize = 1f;
    [SerializeField] private bool _useXZPlane = true;
    [SerializeField] private bool _allowDiagonal = true;
    [SerializeField] private List<TerrainType> _terrainTypes;

    public int GridSizeX => _gridSizeX;
    public int GridSizeY => _gridSizeY;
    public float NodeSize => _nodeSize;
    public bool UseXZPlane => _useXZPlane;
    public bool AllowDiagonal => _allowDiagonal;
    public List<TerrainType> TerrainTypes => _terrainTypes;
}
