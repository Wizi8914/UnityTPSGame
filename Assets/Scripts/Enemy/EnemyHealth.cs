using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    RagdollManager ragdollManager;
    [HideInInspector] public bool isDead;
    [SerializeField] public SphereCollider headCollider;


    void Start()
    {
        ragdollManager = GetComponent<RagdollManager>();
    }

    public void TakeDamage(float damage)
    {
        if (health > 0f)
        {
            health -= damage;
            if (health <= 0f) EnemyDeath();
            else Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");
        }
    } 
    
    void EnemyDeath()
    {
        ragdollManager.EnableRagdoll();
        Debug.Log($"{gameObject.name} has died.");
        //Destroy(gameObject); // Destroy the enemy game object
    }
}
