using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class HyperwallManager : MonoBehaviour  
{
    
    // Initiate all global variables
    public VideoClip earthVideo;
    public VideoClip warmingVideo;
    public Texture warmingImage;
    public GameObject Panel1x2;
    public GameObject Panel2x2;
    public GameObject Panel;
    private string startPanel;
    private string newPanel1;
    private string newPanel2;
    private string newPanel3;
    private string newPanel4; 
    private bool Hyperwall1 = false;
    private bool Hyperwall2 = false;
    private bool isToggle1 = false;
    private bool isToggle2 = false;
    private bool isToggle3 = false;
    private bool isToggle4 = false;
    private bool? isImageVisible;
    private VideoPlayer videoPlayer;
    private Texture sceneImage;



    //public bool isMapOpen = false;
    //public GameObject contents;
    //public GameObject mapContainer;


    // Start is called before the first frame update
    void Start()
    {
        startPanel = "TestPanel";
        isImageVisible = null;


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

        
        if (isImageVisible == false)
        {
            videoPlayer.enabled = true;
        }
        else if (isImageVisible == true) 
        {
            videoPlayer.enabled = false;
        }
        else 
        {
        // Do nothing
        }
      
    }

    public void toggleHW1() 
    {

        Hyperwall1 = true;
        Hyperwall2 = false;
        Panel1x2.SetActive(true);
        Panel2x2.SetActive(false);
    
    }

    public void toggleHW2() 
    {
    
        Hyperwall2= true;
        Hyperwall1= false;
        Panel1x2.SetActive(false);
        Panel2x2.SetActive(true);


    }
    public void toggleOne()
    {
        if (Hyperwall1)
        {
            isToggle1 = true;
            isToggle2 = false;
            isToggle3 = false;
            isToggle4 = false;
            if (isToggle1)
            {
                newPanel1 = startPanel + "1";
            }
        }
        else if (Hyperwall2) 
        {

            isToggle1 = true;
            isToggle2 = false;
            isToggle3 = false;
            isToggle4 = false;
            if (isToggle1)
            {
                newPanel1 = startPanel + "1N";
            }

        }

    }

    public void toggleTwo()
    {
        if (Hyperwall1)
        {
            isToggle1 = false;
            isToggle2 = true;
            isToggle3 = false;
            isToggle4 = false;
            if (isToggle2)
            {
                newPanel2 = startPanel + "2";
            }
        }
        else if (Hyperwall2)
        {

            isToggle1 = false;
            isToggle2 = true;
            isToggle3 = false;
            isToggle4 = false;
            if (isToggle2)
            {
                newPanel2 = startPanel + "2N";
            }

        }

    }

    public void toggleThree()
    {
        if (Hyperwall2)
        {
            isToggle2 = false;
            isToggle1 = false;
            isToggle3 = true;
            isToggle4 = false;
            if (isToggle3)
            {
                newPanel3 = startPanel + "3N";
            }
        }
        else 
        {
        // Do nothing
        }

    }

    public void toggleFour()
    {
        if (Hyperwall2)
        {
            isToggle2 = false;
            isToggle1 = false;
            isToggle3 = false;
            isToggle4 = true;
            if (isToggle4)
            {
                newPanel4 = startPanel + "4N";
            }
        }
        else 
        {
        // Do nothing
        }

    }

    public void EarthVid()
    {
        isImageVisible= false;
        if (isToggle1)
        {
            Panel = GameObject.Find(newPanel1);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = earthVideo;
            videoPlayer.Play();
        }

        if (isToggle2)
        {
            Panel = GameObject.Find(newPanel2);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = earthVideo;
            videoPlayer.Play();
        }

        if (isToggle3)
        {
            Panel = GameObject.Find(newPanel3);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = earthVideo;
            videoPlayer.Play();
        }

        if (isToggle4)
        {
            Panel = GameObject.Find(newPanel4);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = earthVideo;
            videoPlayer.Play();
        }

    }

    public void WarmingVid()
    {
        isImageVisible = false;
        if (isToggle1)
        {
            Panel = GameObject.Find(newPanel1);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = warmingVideo;
            videoPlayer.Play();
        }

        if (isToggle2)
        {
            Panel = GameObject.Find(newPanel2);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = warmingVideo;
            videoPlayer.Play();
        }

        if (isToggle3)
        {
            Panel = GameObject.Find(newPanel3);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = warmingVideo;
            videoPlayer.Play();
        }

        if (isToggle4)
        {
            Panel = GameObject.Find(newPanel4);
            videoPlayer = Panel.GetComponent<VideoPlayer>();
            videoPlayer.clip = warmingVideo;
            videoPlayer.Play();
        }

    }

    public void WarmingImage() 
    {
        isImageVisible = true;
        if (isToggle1)
        {
            Panel = GameObject.Find(newPanel1);
            Renderer renderer = Panel.GetComponent<Renderer>();
            renderer.material.mainTexture = warmingImage;
        }

        if (isToggle2)
        {
            Panel = GameObject.Find(newPanel2);
            Renderer renderer = Panel.GetComponent<Renderer>();
            renderer.material.mainTexture = warmingImage;
        }

        if (isToggle3)
        {
            Panel = GameObject.Find(newPanel3);
            Renderer renderer = Panel.GetComponent<Renderer>();
            renderer.material.mainTexture = warmingImage;
        }

        if (isToggle4)
        {
            Panel = GameObject.Find(newPanel4);
            Renderer renderer = Panel.GetComponent<Renderer>();
            renderer.material.mainTexture = warmingImage;
        }

    }


    //public void toggleMap()
    //{
    //    isMapOpen = !isMapOpen;
    //}
}
