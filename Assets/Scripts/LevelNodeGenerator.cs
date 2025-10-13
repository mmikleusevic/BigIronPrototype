using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelNodeGenerator : MonoBehaviour
{
    public static event Action OnLevelNodesGenerated;
    
    [SerializeField] private int floors;
    [SerializeField] private int nodesPerFloor;
    [SerializeField] private int maxConnections;
    [SerializeField] private float xSpacing;
    [SerializeField] private float ySpacing;

    private readonly List<List<LevelNode>> graph = new List<List<LevelNode>>();

    private void Start()
    {
        GenerateLevelNodesAndConnections();
    }

    private void GenerateLevelNodesAndConnections()
    {
        GenerateLevelNodes();
        GenerateConnectionsBetweenLevelNodes();
        
        OnLevelNodesGenerated?.Invoke();
    }

    private void GenerateLevelNodes()
    {
        for (int floor = 0; floor < floors; floor++)
        {
            List<LevelNode> floorNodes = new List<LevelNode>();
            int nodeCount = (floor == 0 || floor == floors - 1) ? 1 : nodesPerFloor;

            for (int i = 0; i < nodeCount; i++)
            {
                Vector2 position = new Vector2(i * xSpacing - ((nodeCount - 1) * xSpacing / 2f), floor * ySpacing);
                LevelNodeType levelNodeType = ChooseLevelNodeType(floor);
                
                LevelNode levelNode = new LevelNode(floor, position, levelNodeType);

                floorNodes.Add(levelNode);
            }
            
            graph.Add(floorNodes);
        }
    }

    private LevelNodeType ChooseLevelNodeType(int floor)
    {
        if (floor == 0) return LevelNodeType.Start;
        if (floor == floors - 1) return LevelNodeType.Boss;
        return (LevelNodeType)Random.Range(1, Enum.GetValues(typeof(LevelNodeType)).Length - 1);
    }

    private void GenerateConnectionsBetweenLevelNodes()
    {
        for (int floor = 0; floor < floors - 1; floor++)
        {
            List<LevelNode> currentFloorNodes = graph[floor];
            List<LevelNode> nextFloorNodes = graph[floor + 1];
            
            for (int i = 0; i < currentFloorNodes.Count; i++)
            {
                LevelNode floorNode = currentFloorNodes[i];
                List<LevelNode> possibleTargets = new List<LevelNode>();
                
                if (floor == floors - 2)
                {
                    possibleTargets.Add(nextFloorNodes[0]);
                }
                else
                { 
                    int nextLeftIndex = Mathf.Max(0, i - 1);
                    int nextMiddleIndex = Mathf.Clamp(i, 0, nextFloorNodes.Count - 1);
                    int nextRightIndex = Mathf.Min(nextFloorNodes.Count - 1, i + 1);
                    
                    possibleTargets.Add(nextFloorNodes[nextLeftIndex]);
                    if (nextMiddleIndex != nextLeftIndex) possibleTargets.Add(nextFloorNodes[nextMiddleIndex]);
                    if (nextRightIndex != nextMiddleIndex && nextRightIndex != nextLeftIndex) possibleTargets.Add(nextFloorNodes[nextRightIndex]);
                }
                
                int connections;
                
                if (floorNode.LevelNodeType == LevelNodeType.Start)
                {
                    possibleTargets = new List<LevelNode>(nextFloorNodes);
                    connections = nextFloorNodes.Count;
                }
                else
                {
                    connections = Random.Range(1, Mathf.Min(maxConnections, possibleTargets.Count) + 1);
                }
                
                IEnumerable<LevelNode> chosenTargets = possibleTargets.Take(connections);

                foreach (LevelNode target in chosenTargets)
                {
                    floorNode.TryAddConnection(target);
                }
            }
        }
    }
}