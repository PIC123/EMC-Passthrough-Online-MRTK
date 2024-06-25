using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Control : MonoBehaviour
{
    
	public float fogDensity;
	public Color fogColor;
	public float skyExposure01 = 1.0f;
	public Color Tint;
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogColor = fogColor;
		RenderSettings.skybox.SetFloat("_Exposure", skyExposure01);
		RenderSettings.skybox.SetColor("_Tint", Tint);
    }
}
