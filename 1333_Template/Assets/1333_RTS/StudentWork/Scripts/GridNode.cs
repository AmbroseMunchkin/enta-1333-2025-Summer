using System.Collections.Generic;
using UnityEngine;

//Represents each node on our grid. Lightweight struct
[System.Serializable]
public struct GridNode
{
    public TerrainType TerrainType;
    public string Name; //an index for us to keep track and organize nodes
    // public string Name => TerrainType?.TerrainName ?? "No name";
    public Vector3 WorldPosition;
    public bool Walkable => TerrainType != null ? TerrainType.Walkable : false;
    public int Weight => TerrainType != null && TerrainType.Walkable ? TerrainType.MovementCost : 1;
    public Color GizmoColor => TerrainType != null ? TerrainType.Color : Color.gray;
}

