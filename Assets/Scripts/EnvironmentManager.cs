using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject water;
    public float waterLevel;
    public Slider waterSlider;
    public PinchSlider pinchSlider;

    private GameObject[] tooltips;
    private bool showtooltips = true;
    private float initialWaterHeight;

    void Start()
    {
        initialWaterHeight = water.transform.position.y;
        tooltips = GameObject.FindGameObjectsWithTag("Tooltip");
    }

    // Update is called once per frame
    void Update()
    {
        //if(waterSlider.value > 0.1)
        if(pinchSlider.SliderValue > 1f * 0.1f)
        {
            water.SetActive(true);
            water.transform.position = new Vector3(water.transform.position.x, initialWaterHeight + (pinchSlider.SliderValue*0.9f), water.transform.position.z);
        }
        else
        {
            water.SetActive(false);
        }
    }

    public void toggleTooltips()
    {
        showtooltips = !showtooltips;
        foreach (GameObject tooltip in tooltips)
        {
            tooltip.SetActive(showtooltips);

        }
    }
}
