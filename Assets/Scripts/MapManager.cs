using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class MapManager : MonoBehaviour
{
    public MapRenderer mapRenderer;
    public JoystickBallController controller;
    public float translationSpeedOffset = 1;
    public float rotationSpeedOffset = 1;
    public float zoomSpeedOffset = 1;
    //public GameObject generalInfoPanel;
    //public GameObject dataPanel;
    public GameObject water;
    public float waterLevel;
    //public Slider waterSlider;
    public PinchSlider pinchSlider;
    public float initialWaterHeight;
    public GlobeManager globeManager;
    public ParticleSystem particles;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI latText;
    public TextMeshProUGUI longText;
    public TextMeshProUGUI zoomText;

    public TextMeshProUGUI co2Text;
    public TextMeshProUGUI n2oText;
    public TextMeshProUGUI ch4Text;
    public TextMeshProUGUI h2oText;

    private GlobeManager.Marker currMarker;

    private MapTableSync _mapTableSync;

    private void Awake()
    {
        _mapTableSync = GetComponent<MapTableSync>();
    }

    // Start is called before the first frame update
    void Start()
    {
        initialWaterHeight = water.transform.position.y;
        currMarker = globeManager.selectedMarker;
        setText();
    }

    // Update is called once per frame
    void Update()
    {
        //if (waterSlider.value > waterSlider.maxValue * 0.1f)
        if (pinchSlider.SliderValue > 1f * 0.1f)
        {
            water.SetActive(true);
            water.transform.position = new Vector3(water.transform.position.x, initialWaterHeight + (pinchSlider.SliderValue*0.1f), water.transform.position.z);
            //water.transform.position = new Vector3(water.transform.position.x, initialWaterHeight + waterSlider.value, water.transform.position.z);
        }
        else
        {
            water.SetActive(false);
        }

        if (controller.isGrabbed)
        {
            setLatLong(mapRenderer.Center.LatitudeInDegrees + controller.zdist * translationSpeedOffset, mapRenderer.Center.LongitudeInDegrees + controller.xdist * translationSpeedOffset);
            setLatLongText();
            //mapRenderer.Center = new LatLon(
            //    ,
                
            //);
        }

        //mapInfoText.text = $"Latitude: {mapRenderer.Center.LatitudeInDegrees} \n Longitude: {mapRenderer.Center.LongitudeInDegrees} \n Zoom Level: {mapRenderer.ZoomLevel}";
        if(currMarker.title != globeManager.selectedMarker.title)
        {
            //Debug.Log("New location!");
            setText();
            setParticleLevel();
        }
    }

    public void setZoom(float zoomLevel)
    {
        mapRenderer.ZoomLevel = zoomLevel;
        _mapTableSync.setZoom(zoomLevel);
    }

    public void setLatLong(double targetLat, double targetLong)
    {
        mapRenderer.Center = new LatLon(targetLat, targetLong);
        _mapTableSync.setLat(targetLat);
        _mapTableSync.setLong(targetLong);

    }

    public void setLatLongText()
    {
        var markerData = globeManager.selectedMarker;

        // Update Basic Info Panel
        locationText.text = markerData.title;
        latText.text = markerData.latitude.ToString();
        longText.text = markerData.longitude.ToString();
        zoomText.text = mapRenderer.ZoomLevel.ToString();
    }

    public void setText()
    {
        var markerData = globeManager.selectedMarker;

        // Update Basic Info Panel
        locationText.text = markerData.title;
        latText.text = markerData.latitude.ToString();
        longText.text = markerData.longitude.ToString();
        zoomText.text = mapRenderer.ZoomLevel.ToString();


        // Update Data Panel
        co2Text.text = markerData.co2.ToString();
        if(markerData.co2 > 31000f)
        {
            co2Text.color = Color.red;
        } else if(markerData.co2 >28000f)
        {
            co2Text.color = Color.yellow;
        } else if(markerData.co2 <=28000f)
        {
            co2Text.color = Color.green;
        }
        n2oText.text = markerData.n2o.ToString();
        if (markerData.n2o > 17f)
        {
            n2oText.color = Color.red;
        }
        else if (markerData.n2o > 15f)
        {
            n2oText.color = Color.yellow;
        }
        else if (markerData.n2o <= 15f)
        {
            n2oText.color = Color.green;
        }
        ch4Text.text = markerData.ch4.ToString();
        if (markerData.ch4 > 17f)
        {
            ch4Text.color = Color.red;
        }
        else if (markerData.ch4 > 15f)
        {
            ch4Text.color = Color.yellow;
        }
        else if (markerData.ch4 <= 15f)
        {
            ch4Text.color = Color.green;
        }
        h2oText.text = markerData.h2o.ToString();
        if (markerData.ch4 > 17f)
        {
            h2oText.color = Color.red;
        }
        else if (markerData.ch4 > 15f)
        {
            h2oText.color = Color.yellow;
        }
        else if (markerData.ch4 <= 15f)
        {
            h2oText.color = Color.green;
        }
        //mapInfoText.text = $"Location: {markerData.title}\n Latitude: {markerData.latitude} Longitude: {markerData.longitude}\n Zoom Level: {mapRenderer.ZoomLevel}";
    }

    public void setParticleLevel()
    {
        var particleEmission = particles.emission;
        var newROT = ((globeManager.selectedMarker.co2 - 23000f) / (32000f - 24000f)) * 500;
        particleEmission.rateOverTime = newROT;
        //Debug.Log($"NewROT: {newROT}");
        //particles..emission.rateOverTime = (globeManager.selectedMarker.co2 - 28000f)/
    }
}
