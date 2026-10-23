using UnityEngine;

public class ImpulsePlatform : MonoBehaviour
{
    [SerializeField] private float platformImpulse = 30;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody2D rb)) rb.AddForce(transform.up * platformImpulse, ForceMode2D.Impulse);
    }
}