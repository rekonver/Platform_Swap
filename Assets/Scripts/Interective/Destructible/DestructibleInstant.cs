using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestructibleInstant : MonoBehaviour, IDestructible
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
