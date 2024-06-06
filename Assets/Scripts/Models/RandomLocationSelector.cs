using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLocationSelector : MonoBehaviour
{
    private LocationSync _locationSync;
    // Start is called before the first frame update

    private void Awake()
    {
        _locationSync = GameObject.Find("Map").GetComponent<LocationSync>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRandomLocation()
    {
        _locationSync.setLatLong(Random.Range(-90, 90), Random.Range(-180, 180));
    }
}
