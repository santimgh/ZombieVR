using UnityEngine;

public class BulletForce : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet triggered with: " + other.gameObject.name); 

        if (other.CompareTag("Enemy")) // Make sure the zombie has the "Enemy" tag
        {
            Debug.Log("Bullet hit the zombie!"); 

            ZombieController zombie = other.GetComponent<ZombieController>();
            if (zombie != null)
            {
                zombie.TakeDamage();
            }
        }

        Destroy(gameObject); // Destroy bullet after impact
    }
}
