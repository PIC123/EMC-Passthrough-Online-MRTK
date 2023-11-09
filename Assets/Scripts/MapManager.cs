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
    public PinchSlider waterSlider;
    public PinchSlider zoomSlider;
    public float initialWaterHeight;
    public GlobeManager globeManager;
    public ParticleSystem particles;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI latText;
    public TextMeshProUGUI longText;
    public TextMeshProUGUI zoomText;

    public TextMeshProUGUI co2Text;
    public Image co2Dial;
    public TextMeshProUGUI n2oText;
    public Image n2oDial;
    public TextMeshProUGUI ch4Text;
    public Image ch4Dial;
    public TextMeshProUGUI h2oText;
    public Image h2oDial;

    private GlobeManager.Marker currMarker;

    private MapTableSync _mapTableSync;
    private GlobeSync _globeSync;

    private void Awake()
    {
        _mapTableSync = GetComponent<MapTableSync>();
        _globeSync = GameObject.Find("Globe Module").GetComponent<GlobeSync>();
    }

    // Start is called before the first frame update
    void Start()
    {
        zoomSlider.OnValueUpdated.AddListener(delegate { setZoom(); });
        initialWaterHeight = water.transform.position.y;
        currMarker = globeManager.selectedMarker;
        setText();
    }

    // Update is called once per frame
    void Update()
    {
        //if (waterSlider.value > waterSlider.maxValue * 0.1f)
        if (waterSlider.SliderValue > 1f * 0.1f)
        {
            water.SetActive(true);
            water.transform.position = new Vector3(water.transform.position.x, initialWaterHeight + (waterSlider.SliderValue*0.1f), water.transform.position.z);
            _mapTableSync.setWaterLevel(waterSlider.SliderValue);
            //water.transform.position = new Vector3(water.transform.position.x, initialWaterHeight + waterSlider.value, water.transform.position.z);
        }
        else
        {
            _mapTableSync.setWaterLevel(waterSlider.SliderValue);
            water.SetActive(false);
        }

        if (controller.isGrabbed)
        {
            setLatLong(mapRenderer.Center.LatitudeInDegrees + controller.zdist * translationSpeedOffset, mapRenderer.Center.LongitudeInDegrees + controller.xdist * translationSpeedOffset);
            //mapRenderer.Center = new LatLon(
            //    ,
                
            //);
        }

        //mapInfoText.text = $"Latitude: {mapRenderer.Center.LatitudeInDegrees} \n Longitude: {mapRenderer.Center.LongitudeInDegrees} \n Zoom Level: {mapRenderer.ZoomLevel}";
        if(currMarker.title != globeManager.selectedMarker.title)
        {
            currMarker = globeManager.selectedMarker;
            Debug.Log("New location!");
            setText();
            setParticleLevel();
        }
    }

    public void setZoom()
    {
        float newZoom = 12 + (8 * zoomSlider.SliderValue);
        mapRenderer.ZoomLevel = newZoom;
        _mapTableSync.setZoom(newZoom);
        setLatLongText();
    }

    public void setZoom(float zoomLevel)
    {
        mapRenderer.ZoomLevel = zoomLevel;
        _mapTableSync.setZoom(zoomLevel);
        setLatLongText();
    }

    public void setLatLong(double targetLat, double targetLong)
    {
        mapRenderer.Center = new LatLon(targetLat, targetLong);
        _mapTableSync.setLat(targetLat);
        _mapTableSync.setLong(targetLong);
        setLatLongText();
    }

    public void setLatLongText()
    {
        var markerData = currMarker;

        // Update Basic Info Panel
        locationText.text = markerData.title;
        latText.text = mapRenderer.Center.LatitudeInDegrees.ToString("F4");
        longText.text = mapRenderer.Center.LongitudeInDegrees.ToString("F4");
        zoomText.text = mapRenderer.ZoomLevel.ToString("F4");
    }

    public void setText()
    {
        var markerData = currMarker;

        // Update Basic Info Panel
        locationText.text = markerData.title;
        latText.text = markerData.latitude.ToString("F4");
        longText.text = markerData.longitude.ToString("F4");
        zoomText.text = mapRenderer.ZoomLevel.ToString("F4");


        // Update Data Panel
        co2Text.text = markerData.co2.ToString() + "\nmt/year";
        co2Dial.fillAmount = ((markerData.co2 - 10000f) / 25000f)*0.5f; //(Mathf.Clamp(markerData.co2 - 25000f, 10000f, 35000f) / 10000f);
        if(markerData.co2 > 31000f)
        {
            co2Text.color = Color.red;
            co2Dial.color = Color.red;
        } else if(markerData.co2 >28000f)
        {
            co2Text.color = Color.yellow;
            co2Dial.color = Color.yellow;
        } else if(markerData.co2 <=28000f)
        {
            co2Text.color = Color.green;
            co2Dial.color = Color.green;
        }
        n2oText.text = markerData.n2o.ToString() + "\nmt/year";
        n2oDial.fillAmount = ((markerData.n2o - 5f) / 25f) * 0.5f;
        if (markerData.n2o > 17f)
        {
            n2oText.color = Color.red;
            n2oDial.color = Color.red;
        }
        else if (markerData.n2o > 15f)
        {
            n2oText.color = Color.yellow;
            n2oDial.color = Color.yellow;
        }
        else if (markerData.n2o <= 15f)
        {
            n2oText.color = Color.green;
            n2oDial.color = Color.green;
        }
        ch4Text.text = markerData.ch4.ToString() + "\nmt/year";
        ch4Dial.fillAmount = ((markerData.ch4 - 5f) / 25f) * 0.5f;
        if (markerData.ch4 > 17f)
        {
            ch4Text.color = Color.red;
            ch4Dial.color = Color.red;
        }
        else if (markerData.ch4 > 15f)
        {
            ch4Text.color = Color.yellow;
            ch4Dial.color = Color.yellow;
        }
        else if (markerData.ch4 <= 15f)
        {
            ch4Text.color = Color.green;
            ch4Dial.color = Color.green;
        }
        h2oText.text = markerData.h2o.ToString() + "\n3M gal/year";
        h2oDial.fillAmount = ((markerData.h2o - 10000f) / 13000f) * 0.5f;
        if (markerData.h2o > 12000f)
        {
            h2oText.color = Color.red;
            h2oDial.color = Color.red;
        }
        else if (markerData.h2o > 11000f)
        {
            h2oText.color = Color.yellow;
            h2oDial.color = Color.yellow;
        }
        else if (markerData.h2o <= 11000f)
        {
            h2oText.color = Color.green;
            h2oDial.color = Color.green;
        }
        //mapInfoText.text = $"Location: {markerData.title}\n Latitude: {markerData.latitude} Longitude: {markerData.longitude}\n Zoom Level: {mapRenderer.ZoomLevel}";
    }

    public void toggleParticles(bool showParticles)
    {
        particles.gameObject.SetActive(showParticles);
        _mapTableSync.setParticlesVisible(showParticles);
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
