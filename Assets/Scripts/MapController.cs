using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private LevelNodeGenerator levelNodeGenerator;
    [SerializeField] private LevelNodeSpriteSet levelNodeSpriteSet;

    private LevelNode currentLevelNode;

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
        currentLevelNode = levelNodeGenerator.StartingLevelNode;
        
        
    }

    private void OnNodeClicked(LevelNode clickedNode)
    {
        if (!currentLevelNode.Connections.Contains(clickedNode)) return;
        
        currentLevelNode = clickedNode;
        
        //TODO load level
    }

    private void HighlightReachableNodes()
    {
        
    }
}