using System.Collections.Generic;
using UnityEngine;

public class SliderManager : MonoBehaviour
{
    private GameObject slider;
    public Texture[] displayImage;
    public GameObject Panel;

    private List<GameObject> tickObjects;
    private List<float> tickPositions;
    private float tolerance = 0.01f; 

    void Start()
    {
        slider = GameObject.FindGameObjectWithTag("slider");

        tickObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("ticks"));
        tickPositions = new List<float>();

        foreach (GameObject tick in tickObjects)
        {
            tickPositions.Add(tick.transform.position.x);
        }

        tickPositions.Sort(); // Sort the tick positions in ascending order
    }

    void Update()
    {
        if (slider == null)
        {
            slider = GameObject.FindGameObjectWithTag("slider");
        }

        CheckSliderPosition();
    }

    public void CheckSliderPosition()
    {
        slider = GameObject.FindGameObjectWithTag("slider");

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

            Renderer renderer = Panel.GetComponent<Renderer>();

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
