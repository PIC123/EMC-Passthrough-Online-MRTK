using UnityEngine;

public class LatLongMapper : MonoBehaviour
{

    public Transform marker; // marker object
    public float radius = 5; // globe ball radius (unity units)
    public float latitude = 51.5072f; // lat
    public float longitude = 0.1275f; // long

    // Use this for initialization
    void Start()
    {
        // calculation code taken from 
        // @miquael http://www.actionscript.org/forums/showthread.php3?p=722957#post722957
        // convert lat/long to radians

        latitude = Mathf.PI * latitude / 180;
        longitude = Mathf.PI * longitude / 180;

        // adjust position by radians	
        latitude -= 1.570795765134f; // subtract 90 degrees (in radians)

        // and switch z and y (since z is forward)
        float xPos = (radius) * Mathf.Sin(latitude) * Mathf.Cos(longitude);
        float zPos = (radius) * Mathf.Sin(latitude) * Mathf.Sin(longitude);
        float yPos = (radius) * Mathf.Cos(latitude);


        // move marker to position
        marker.position = new Vector3(xPos, yPos, zPos);
    }
}