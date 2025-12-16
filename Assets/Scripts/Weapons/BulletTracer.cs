using UnityEngine;

namespace Weapons
{
    public class BulletTracer : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 0.05f;

        public void Initialize(Vector3 start, Vector3 end)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            Destroy(gameObject, lifeTime);
        }
    }
}