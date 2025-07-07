using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Rigidbody[] ragdollRigidbodies;

    void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdollRigidbodies) rb.isKinematic = true;
        
    }

    public void EnableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRigidbodies) rb.isKinematic = false;
    }
}
