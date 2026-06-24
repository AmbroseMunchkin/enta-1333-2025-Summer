using UnityEngine;
using System.Collections.Generic;

public class NaivePathfinder
{
    private System.Func<Vector2Int, List<Vector2Int>> _getNeighbours;
    private bool _allowDiagonal;
    private int _maxPathLength;

    public NaivePathfinder(
        System.Func<Vector2Int, List<Vector2Int>> getNeighbours, 
        bool allowDiagonal = true, 
        int maxPathLeength = 100)
    {
        this._getNeighbours = getNeighbours;
        this._allowDiagonal = allowDiagonal;
        this._maxPathLength = maxPathLeength;
    }

    public (List<Vector2Int> path, HashSet<Vector2Int> visited) FindPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Vector2Int current = start;
        path.Add(current);
        visited.Add(current);

        while(current != end)
        {
            Vector2Int next;
            if(_allowDiagonal)
            {
                //TODO
            }
            else
            {
                next = GetNextCardinalNaivePosition(current, end);
            }
        }
        
        return (path, visited);
    }
    private Vector2Int GetNextCardinalNaivePosition(Vector2Int current, Vector2Int target)
    {
        //calculate which direction has the larger difference
        int dx = Mathf.Abs(target.x - current.x);
        int dy = Mathf.Abs(target.y - current.y);

        //try to move in the direction with the larger difference first
        if (dx >  dy)
        {
            //try horizontal movement
            int signX = target.x > current.x ? 1 : -1;
            Vector2Int next = current + new Vector2Int(signX, 0);
            if(isValidMove(next))
            {
                return next;
            }

            //if the horizontal movement failed, try vertical
            int signY = target.y > current.y ? 1 : -1;
            next = current + new Vector2Int(0, signY);
            if(isValidMove(next))
            {
                return next;
            }

            return current; //No valid moves found
        }

        return current;
    }

    private bool isValidMove(Vector2Int pos)
    {
        return _getNeighbours(pos).Count > 0;
    }
}
