using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorSetter : MonoBehaviour
{
    private RandomColorSync _randomColorSync;

    private void Awake()
    {
        _randomColorSync = GameObject.Find("Sphere").GetComponent<RandomColorSync>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRandomColor()
    {
        _randomColorSync.setColor(Random.ColorHSV());
    }
}
