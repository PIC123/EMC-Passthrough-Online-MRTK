using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp_Panel : MonoBehaviour
{
    public GameObject Panel;
    private Vector3 localPositionToMoveTo = new Vector3(5.8f, 5.8f, -3f); // Example target position
    private Vector3 targetLocalScale = new Vector3(0.95f, 1f, 0.66f); // Example target scale
    private float duration = 2f; // Duration to reach the target position and scale

    private Vector3 originalLocalPosition;
    private Vector3 originalLocalScale;
    private bool isAtTarget = false;

    void Start()
    {
        // Store the original position and scale
        originalLocalPosition = Panel.transform.localPosition;
        originalLocalScale = Panel.transform.localScale;
    }

    // Method to start the Lerp process
    public void Lerpin()
    {
        if (isAtTarget)
        {
            // Move back to the original position and scale
            StartCoroutine(LerpLocalPosition(originalLocalPosition, duration));
            StartCoroutine(LerpLocalScale(originalLocalScale, duration));
        }
        else
        {
            // Move to the target position and scale
            StartCoroutine(LerpLocalPosition(localPositionToMoveTo, duration));
            StartCoroutine(LerpLocalScale(targetLocalScale, duration));
        }

        // Toggle the state
        isAtTarget = !isAtTarget;
    }

    IEnumerator LerpLocalPosition(Vector3 targetLocalPosition, float duration)
    {
        float time = 0;
        Vector3 startLocalPosition = Panel.transform.localPosition;

        while (time < duration)
        {
            Panel.transform.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the panel reaches the target local position exactly
        Panel.transform.localPosition = targetLocalPosition;
    }

    IEnumerator LerpLocalScale(Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = Panel.transform.localScale;

        while (time < duration)
        {
            Panel.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the panel reaches the target scale exactly
        Panel.transform.localScale = targetScale;
    }

    void Update()
    {
        // Optional: Call Lerpin() based on a condition or event
    }
}
