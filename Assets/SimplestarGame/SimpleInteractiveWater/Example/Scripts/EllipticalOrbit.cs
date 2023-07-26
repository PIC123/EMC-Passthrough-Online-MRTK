using UnityEngine;

namespace SimplestarGame.Example
{
    public class EllipticalOrbit : MonoBehaviour
    {
        [SerializeField] Transform center;
        [SerializeField] Vector3 radius;
        [SerializeField] Vector3 speed;
        void Start()
        {
            this.initialPosition = this.transform.position;
        }

        void FixedUpdate()
        {
            this.transform.position = (null != this.center ? this.center.position : this.initialPosition) + 
                new Vector3(this.radius.x * Mathf.Cos(Time.realtimeSinceStartup * this.speed.x),
                            this.radius.y * Mathf.Sin(Time.realtimeSinceStartup * this.speed.y),
                            this.radius.z * Mathf.Cos(Time.realtimeSinceStartup * this.speed.z));
        }

        Vector3 initialPosition;
    }
}