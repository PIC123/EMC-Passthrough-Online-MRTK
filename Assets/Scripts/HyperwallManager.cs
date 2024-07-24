using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class HyperwallManager : MonoBehaviour
{

    public int selectedHyperwall = 0;
    public int selectedPanel = 0;
    public VideoClip[] Clips;
    public Texture[] Images;
    public GameObject[] togglePanels;
    private VideoPlayer videoPlayer;
    private GameObject[] hyperWalls;
    public List<List<GameObject>> allPanelList;

    // Start is called before the first frame update
    void Awake()
    {
        allPanelList = new List<List<GameObject>>();
        hyperWalls = GameObject.FindGameObjectsWithTag("hyperwall");
        int wallCount = 0;
        int panelCount = 0;

        // Iterate through each parent object
        foreach (GameObject hyperWall in hyperWalls)
        {
            // Create a new list to store child panels of the current parent
            List<GameObject> panelList = new List<GameObject>();

            foreach (Transform child in hyperWall.transform)
            {
                // Iterate through grandchildren, since child of hyperwall is "content"
                foreach (Transform grandChild in child)
                {
                    if (grandChild.CompareTag("panel"))
                    {
                        panelList.Add(grandChild.gameObject);
                        PanelData pd = grandChild.GetComponent<PanelData>();
                        pd.hyperwallNum = wallCount;
                        pd.panelNum = panelCount;
                        panelCount++;
                    }
                }
            }
            allPanelList.Add(panelList);
            wallCount++;
        }
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

    public void selectDash(int selectedWall)
    {
        selectedHyperwall = selectedWall;

        // Toggles to enable and disable panel buttons based on which Hyperwall is chose to be edited
        for (int i = 0; i < togglePanels.Length; i++)
        {
            if (i == selectedHyperwall)
            {
                togglePanels[i].SetActive(true);
            }
            else
            {
                togglePanels[i].SetActive(false);
            }
        }


    }

    public void selectPanel(int selectedPan)
    {
        selectedPanel = selectedPan;
    }

    public void selectVid(int clipInd)
    {

        GameObject selectedPanelObject = allPanelList[selectedHyperwall][selectedPanel];
        VideoPlayer panelVideoPlayer = selectedPanelObject.GetComponent<VideoPlayer>();
        panelVideoPlayer.clip = Clips[clipInd];
        videoPlayer.Play();
        selectedPanelObject.GetComponent<PanelSync>().SetContent(0, clipInd);
    }

    public void selectImg(int imgInd)
    {
        GameObject selectedPanelObject = allPanelList[selectedHyperwall][selectedPanel];
        Renderer renderer = selectedPanelObject.GetComponent<Renderer>();
        renderer.material.mainTexture = Images[imgInd];
        VideoPlayer panelVideoPlayer = selectedPanelObject.GetComponent<VideoPlayer>();
        panelVideoPlayer.clip = null;
        selectedPanelObject.GetComponent<PanelSync>().SetContent(1, imgInd);
    }

}
