using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Pathfinder : MonoBehaviour
{
    public enum PathfindingType
    {
        Naive
    }
    public enum VisualizationState
    {
        Idle,
        Exploring,
        Reconstructing,
        Paused
    }

    [Header("Required Reference")]
    [SerializeField] private GridManager _gridManager;

    [Header("Pathfinding Settings")]
    [SerializeField] private PathfindingType _pathfindingType = PathfindingType.Naive;
    [SerializeField, Range(0, 100)] private int _framesPerStep = 10;
    [SerializeField] private bool _visualizePathfinding = true;

    [Header("Visualization Colors")]
    [SerializeField] private Color _startNodeColor = Color.green;
    [SerializeField] private Color _endNodeColor = Color.red;
    [SerializeField] private Color _currentPathColor = Color.yellow;
    [SerializeField] private Color _visitedNodeColor = new Color (0.5f, 0.5f, 1f, 0.5f);
    [SerializeField] private Color _unvisitedNodeColor = new Color (0.3f, 0.3f, 0.3f, 0.3f);
    [SerializeField] private Color _finalPathColor = Color.cyan;
    [SerializeField] private Color _currentNodeColor = Color.magenta;
    [SerializeField] private Color _currentNeightborColor = new Color(1f, 0.5f, 0f, 0.5f); //Orange
    [SerializeField] private Color _explorationLineColor = new Color(1f, 1f, 0f, 0.3f); //semi-transparent yellow

    [Header("Visualization Settings")]
    [SerializeField] private int _currentSeed = 0;
    [SerializeField] private bool _useSeededRandom = true;
    [SerializeField] private float _minWeight;
    [SerializeField] private float _maxWeight;

    //Pathfinder instances
    private NaivePathfinder _naivePathfinder;

    //Visualized instances
    private NaivePathfinderVisualizer _naivePathfinderVisualizer;

    private System.Random _seedRandom;
}
