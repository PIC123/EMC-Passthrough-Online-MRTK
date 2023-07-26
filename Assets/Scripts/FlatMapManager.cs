using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using static GlobeManager;

public class FlatMapManager : MonoBehaviour
{

    public MapPinLayer mapPinLayer;
    public MapPin mapPinPrefab;
    public string fileName;
    public MarkerList markerList;
    public MapPin[] mapPins;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset txtAsset = (TextAsset)Resources.Load(fileName);
        markerList = JsonUtility.FromJson<MarkerList>(txtAsset.text);

        foreach (Marker marker in markerList.markers)
        {
            var mapPin = Instantiate(mapPinPrefab, gameObject.transform);
            mapPin.Location = new LatLon(marker.latitude, marker.longitude);
            var mapPinManager = mapPin.GetComponent<MapPinManager>();
            mapPinManager.markerData = marker;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
