using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataItemManager : MonoBehaviour
{

    public DataItem dataItem;
    public ControlPanelManager panelManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupItem(DataItem newDataItem)
    {
        dataItem = newDataItem;
        gameObject.GetComponentInChildren<TextMeshPro>().text = dataItem.name;
    }

    public void setCurrentItem()
    {
        panelManager.currentDataItem = dataItem;
        panelManager.setPreviewDesc();
    }
}
