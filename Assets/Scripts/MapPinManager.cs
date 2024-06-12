using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine.UI;

public class MapPinManager : MonoBehaviour
{
    public GlobeManager.Marker markerData;
    public MapManager mapManager;
    public Text overviewText;
    private SpinFree spinner;
    public Material goodMat;
    public Material badMat;
    public Material selectMat;
    private GlobeManager globeManager;
    private GlobeSync _globeSync;



    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.FindGameObjectsWithTag("MapTable")[0].GetComponent<MapManager>();
        spinner = gameObject.GetComponentInParent<SpinFree>();
        globeManager = GameObject.Find("Globe").GetComponentInParent<GlobeManager>();
        _globeSync = GameObject.Find("Globe Module").GetComponent<GlobeSync>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupPin(GlobeManager.Marker marker)
    {
        transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);
        if (marker.co2 > 31000f)
        {
            GetComponent<Renderer>().material = badMat;
            transform.localScale = new Vector3(0.065f, 0.065f, 0.065f);
        }
        markerData = marker;
    }

    public void setLatLong()
    {
        mapManager.setLatLong(markerData.latitude, markerData.longitude);
        Debug.Log($"going to {markerData.title}");
        globeManager.UpdateSelectedMarker(markerData);
        _globeSync.SetCurrMarkerTitle(markerData.title);
    }

    public void toggleSpin(bool spinning)
    {
        spinner.spin = spinning;
    }
}
