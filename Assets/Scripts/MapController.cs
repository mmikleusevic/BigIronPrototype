using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private LevelNodeGenerator levelNodeGenerator;
    [SerializeField] private LevelNodeSpriteSet levelNodeSpriteSet;
    [SerializeField] private MapLevelNode mapLevelNodePrefab;
    [SerializeField] private RectTransform mapLevelNodeParent;
    
    private readonly List<MapLevelNode> activeNodes = new List<MapLevelNode>();
    private MapLevelNode currentLevelNode;

    private void OnEnable()
    {
        levelNodeGenerator.OnLevelNodesGenerated += OnNodesGenerated;
    }

    private void OnDisable()
    {
        levelNodeGenerator.OnLevelNodesGenerated -= OnNodesGenerated;
    }

    private void OnNodesGenerated()
    {
        foreach (IReadOnlyList<LevelNode> floor in levelNodeGenerator.LevelNodeGraph)
        {
            foreach (LevelNode nodeData in floor)
            {
                MapLevelNode nodeUI = Instantiate(mapLevelNodePrefab, mapLevelNodeParent);
                nodeUI.Initialize(levelNodeSpriteSet.GetSprite(nodeData.LevelNodeType), nodeData, HandleNodeClicked);
                
                activeNodes.Add(nodeUI);
            }
        }

        currentLevelNode = activeNodes.FirstOrDefault(a => a.LevelNode.LevelNodeType == LevelNodeType.Start);
        currentLevelNode?.SetInteractable(true);
        currentLevelNode?.Highlight(true);
    }

    private void HandleNodeClicked(MapLevelNode clickedNode)
    {
        if (!currentLevelNode.LevelNode.Connections.Contains(clickedNode.LevelNode)) return;
        
        currentLevelNode.Highlight(false);
        currentLevelNode.SetInteractable(false);
        currentLevelNode = clickedNode;
        currentLevelNode.Highlight(true);
        
        foreach (MapLevelNode node in activeNodes)
        {
            bool canReach = currentLevelNode.LevelNode.Connections.Contains(node.LevelNode);
            node.SetInteractable(canReach);
        }

        // TODO: load actual level
        Debug.Log($"Loading level: {clickedNode.LevelNode.LevelNodeType}");
    }
}