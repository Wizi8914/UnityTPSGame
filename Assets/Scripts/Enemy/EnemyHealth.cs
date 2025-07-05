using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void TakeDamage(float damage)
    {
        if (health > 0f)
        {
            health -= damage;
            if (health <= 0f) EnemyDeath();
            Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");    
        }
    } 
    
    void EnemyDeath()
    {
        // Handle enemy death logic here, such as playing an animation, dropping loot, etc.
        Debug.Log($"{gameObject.name} has died.");
        //Destroy(gameObject); // Destroy the enemy game object
    }
}
