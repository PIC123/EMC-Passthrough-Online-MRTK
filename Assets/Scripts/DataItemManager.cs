using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataItemManager : MonoBehaviour
{

    public DataItem dataItem;
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
        gameObject.GetComponentInChildren<Text>().text = dataItem.name;
    }
}
