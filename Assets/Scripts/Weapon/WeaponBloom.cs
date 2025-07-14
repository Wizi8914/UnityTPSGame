using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] float defaultBloomAngle = 2f;
    [SerializeField] float walkBloomMultiplier = 1.5f;
    [SerializeField] float crouchBloomMultiplier = 0.2f;
    [SerializeField] float sprintBloomMultiplier = 3f;
    [SerializeField] float adsBloomMultiplier = 0.1f;

    MovementStateManager movement;
    AimStateManager aiming;

    float currentBloomAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aiming = GetComponentInParent<AimStateManager>();
    }

    public Vector3 bloomAngle(Transform barrelPosition)
    {
        if (movement.currentState == movement.Idle) currentBloomAngle = defaultBloomAngle;
        else if (movement.currentState == movement.Walk) currentBloomAngle = defaultBloomAngle * walkBloomMultiplier;
        else if (movement.currentState == movement.Run) currentBloomAngle = defaultBloomAngle * sprintBloomMultiplier;
        else if (movement.currentState == movement.Crouch)
        {
            if (movement.moveDirection.magnitude == 0) currentBloomAngle = defaultBloomAngle * crouchBloomMultiplier;
            else currentBloomAngle = defaultBloomAngle * crouchBloomMultiplier * walkBloomMultiplier;
        }

        if (aiming.currentState == aiming.Aim) currentBloomAngle *= adsBloomMultiplier;

        float randomX = Random.Range(-currentBloomAngle, currentBloomAngle);
        float randomY = Random.Range(-currentBloomAngle, currentBloomAngle);
        float randomZ = Random.Range(-currentBloomAngle, currentBloomAngle);

        Vector3 bloomOffset = new Vector3(randomX, randomY, randomZ);
        
        return barrelPosition.localEulerAngles + bloomOffset;
    }
}
