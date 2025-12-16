using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapRoom
{
    public class LevelNodeGenerator : MonoBehaviour
    {
        public event Action OnLevelNodesGenerated;
    
        [SerializeField] private int floors;
        [SerializeField] private int nodesPerFloor;
        [SerializeField] private int maxConnections;
        [SerializeField] private float xSpacing;
        [SerializeField] private float ySpacing;

        public IReadOnlyList<IReadOnlyList<LevelNode>> LevelNodeGraph => levelNodeGraph;

        private readonly List<List<LevelNode>> levelNodeGraph = new List<List<LevelNode>>();
        
        private LevelNodeType[] validTypes;

        private void Start()
        {
            GenerateLevelNodesAndConnections();
        }

        private void GenerateLevelNodesAndConnections()
        {
            GenerateValidLevelNodeTypes();
            GenerateLevelNodes();
            GenerateConnectionsBetweenLevelNodes();
        
            OnLevelNodesGenerated?.Invoke();
        }

        private void GenerateValidLevelNodeTypes()
        {
            LevelNodeType[] levelNodeTypes = (LevelNodeType[])Enum.GetValues(typeof(LevelNodeType));

            validTypes = levelNodeTypes.Where(t => t != LevelNodeType.Start && t != LevelNodeType.Boss)
                .ToArray();
        }

        private void GenerateLevelNodes()
        {
            for (int floor = 0; floor < floors; floor++)
            {
                List<LevelNode> floorNodes = new List<LevelNode>();
                int nodeCount = (floor == 0 || floor == floors - 1) ? 1 : nodesPerFloor;

                for (int i = 0; i < nodeCount; i++)
                {
                    LevelNodeType levelNodeType = ChooseLevelNodeType(floor);

                    LevelNode levelNode = new LevelNode(floor, levelNodeType);
                
                    floorNodes.Add(levelNode);
                }
            
                levelNodeGraph.Add(floorNodes);
            }
        }

        private LevelNodeType ChooseLevelNodeType(int floor)
        {
            if (floor == 0) return LevelNodeType.Start;
            if (floor == floors - 1) return LevelNodeType.Boss;

            return validTypes[Random.Range(0, validTypes.Length)];
        }

        private void GenerateConnectionsBetweenLevelNodes()
        {
            for (int floor = 0; floor < floors - 1; floor++)
            {
                List<LevelNode> currentFloorNodes = levelNodeGraph[floor];
                List<LevelNode> nextFloorNodes = levelNodeGraph[floor + 1];
            
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
                        connections = Random.Range(2, Mathf.Min(maxConnections, possibleTargets.Count) + 1);
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
}