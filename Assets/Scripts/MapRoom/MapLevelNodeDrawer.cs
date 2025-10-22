using Extensions;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace MapRoom
{
    public class MapLevelNodeDrawer : MonoBehaviour
    {
        [SerializeField] private UILineRenderer uiLineRendererPrefab;
        [SerializeField] private RectTransform parent;
    
        public void DrawConnections(MapLevelNodes mapLevelNodes)
        {
            Canvas.ForceUpdateCanvases();

            for (int i = mapLevelNodes.LevelNodeGraph.Count - 1; i >= 0; i--)
            {
                MapLevelNode startNode = mapLevelNodes.LevelNodeGraph[i];
                RectTransform startRect = startNode.GetComponent<RectTransform>();

                foreach (LevelNode connection in startNode.LevelNode.Connections)
                {
                    MapLevelNode endNode = mapLevelNodes.GetNodeByLevelNode(connection);
                    RectTransform endRect = endNode.GetComponent<RectTransform>();

                    UILineRenderer lineRenderer = uiLineRendererPrefab.GetPooledObject<UILineRenderer>(parent);
                    
                    Vector2 startPos = WorldToLocal(startRect.position, parent);
                    Vector2 endPos = WorldToLocal(endRect.position, parent);
                    
                    lineRenderer.Points = new Vector2[] { startPos, endPos };
                    lineRenderer.color = Color.white;
                }
            }
        }
        
        private static Vector2 WorldToLocal(Vector3 worldPos, RectTransform parent)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                RectTransformUtility.WorldToScreenPoint(null, worldPos),
                null,
                out Vector2 localPoint
            );
            return localPoint;
        }
    }
}