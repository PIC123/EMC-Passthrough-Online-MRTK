using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

[ExecuteInEditMode]
public class ControlPanelManager : MonoBehaviour
{

    //public DataItemList dataItemList;
    //public string dataFileName;
    //public GameObject dataItemPrefab;
    public DataItem currentDataItem;

    public GameObject[] panels;
    public GameObject[] displayTeleports;
    public GameObject player;
    public int activePanel = 0;

    public TextMeshPro dataDescription;

    private Texture previewImg;

    private GameObject[] hyperwalls;
    private bool enableEditMode = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log($"starting setup");
        //GameObject dataSelectorContent = GameObject.Find("Data Selector/Scroll View/Viewport/Content");
        //TextAsset txtAsset = (TextAsset)Resources.Load(dataFileName);
        //dataItemList = JsonUtility.FromJson<DataItemList>(txtAsset.text);
        //foreach(DataItem dataItem in dataItemList.dataItems)
        //{
        //    var listDataItem = Instantiate(dataItemPrefab, dataSelectorContent.transform);
        //    var listDataItemManager = listDataItem.GetComponent<DataItemManager>();
        //    listDataItemManager.setupItem(dataItem);
        //    Debug.Log($"setup: {dataItem.name}");
        //}
        //currentDataItem = dataItemList.dataItems[0];
        hyperwalls = GameObject.FindGameObjectsWithTag("hyperwall");
        Debug.Log($"found {hyperwalls.Length} walls");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchActivePanel(int newActivePanel)
    {
        if(newActivePanel != activePanel)
        {
            panels[activePanel].SetActive(false);
            panels[newActivePanel].SetActive(true);
            activePanel = newActivePanel;
        }
    }

    public void goToDisplay(int displayNum)
    {
        GameObject targetLocation = displayTeleports[displayNum];
        player.transform.position = new Vector3(targetLocation.transform.position.x, player.transform.position.y, targetLocation.transform.position.z);
    }

    //public void setupDataList()
    //{
    //    foreach
    //}

    public void setPreviewDesc()
    {
        dataDescription.text = currentDataItem.description;
    }

    public void toggleWallEditMode()
    {
        enableEditMode = !enableEditMode;
        Debug.Log("new edit bool");
        foreach (GameObject wall in hyperwalls)
        {
            wall.GetComponent<ObjectManipulator>().enabled = enableEditMode;
        }
    }

    IEnumerator GetTexture(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                previewImg = DownloadHandlerTexture.GetContent(uwr);
            }
        }
    }
}
