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

    private GameObject[] tooltips;
    private bool showtooltips = true;

    void Start()
    {
        tooltips = GameObject.FindGameObjectsWithTag("Tooltip");
    }

    // Update is called once per frame
    void Update()
    {
        if(waterSlider.value > 0.1)
        {
            water.SetActive(true);
            water.transform.position = new Vector3(water.transform.position.x, waterSlider.value - 1, water.transform.position.z);
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
