using Unity.Cinemachine;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int clipSize;
    public int extraAmmo;
    [HideInInspector] public int currentAmmo;

    public AudioClip magazineInSound;
    public AudioClip magazineOutSound;
    public AudioClip releaseSlideSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = clipSize;
    }

    public void Reload()
    {
        if (extraAmmo >= clipSize)
        {
            int ammmoToReload = clipSize - currentAmmo;
            extraAmmo -= ammmoToReload;
            currentAmmo += ammmoToReload;
        }
        else if (extraAmmo > 0)
        {
            if (extraAmmo + currentAmmo > clipSize)
            {
                int leftOverAmmo = (extraAmmo + currentAmmo) - clipSize;
                extraAmmo = leftOverAmmo;
                currentAmmo = clipSize;
            }
            else
            {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
    }
}
