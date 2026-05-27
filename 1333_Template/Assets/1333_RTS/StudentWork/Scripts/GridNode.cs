using UnityEngine;

//Represents each node on our grid. Lightweight struct
[System.Serializable]
public struct GridNode
{
    public string Name; //an index for us to keep track and organize nodes
    public Vector3 WorldPosition;
    public bool Walkable;
    public int Weight;
}
