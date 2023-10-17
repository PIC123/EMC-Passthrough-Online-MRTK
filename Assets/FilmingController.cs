using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FilmingController : MonoBehaviour
{
    public float rotationSpeed = 0.05f;
    public GameObject globe;
    public MapManager mapManager;
    public GlobeManager globeManager;
    public VideoPlayer globeVideo;
    public GameObject particles;
    public SpinFree spinner;
    public Vector3 startRot;
    public Vector3 endRot;

    public float timeCount = 0.0f;
    private bool isMoving = false;


    private GameObject[] mapPoints;
    private bool showPoints = true;

    // Start is called before the first frame update
    void Start()
    {
        startRot = globe.transform.eulerAngles;
        endRot = globe.transform.eulerAngles;
        mapPoints = GameObject.FindGameObjectsWithTag("data_point");
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

        if (Input.GetKey("z"))
        {
            print("panning left");
            transform.Rotate(0, -rotationSpeed, 0);
        }
        if (Input.GetKey("x"))
        {
            print("panning right");
            transform.Rotate(0, rotationSpeed, 0);
        }
        if (Input.GetKey("p"))
        {
            print("particles");
            particles.SetActive(!particles.activeSelf);
        }
        if (Input.GetKey("t"))
        {
            //var tempMarker = new GlobeManager.Marker
            //{
            //    title="Tokyo",
            //    latitude=35.6895f,
            //    longitude= 139.6917f,
            //    co2= 25934,
            //    ch4= 16,
            //    n2o= 13,
            //    h2o= 12304,
            //    description= "Tokyo is an interesting place"
            //};
            spinner.spin = false;
            var tpoint = GameObject.Find("Tokyo");
            var tmpm = tpoint.GetComponent<MapPinManager>();
            var spoint = GameObject.Find("San Francisco");
            var smpm = spoint.GetComponent<MapPinManager>();
            startRot = globe.transform.eulerAngles;
            endRot = new Vector3(0, 0, 0);
            globeManager.selectedMarker = tmpm.markerData;
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
        if (Input.GetKey("s"))
        {
            var spoint = GameObject.Find("San Francisco");
            var smpm = spoint.GetComponent<MapPinManager>();
            var tpoint = GameObject.Find("Tokyo");
            var tmpm = tpoint.GetComponent<MapPinManager>();
            spinner.spin = false;
            //var tempMarker = new GlobeManager.Marker
            //{
            //    title="San Francisco",
            //    latitude= 37.7749f,
            //    longitude= -122.4194f,
            //    co2= 34358,
            //    ch4= 16,
            //    n2o= 18,
            //    h2o= 10456,
            //    description= "SF is an interesting place"
            //};
            startRot = globe.transform.eulerAngles;
            endRot = new Vector3(0, 101, 0);
            globeManager.selectedMarker = smpm.markerData;
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
        if (Input.GetKey("g"))
        {
            print("globe toggle");
            globeVideo.enabled = !globeVideo.enabled;
            showPoints = !showPoints;
            foreach(GameObject data_point in mapPoints)
            {
                data_point.SetActive(showPoints);
            }
        }

        //globe.transform.rotation = endRot;


    }
}
