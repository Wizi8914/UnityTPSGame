using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }
}
