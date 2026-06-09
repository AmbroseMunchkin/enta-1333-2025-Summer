using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    // variable to allow us to plug in our GridSettings scriptable Object
    [SerializeField] private GridSettings _gridSettings;

    public GridSettings GridSettings => _gridSettings;

    // 2-Dimensional array of GridNode structs
    private GridNode[,] gridNodes;

#if UNITY_EDITOR
    [Header("Debug for editor playmode only")]
    [SerializeField] private List<GridNode> AllNodes = new();
#endif

    //flag for other scripts or this one to use to makesure grid is initialized before doing something else
    public bool IsInitialized { get; private set; } = false;

    public void InitializeGrid()
    {
        //initializing our array of GridNode structs using the dimensions from the Scriptable objects
        gridNodes = new GridNode[_gridSettings.GridSizeX, _gridSettings.GridSizeY];

        //Nest for Loop to iterate over all gridnodes
        //at each grid position, instantiate a new GridNode struckt, give it some default values and add it to our gridNodes 2D array
        for(int x=0; x < _gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y< _gridSettings.GridSizeY; y++)
            {
                Vector3 worldPos = _gridSettings.UseXZPlane
                    ? new Vector3(x, 0, y) * _gridSettings.NodeSize
                    : new Vector3(x, y, 0) * _gridSettings.NodeSize;

                GridNode node = new GridNode
                {
                    Name = $"Cell_{(x + _gridSettings.GridSizeX * x + y)}",
                    WorldPosition = worldPos,
                    Walkable = true,
                    Weight = 1
                };

                gridNodes[x, y] = node;
            }
        }
        IsInitialized = true;
    }
#if UNITY_EDITOR
    private void PopulatedDebugList()
    {
        AllNodes.Clear();
        for(int x = 0; x < _gridSettings.GridSizeX; x++)
        {
            for(int y = 0; y < _gridSettings.GridSizeY; y++)
            {
                GridNode node = gridNodes[x, y];
                AllNodes.Add(new GridNode
                {
                    Name = $"Cell_{x}+{y}",
                    WorldPosition = node.WorldPosition,
                    Walkable = node.Walkable,
                    Weight = node.Weight
                });
            }
        }
    }
#endif

    //Function to retrieve GridNode data efficiently
    public GridNode GetNode(int x, int y)
    {
        if(x < 0 || x >= _gridSettings.GridSizeX || y < 0 || y>= _gridSettings.GridSizeY)
            throw new System.IndexOutOfRangeException("Grid node indices out of range");

        return gridNodes[x, y];
    }

    public void SetWalkable(int x, int y, bool walkable)
    {
        //gridNodes[x,y].Walkable = walkable;
    }
    private void OnDrawGizmos()
    {
        if (gridNodes == null || GridSettings == null) return;

        Gizmos.color = Color.green;
        //Draw the gridnode gizmos, size is 90% of GridNode Size for visual clarity
        for (int x = 0; x < _gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y < _gridSettings.GridSizeY; y++)
            {
                GridNode node = gridNodes[x, y];
                Gizmos.color = node.Walkable ? Color.green : Color.red;
                Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * GridSettings.NodeSize * 0.9f);
            }
        }
    }

    //Create a custom editor button that, when pressed, calls PopulatedDebugList and refreshes the Editor GUI
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //first draw the normal inspector GUI
            DrawDefaultInspector();

            //then look at the GridManager class this is attached to and call the PopulatedDebugList function
            GridManager grid = (GridManager)target;
            if(grid.IsInitialized)
            {
                if(GUILayout.Button("RefreshGrid Debug View"))
                {
                    grid.PopulatedDebugList();
                }
            }
        }
        
    }
}
