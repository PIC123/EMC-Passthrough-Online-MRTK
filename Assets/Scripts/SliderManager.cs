using System.Collections.Generic;
using UnityEngine;

public class SliderManager : MonoBehaviour
{
    private GameObject slider;
    public Texture[] displayImage;
    public GameObject Panel;

    private List<GameObject> tickObjects;
    private List<float> tickPositions;
    private float tolerance = 0.01f; // Adjust this value as needed to handle precision issues

    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("slider");

        if (slider == null)
        {
            Debug.LogWarning("Slider GameObject not found in Start!");
        }
        else
        {
            Debug.Log("Slider GameObject found in Start.");
        }

        tickObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("ticks"));
        tickPositions = new List<float>();

        foreach (GameObject tick in tickObjects)
        {
            tickPositions.Add(tick.transform.position.x);
        }

        tickPositions.Sort(); // Sort the tick positions in ascending order

        Debug.Log("Found " + tickObjects.Count + " tick objects.");
    }

    void Update()
    {
        if (slider == null)
        {
            slider = GameObject.FindGameObjectWithTag("slider");
            if (slider == null)
            {
                Debug.Log("Slider GameObject not found in Update!");
                return;
            }
            else
            {
                Debug.Log("Slider GameObject found in Update.");
            }
        }

        CheckSliderPosition();
    }

    public void CheckSliderPosition()
    {
        slider = GameObject.FindGameObjectWithTag("slider");
        if (slider == null)
        {
            Debug.LogWarning("Slider GameObject not assigned!");
            return;
        }

        float sliderXPosition = slider.transform.position.x;

        float closestTick = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < tickPositions.Count; i++)
        {
            float distance = Mathf.Abs(tickPositions[i] - sliderXPosition);
            if (distance < closestTick && distance <= tolerance)
            {
                closestTick = distance;
                closestIndex = i;
            }
        }

        if (closestIndex != -1)
        {
            if (Panel == null)
            {
                Debug.LogWarning("Panel GameObject not assigned!");
                return;
            }

            Renderer renderer = Panel.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogWarning("Renderer component not found on Panel!");
                return;
            }

            if (closestIndex < displayImage.Length)
            {
                renderer.material.mainTexture = displayImage[closestIndex];
            }
            else
            {
                Debug.LogWarning("Index out of range for displayImage array.");
            }
        }
    }
}
