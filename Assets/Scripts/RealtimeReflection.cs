using UnityEngine;
using System.Collections;

public class RealtimeReflection : MonoBehaviour
{

    ReflectionProbe probe;
    public float offset;

    void Awake()
    {
        probe = GetComponent<ReflectionProbe>();
    }

    void Update()
    {
        probe.transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,//(Camera.main.transform.position.y * -1) + offset,
            Camera.main.transform.position.z
        );

        probe.RenderProbe();
    }
}