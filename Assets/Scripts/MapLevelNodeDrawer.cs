using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MapLevelNodeDrawer : MonoBehaviour
{
    [SerializeField] private UILineRenderer uiLineRendererPrefab;
    [SerializeField] private Transform parent;
    
    public void DrawConnections(MapLevelNodes mapLevelNodes)
    {
        Canvas.ForceUpdateCanvases();
        foreach (var startNode in mapLevelNodes.LevelNodeGraph)
        {
            RectTransform startRect = startNode.GetComponent<RectTransform>();

            foreach (LevelNode connection in startNode.LevelNode.Connections)
            {
                MapLevelNode endNode = mapLevelNodes.GetNodeByLevelNode(connection);
                RectTransform endRect = endNode.GetComponent<RectTransform>();
                
                UILineRenderer lineRenderer = Instantiate(uiLineRendererPrefab, parent);
                RectTransform lineRectTransform = lineRenderer.GetComponent<RectTransform>();
                
                Vector2[] points = new Vector2[2];
                points[0] = parent.position - startRect.position;
                points[1] = parent.position - endRect.position;

                lineRenderer.Points = points;
                lineRenderer.color = Color.white;
                lineRectTransform.SetAsFirstSibling();
            }
        }
    }
}