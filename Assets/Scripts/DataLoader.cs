using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [Serializable]
    public class DataItem
    {
        public string name;
        public string url;
        public string description;
        public string[] tags;
    }

    public DataItem[] dataItems;
    public string fileName;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset txtAsset = (TextAsset)Resources.Load(fileName);
        dataItems = JsonUtility.FromJson<DataItem[]>(txtAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
