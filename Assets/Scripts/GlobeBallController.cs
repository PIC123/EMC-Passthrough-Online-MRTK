using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeBallController : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    public bool isGrabbed = false;
    public float angle;
    public GameObject globe;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.localPosition;
        startRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            //var xdir = transform.position.x - startPos.x > 0 ? -1 : 1;
            //var zdir = transform.position.z - startPos.z > 0 ? -1 : 1;
            //var currx = new Vector3(transform.position.x, 0, 0);
            //var origx = new Vector3(startPos.x, 0, 0);
            //var currz = new Vector3(0, 0, transform.position.z);
            //var origz = new Vector3(0, 0, startPos.z);
            //angle = Quaternion.Angle(gameObject.transform.rotation, startRot);
            globe.transform.rotation = gameObject.transform.rotation;
        }
        else
        {
            gameObject.transform.localPosition = startPos;
            //gameObject.transform.rotation = startRot;
        }
    }

    public void toggleGrabbed(bool newState)
    {
        isGrabbed = newState;
    }
}
