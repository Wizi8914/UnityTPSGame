using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform topRetire;
    [SerializeField] private RectTransform bottomRetire;
    [SerializeField] private RectTransform rightRetire;
    [SerializeField] private RectTransform leftRetire;

    [Header("Bump Settings")]

    public float originalBumpAmount = 20f;
    public float currentBumpAmount = 20f; // Default bump amount, can be adjusted in the Inspector
    public float bumpSpeed = 10f;
    public float returnSpeed = 5f;

    [Header("Color Settings")]
    public Color normalColor = Color.white;
    public Color enemyColor = Color.red;

    float currentRecoilPosition, finalRecoilPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0f, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, bumpSpeed * Time.deltaTime);

        topRetire.localPosition = new Vector3(0, finalRecoilPosition + currentBumpAmount, 0);
        bottomRetire.localPosition = new Vector3(0, -currentBumpAmount - finalRecoilPosition, 0);
        rightRetire.localPosition = new Vector3(finalRecoilPosition + currentBumpAmount, 0, 0);
        leftRetire.localPosition = new Vector3(-finalRecoilPosition - currentBumpAmount, 0, 0);
    }

    public void BumpCrosshair(float bumpAmount)
    {
        //topRetire.localPosition = Vector3.Lerp()
        currentRecoilPosition += bumpAmount;
    }

    public void SetCrosshairColor(Color color)
    {
        topRetire.GetComponent<Image>().color = color;
        bottomRetire.GetComponent<Image>().color = color;
        rightRetire.GetComponent<Image>().color = color;
        leftRetire.GetComponent<Image>().color = color;
    }

    public void SetCrosshairBumpAmount(float amount)
    {
        currentRecoilPosition = amount;
        currentBumpAmount = amount;
    }
}


//currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0f, returnSpeed * Time.deltaTime);
//finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);

//recoilFollowPos.localPosition = new Vector3(0, 0, finalRecoilPosition);
    

//public void TriggerRecoil() => currentRecoilPosition += kickBackForce;