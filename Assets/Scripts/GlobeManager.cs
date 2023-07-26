using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine.Video;
using UnityEngine.UI;

public class GlobeManager : MonoBehaviour
{
    [Serializable]
    public class MarkerList
    {
        public Marker[] markers;
    }

    [Serializable]
    public class Marker
    {
        public float latitude;
        public float longitude;
        public string title;
        public string description;
        public float co2;
        public float ch4;
        public float n2o;
        public float h2o;
    }

    public GameObject markerPrefab;
    public string fileName;
    public MapRenderer mapRenderer;
    public Material[] globeMaterials;
    public string[] globeAnimationUrls;
    private MarkerList markerList;
    private float radius;
    private Renderer globeMaterialRenderer;
    private VideoPlayer videoPlayer;
    public Marker selectedMarker;
    public MapManager mapManager;
    //private SpinFree spinner;
    //public Slider spinSlider;
    //public Slider scaleSlider;
    //public GlobeBallController ballController;

    // Start is called before the first frame update
    void Start()
    {
        globeMaterialRenderer = gameObject.GetComponent<Renderer>();
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        radius = gameObject.transform.localScale.x * 0.55f;
        //spinner = gameObject.GetComponent<SpinFree>();
        //radius = gameObject.transform.localScale.x / 1.75f;
        TextAsset txtAsset = (TextAsset)Resources.Load(fileName);
        markerList = JsonUtility.FromJson<MarkerList>(txtAsset.text);
        Debug.Log("test");
        Debug.Log(markerList.markers[0].title);
        foreach (Marker marker in markerList.markers)
        {
            //Get correct position
            var correctedPos = ConvertLatLong(marker.latitude, marker.longitude, radius) + transform.position;
            Debug.Log(correctedPos);
            // Get correct orientation
            var correctedRot = AlignRotation(correctedPos);
            // Instantiate marker
            var mapMarker = Instantiate(markerPrefab, correctedPos, correctedRot, transform);
            var mapPinManager = mapMarker.GetComponent<MapPinManager>();
            mapPinManager.setupPin(marker);
            Debug.Log(marker.title);
        }
        selectedMarker = markerList.markers[0];
        mapManager.setLatLong(selectedMarker.latitude, selectedMarker.longitude);
    }

    // Update is called once per frame
    void Update()
    {
        //if (ballController.isGrabbed)
        //{
        //    spinner.spin = false;
        //} else
        //{
        //    spinner.spin = true;
        //    spinner.setSpinspeed(spinSlider.value * 3);
        //}
        //var fixedScale = (scaleSlider.value * 3) + 1;
        //gameObject.transform.localScale = new Vector3(fixedScale, fixedScale, fixedScale);
    }

    //void Read(string path)
    //{
    //    string[] Lines = System.IO.File.ReadAllLines(path);
    //    string[] Columns = Lines[/*   INDEX  */].Split(';');
    //    for (int i = 0; i <= Lines.Length - 1; i++)
    //    {
    //        Debug.Log(Lines[i]);
    //    }
    //}

    public Quaternion AlignRotation(Vector3 markerPos)
    {
        var lookDir = markerPos - transform.position; //Find the correct rotation
        var addition = new Vector3(90, 0, 0);
        //var addition = new Vector3(0, 0, 90);
        return Quaternion.LookRotation(lookDir, Vector3.up) * Quaternion.Euler(addition);
    }

    public Vector3 ConvertLatLong(float latitude, float longitude, float radius)
    {
        latitude = Mathf.PI * latitude / 180;
        longitude = Mathf.PI * longitude / 180;

        // adjust position by radians	
        latitude -= 1.570795765134f; // subtract 90 degrees (in radians)

        // and switch z and y (since z is forward)
        float xPos = (radius) * Mathf.Sin(latitude) * Mathf.Cos(longitude);
        float zPos = (radius) * Mathf.Sin(latitude) * Mathf.Sin(longitude);
        float yPos = (radius) * Mathf.Cos(latitude);


        // return new position
        return new Vector3(xPos, yPos, zPos);
    }

    public void setGlobeMap(int mapType)
    {
        globeMaterialRenderer.material = globeMaterials[mapType];
    }

    public void setGlobeAnimation(int mapType)
    {
        videoPlayer.url = globeAnimationUrls[mapType];
    }

    public void setGlobeScale(float globescale)
    {
        var fixedScale = (globescale * 3) + 1;
        gameObject.transform.localScale = new Vector3(fixedScale, fixedScale, fixedScale);
    }
}
