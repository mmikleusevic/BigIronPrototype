using CombatRoom;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private Rigidbody rb;
        
        private void Start()
        {
            rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, lifetime);
        }
    }
}