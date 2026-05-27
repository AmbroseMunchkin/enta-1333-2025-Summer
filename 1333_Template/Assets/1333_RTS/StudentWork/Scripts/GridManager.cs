using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
        for(int x=0; x < _gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y< _gridSettings.GridSizeY; y++)
            {

            }
        }
    }
}
