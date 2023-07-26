using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickBallController : MonoBehaviour
{
    public Vector3 startPos;
    private Quaternion startRot;
    public bool isGrabbed = false;
    public float xdist;
    public float zdist;
    public float heading;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            var xdir = transform.position.x - startPos.x > 0 ? -1 : 1;
            var zdir = transform.position.z - startPos.z > 0 ? -1 : 1;
            var currx = new Vector3(transform.position.x, 0, 0);
            var origx = new Vector3(startPos.x, 0, 0);
            var currz = new Vector3(0, 0, transform.position.z);
            var origz = new Vector3(0, 0, startPos.z);
            xdist = Vector3.Distance(currx, origx) * xdir;
            zdist = Vector3.Distance(currz, origz) * zdir;
            heading = Vector3.Angle(gameObject.transform.position, startPos) * Mathf.Deg2Rad;
            angle = Quaternion.Angle(gameObject.transform.rotation, startRot);
        } else
        {
            gameObject.transform.localPosition = new Vector3(0,0,0);
            gameObject.transform.rotation = startRot;
        }
    }

    public void SetGrabState(bool state)
    {
        isGrabbed = state;
    }

    public void SetStartPos()
    {
        startPos = gameObject.transform.position;
    }
}
