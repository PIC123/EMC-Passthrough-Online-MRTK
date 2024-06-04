using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static GlobeManager;

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

    // Variables for 360 env toggle
    public GameObject envSphere;
    private GameObject[] envObjects;
    private bool showSphere = false;

    public GameObject particles;
    public bool showParticles = false;

    void Start()
    {
        initialWaterHeight = water.transform.position.y;
        tooltips = GameObject.FindGameObjectsWithTag("Tooltip");
        envObjects = GameObject.FindGameObjectsWithTag("EnvObj");
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

    public void toggle360Env()
    {
        showSphere = !showSphere;
        envSphere.SetActive(showSphere);
        //if (showSphere)
        //{
        //    StartCoroutine(SetTexture());
        //}
        foreach(GameObject envObj in envObjects)
        {
            envObj.SetActive(!showSphere);
        }
    }

    public void toggleParticles()
    {
        showParticles = !showParticles;
        particles.SetActive(showParticles);
    }

    //IEnumerator SetTexture()
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(selectedMarker.imgURL);
    //    Debug.Log($"Downloading texture from {selectedMarker.imgURL}");
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        envSphere.GetComponent<Renderer>().material.SetTexture("_MainTex", myTexture);
    //        Debug.Log("Setting texture");
    //    }
    //}
}
