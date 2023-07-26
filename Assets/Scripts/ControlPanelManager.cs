using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ControlPanelManager : MonoBehaviour
{

    public DataItemList dataItemList;
    public string dataFileName;
    public GameObject dateItemPrefab;

    public GameObject[] panels;
    public GameObject[] displayTeleports;
    public GameObject player;
    private int activePanel = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"starting setup");
        GameObject dataSelectorContent = GameObject.Find("Data Selector/Scroll View/Viewport/Content");
        TextAsset txtAsset = (TextAsset)Resources.Load(dataFileName);
        dataItemList = JsonUtility.FromJson<DataItemList>(txtAsset.text);
        foreach(DataItem dataItem in dataItemList.dataItems)
        {
            var listDataItem = Instantiate(dateItemPrefab, dataSelectorContent.transform);
            var listDataItemManager = listDataItem.GetComponent<DataItemManager>();
            listDataItemManager.setupItem(dataItem);
            Debug.Log($"setup: {dataItem.name}");
        }
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

    }
}
