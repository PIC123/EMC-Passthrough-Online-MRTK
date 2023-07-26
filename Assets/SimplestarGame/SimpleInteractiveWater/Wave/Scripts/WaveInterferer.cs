using SimplestarGame.Splash;
using UnityEngine;

namespace SimplestarGame.Wave
{
    /// <summary>
    /// If collision object has WaveSimulator, Add wave with its velocity.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WaveInterferer : MonoBehaviour
    {
        [SerializeField] WaveType waveType = WaveType.Point;
        [SerializeField, Range(0.01f, 1f)] float radius = 0.2f;
        [SerializeField, Range(0.01f, 20f)] float length = 0.5f;
        [SerializeField] PeriodicEffector waterDrop;
        [SerializeField] PeriodicEffector waterSplash;

        void Start()
        {
            this.lastPoint = transform.position;
            this.waterLayer = LayerMask.NameToLayer("Water");
            this.InitSphereYs();
        }

        void OnValidate()
        {
            if (this.waveType != WaveType.Point)
            {
                this.InitSphereYs();
                if (null == this.sphereY || null == this.sphere_Y)
                {
                    this.waveType = WaveType.Point;
                }
                else
                {
                    float r = Mathf.Max(0.01f, this.radius);
                    this.sphereY.localScale = this.sphere_Y.localScale = new Vector3(1f, r / this.length, 1f);
                    this.transform.localScale = new Vector3(r, this.length, r);
                }
            }
            else
            {
                float r = Mathf.Max(0.01f, this.radius);
                this.transform.localScale = Vector3.one * this.radius;
            }
        }

        void InitSphereYs()
        {
            foreach (Transform child in this.transform)
            {
                if (this.SphereYName == child.name)
                {
                    this.sphereY = child;
                }
                if (this.Sphere_YName == child.name)
                {
                    this.sphere_Y = child;
                }
            }
        }

        void FixedUpdate()
        {
            this.velocity = (this.transform.position - this.lastPoint) / Time.deltaTime;
            this.lastPoint = transform.position;    
        }

        void OnTriggerEnter(Collider other)
        {
            var waveComputer = other.gameObject.GetComponent<WaveSimulator>();
            if (null == waveComputer)
            {
                return;
            }
            if (0 < Vector3.Dot(this.velocity, other.transform.up))
            {
                return;
            }
            var normalizedVelocity = this.velocity.normalized;
            if (Physics.Raycast(this.transform.position - this.velocity * 0.5f, normalizedVelocity, out RaycastHit hit, Vector3.Distance(Vector3.zero, this.velocity),  (1 << this.waterLayer))){
                switch (this.waveType)
                {
                    case WaveType.Point:
                        {
                            waveComputer.AddWavePoint(hit.point, this.radius, this.velocity);
                        }
                        break;
                    case WaveType.Line:
                        {
                            waveComputer.AddWaveLine(new Vector3(this.sphereY.position.x, hit.point.y, this.sphereY.position.z), 
                                new Vector3(this.sphere_Y.position.x, hit.point.y,  this.sphere_Y.position.z), 
                                this.radius, this.velocity);
                        }
                        break;
                }
                if (null != this.waterSplash)
                {
                    this.waterSplash.transform.position = hit.point;
                    this.waterSplash.transform.rotation = Quaternion.FromToRotation(this.waterSplash.transform.up, hit.normal) * this.waterSplash.transform.rotation;
                    this.waterSplash.StartPowerEffect(0.2f);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            var waveComputer = other.gameObject.GetComponent<WaveSimulator>();
            if (null == waveComputer)
            {
                return;
            }
            if (0 > Vector3.Dot(this.velocity, other.transform.up))
            {
                return;
            }
            var normalizedVelocity = this.velocity.normalized;
            if (Physics.Raycast(this.transform.position + this.velocity * 0.5f, -normalizedVelocity, out RaycastHit hit, Vector3.Distance(Vector3.zero, this.velocity), (1 << this.waterLayer)))
            {
                switch (this.waveType)
                {
                    case WaveType.Point:
                        {
                            waveComputer.AddWavePoint(hit.point, this.radius, this.velocity);
                        }
                        break;
                    case WaveType.Line:
                        {
                            waveComputer.AddWaveLine(new Vector3(this.sphereY.position.x, hit.point.y, this.sphereY.position.z),
                                new Vector3(this.sphere_Y.position.x, hit.point.y, this.sphere_Y.position.z),
                                this.radius, this.velocity);
                        }
                        break;
                }
                if (null != this.waterDrop)
                {
                    this.waterDrop.StartEffect(0.2f);
                }
            }
        }

        Vector3 velocity;
        Vector3 lastPoint;
        int waterLayer;
        readonly string SphereYName = "SphereY";
        readonly string Sphere_YName = "Sphere_Y";
        Transform sphereY = null;
        Transform sphere_Y = null;
    }

    public enum WaveType
    {
        Point = 0,
        Line,
    }
}
