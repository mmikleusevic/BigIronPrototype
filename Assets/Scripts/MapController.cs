using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MapController : MonoBehaviour
{
    [SerializeField] private LevelNodeGenerator levelNodeGenerator;
    [SerializeField] private LevelNodeSpriteSet levelNodeSpriteSet;
    [SerializeField] private MapLevelNodeDrawer mapLevelNodeDrawer;
    
    [Header("Map nodes")]
    [SerializeField] private MapLevelNode mapLevelNodePrefab;
    [SerializeField] private RectTransform mapLevelNodeParent;
    [SerializeField] private GameObject mapLevelNodeGroupPrefab;
    
    private readonly MapLevelNodes activeNodes = new MapLevelNodes();
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
        for (int i = levelNodeGenerator.LevelNodeGraph.Count- 1; i >= 0; i--)
        {
            GameObject mapLevelNodeGroup = Instantiate(mapLevelNodeGroupPrefab, mapLevelNodeParent);
            
            IReadOnlyList<LevelNode> floor = levelNodeGenerator.LevelNodeGraph[i];
            
            foreach (LevelNode nodeData in floor)
            {
                MapLevelNode nodeUI = Instantiate(mapLevelNodePrefab, mapLevelNodeGroup.transform);
                
                activeNodes.AddNode(nodeUI);
                
                nodeUI.Initialize(levelNodeSpriteSet.GetSprite(nodeData.LevelNodeType), nodeData, HandleNodeClicked);
            }
        }
        
        DrawConnections();

        currentLevelNode = activeNodes.GetNodeByLevelType(LevelNodeType.Start);
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
        
        foreach (MapLevelNode node in activeNodes.LevelNodeGraph)
        {
            bool canReach = currentLevelNode.LevelNode.Connections.Contains(node.LevelNode);
            node.SetInteractable(canReach);
        }

        // TODO: load actual level
        Debug.Log($"Loading level: {clickedNode.LevelNode.LevelNodeType}");
    }
    
    private void DrawConnections()
    {
        mapLevelNodeDrawer.DrawConnections(activeNodes);
    }
}