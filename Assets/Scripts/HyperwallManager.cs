using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class HyperwallManager : MonoBehaviour
{

    public VideoClip earthVideo;
    public VideoClip warmingVideo;
    public GameObject Panel;
    private string startPanel;
    private string newPanel1;
    private string newPanel2;
    private bool isToggle1 = false;
    private bool isToggle2 = false;
    
    //public bool isMapOpen = false;
    //public GameObject contents;
    //public GameObject mapContainer;


    // Start is called before the first frame update
    void Start()
    {
        startPanel = "TestPanel";
       
        

    }


    
    // Update is called once per frame
    void Update()
    {
        //if (isMapOpen)
        //{
        //    contents.SetActive(false);
        //    mapContainer.transform.localPosition = new Vector3(0, 0, 4.6f);
        //    mapContainer.transform.localScale = new Vector3(1, 1, 1.97f);
        //} else
        //{
        //    mapContainer.transform.localPosition = new Vector3(0.76f, 5.43f, -1.55f);
        //    mapContainer.transform.localScale = new Vector3(1, 0.51f, 0.94f);
        //    contents.SetActive(true);
        //}
    }
    public void toggleOne()
    {
        isToggle1 = true;
        if (isToggle1)
        {
            newPanel1 = startPanel + "1";
        }

    }

    public void toggleTwo()
    {
        isToggle2 = true;
        if (isToggle2)
        {
            newPanel2 = startPanel + "2";
        }

    }

    public void EarthVid()
    {
        if (isToggle1)
        {
            GameObject Panel = GameObject.FindWithTag(newPanel1);
            VideoPlayer videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = earthVideo;
            videoPlayer.Play();
        }

        if (isToggle2)
        {
            GameObject Panel = GameObject.FindWithTag(newPanel2);
            VideoPlayer videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = earthVideo;
            videoPlayer.Play();
        }

    }

    public void WarmingVid()
    {

        if (isToggle1)
        {
            GameObject Panel = GameObject.FindWithTag(newPanel1);
            VideoPlayer videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = warmingVideo;
            videoPlayer.Play();
        }

        if (isToggle2)
        {
            GameObject Panel = GameObject.FindWithTag(newPanel2);
            VideoPlayer videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = warmingVideo;
            videoPlayer.Play();
        }



    }


    //public void toggleMap()
    //{
    //    isMapOpen = !isMapOpen;
    //}
}
