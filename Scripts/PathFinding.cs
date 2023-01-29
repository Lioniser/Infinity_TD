using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Waypoint startPoint, endPoint;
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    bool isRunning = true;
    Waypoint searchPoint;
    public List<Waypoint> path = new List<Waypoint>();
    void Start()
    {
        setPath();
    }
    public List<Waypoint> setPath()
    {
        if (path.Count == 0)
        {
        LoadBlocks();
        FindPathAlgorithm();
        generatePath();
        }
        return path;
    }
    private void FindPathAlgorithm()
    {
        queue.Enqueue(startPoint);
        startPoint.startCheck = true;
        while(queue.Count > 0 && isRunning == true)
        {
            searchPoint = queue.Dequeue();
            searchPoint.check = true;
            CheckForEndPoint();
            ExploreNearPoint();
        }
    }
    private void CheckForEndPoint()
    {
        if (searchPoint == endPoint)
        {
        endPoint.endCheck = true;
        isRunning = false;
        }
    }
    private void ExploreNearPoint()
    {
        if (!isRunning)
        return;
        foreach (Vector2Int direction in directions)
        {
            Vector2Int nearCoord = searchPoint.GetGridPos() + direction;
            if (grid.ContainsKey(nearCoord))
            {
                Waypoint nearPoint = grid[nearCoord];
                addToQueue(nearPoint);
            }
        }
    }
    private void addToQueue(Waypoint addPoint)
    {
        if (addPoint.check || queue.Contains(addPoint))
        return;
        else
        {
        queue.Enqueue(addPoint);
        addPoint.checkFrom = searchPoint;
        }
    }
    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            Vector2Int gridPos = waypoint.GetGridPos();
            if (grid.ContainsKey(gridPos))
            Debug.LogWarning("Повтор"+waypoint);
            else
            grid.Add(gridPos,waypoint);
        }
    }
    private void generatePath()
    {
        AddPointToPath(endPoint);
        Waypoint exploredFrom = endPoint.checkFrom;
        while (exploredFrom != startPoint)
        {
            AddPointToPath(exploredFrom);
            exploredFrom = exploredFrom.checkFrom;
        }
        AddPointToPath(startPoint);
        path.Reverse();
    }
    private void AddPointToPath(Waypoint pathPoint)
    {
        pathPoint.isPlaceble = false;
        pathPoint.SetPathMat();
        path.Add(pathPoint);
    }
}


