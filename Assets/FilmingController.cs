using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityTemplateProjects;

public class FilmingController : MonoBehaviour
{
    public float rotationSpeed = 0.05f;
    private GameObject globe;
    private MapManager mapManager;
    private GlobeManager globeManager;
    private VideoPlayer globeVideo;
    private GameObject particles;
    private SpinFree spinner;
    public Vector3 startRot;
    public Vector3 endRot;
    private int currGlobeAnimation = 0;

    public float timeCount = 0.0f;
    private bool isMoving = false;


    private GameObject[] mapPoints;
    private bool showPoints = true;

    private MapTableSync _mapTableSync;
    private GlobeSync _globeSync;


    // Start is called before the first frame update
    void Start()
    {
        globe = GameObject.Find("Globe");
        startRot = globe.transform.eulerAngles;
        endRot = globe.transform.eulerAngles;
        mapPoints = GameObject.FindGameObjectsWithTag("data_point");
        _mapTableSync = GameObject.Find("Map").GetComponent<MapTableSync>();
        _globeSync = GameObject.Find("Globe Module").GetComponent<GlobeSync>();
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        globeManager = GameObject.Find("Globe").GetComponent<GlobeManager>();
        globeVideo = GameObject.Find("Globe").GetComponent<VideoPlayer>();
        particles = GameObject.Find("Map Table Particles");
        spinner = GameObject.Find("Globe").GetComponent<SpinFree>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            globe.transform.eulerAngles = Vector3.Slerp(startRot, endRot, timeCount);
            timeCount = timeCount + Time.deltaTime;
        }

        if (timeCount > 1)
        {
            isMoving = false;
        }

        if (Input.GetKey("z")) // Pan left
        {
            print("panning left");
            transform.Rotate(0, -rotationSpeed, 0);
        }
        if (Input.GetKey("x")) // Pan right
        {
            print("panning right");
            transform.Rotate(0, rotationSpeed, 0);
        }
        if (Input.GetKey("p")) // Toggle Particles
        {
            print("particles");
            particles.SetActive(!particles.activeSelf);
            _mapTableSync.setParticlesVisible(particles.activeSelf);
        }
        if (Input.GetKey("1")) // Select location 1 (Tokyo)
        {
            spinner.spin = false;
            var tpoint = GameObject.Find("Cambridge");
            var tmpm = tpoint.GetComponent<MapPinManager>();
            var spoint = GameObject.Find("Venice");
            var smpm = spoint.GetComponent<MapPinManager>();
            startRot = globe.transform.eulerAngles;
            endRot = new Vector3(0, 0, 0);
            //globeManager.selectedMarker = tmpm.markerData;
            globeManager.UpdateSelectedMarker(tmpm.markerData);
            _globeSync.SetCurrMarkerTitle(tmpm.markerData.title);
            timeCount = 0;
            isMoving = true;
            //globe.transform.eulerAngles = endRot;
            print("tokyo");
            tmpm.setLatLong();
            tmpm.GetComponent<Renderer>().material = tmpm.selectMat;
            smpm.GetComponent<Renderer>().material = smpm.badMat;
            Debug.Log($"tokyo: {startRot}, {endRot}, {timeCount}");
            //mapManager.setLatLong(tempMarker.latitude, tempMarker.longitude);
            //mapManager.setText();
        }
        if (Input.GetKey("2")) // Select location 2 (sf)
        {
            var spoint = GameObject.Find("Cambridge");
            var smpm = spoint.GetComponent<MapPinManager>();
            var tpoint = GameObject.Find("Venice");
            var tmpm = tpoint.GetComponent<MapPinManager>();
            spinner.spin = false;
            startRot = globe.transform.eulerAngles;
            endRot = new Vector3(0, 101, 0);
            //globeManager.selectedMarker = smpm.markerData;
            globeManager.UpdateSelectedMarker(smpm.markerData);
            _globeSync.SetCurrMarkerTitle(smpm.markerData.title);
            timeCount = 0;
            isMoving = true;
            //globe.transform.eulerAngles = endRot;
            print("sf");
            smpm.setLatLong();
            smpm.GetComponent<Renderer>().material = smpm.selectMat;
            tmpm.GetComponent<Renderer>().material = tmpm.goodMat;
            Debug.Log($"sf: {startRot}, {endRot}, {timeCount}");
            //mapManager.setLatLong(tempMarker.latitude, tempMarker.longitude);
            //mapManager.setText();
        }
        //if (Input.GetKey("l"))
        //{
        //    print("globe toggle");
        //    globeVideo.enabled = !globeVideo.enabled;
        //    showPoints = !showPoints;
        //    foreach(GameObject data_point in mapPoints)
        //    {
        //        data_point.SetActive(showPoints);
        //    }
        //}
        if (Input.GetKey("g")) // Change globe map
        {
            print("change globe map");
            currGlobeAnimation = (currGlobeAnimation + 1) % globeManager.globeAnimationClips.Length;
            _globeSync.SetGlobeLayer(currGlobeAnimation);
            globeManager.setGlobeAnimation(currGlobeAnimation);
        }

        if (Input.GetKey("c")) // Toggle camera controls
        {
            gameObject.GetComponent<SimpleCameraController>().enabled = !gameObject.GetComponent<SimpleCameraController>().enabled;

        }

        //globe.transform.rotation = endRot;


    }
}
