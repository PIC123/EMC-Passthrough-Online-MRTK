using SimplestarGame.Gravity;
using SimplestarGame.Wave;
using UnityEngine;

namespace SimplestarGame.Floater
{
    /// <summary>
    /// Floater Logic
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Floater : MonoBehaviour
    {
        [SerializeField] FloaterType floaterType = FloaterType.QuadPlane;

        internal void SetFloaterType(FloaterType floaterType)
        {
            this.floaterType = floaterType;
        }

        void Start()
        {
            if (this.gameObject.TryGetComponent(out Rigidbody rigidbody))
            {
                this.rb = rigidbody;
            }
        }

        void FixedUpdate()
        {
            if (null == this.waterObject)
            {
                return;
            }
            this.AddBuoyantForce(this.waterObject);
        }

        void OnTriggerEnter(Collider other)
        {
            this.AddBuoyantForce(other.transform);
        }

        void AddBuoyantForce(Transform other)
        {
            if (!other.TryGetComponent(out WaveSimulator waveSimulator))
            {
                return;
            }
            this.waterObject = other;
            if (null == this.rb)
            {
                return;
            }
            var dir = Vector3.up;
            this.G = Physics.gravity.magnitude;
            float addedForce = 0;
            this.volume = this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
            float height = waveSimulator.GetWaveHeight(this.transform.position);
            float d = Mathf.Max(Mathf.Abs(Vector3.Dot(dir, this.transform.right) * this.transform.localScale.x),
                Mathf.Abs(Vector3.Dot(dir, this.transform.up) * this.transform.localScale.y),
                Mathf.Abs(Vector3.Dot(dir, this.transform.forward) * this.transform.localScale.z));
            this.minDepth = height + d * 0.5f;
            this.maxDepth = height - d * 0.5f;
            if (FloaterType.QuadPlane == this.floaterType)
            {
                if (other.position.y + this.minDepth > this.transform.position.y)
                {
                    float depth = this.transform.position.y - other.position.y;
                    float t = Mathf.Clamp01((depth - this.minDepth) / (this.maxDepth - this.minDepth));
                    addedForce = this.G * this.volume * t;
                    this.rb.AddForce(dir * addedForce);
                }
            }
            else if (FloaterType.QuadSphere == this.floaterType)
            {
                if (null != other.parent && other.parent.TryGetComponent(out Gravity.Gravity gravity))
                {
                    this.G = gravity.G * gravity.Mass;
                    if (!this.TryGetComponent(out GravityReceiver gravityReceiver))
                    {
                        gravityReceiver = this.gameObject.AddComponent<GravityReceiver>();
                        gravityReceiver.SetGravity(gravity);
                        this.rb.useGravity = false;
                    }
                    dir = (this.transform.position - other.parent.position).normalized;
                    var r = Mathf.Max(other.parent.localScale.x, other.parent.localScale.y, other.parent.localScale.z);
                    var distance = Vector3.Distance(this.transform.position, other.parent.position);
                    if (r + this.minDepth > distance)
                    {
                        var depth = distance - r;
                        float t = Mathf.Clamp01((depth - this.minDepth) / (this.maxDepth - this.minDepth));
                        var g = this.G / (r * r);
                        addedForce = g * distance / r * this.volume * t;
                        this.rb.AddForce(dir * addedForce);
                    }
                }
            }
            
            if (0 == addedForce)
            {
                return;
            }

            float downG = 0.1f * addedForce;
            float scaleX = this.transform.localScale.x;
            float scaleY = this.transform.localScale.y;
            float scaleZ = this.transform.localScale.z;

            bool zMin = false;
            if (scaleY > scaleX)
            {
                if (scaleZ > scaleX)
                {
                    this.rb.AddForceAtPosition(-dir * downG, this.transform.position - Mathf.Sign(Vector3.Dot(dir, this.transform.right)) * this.transform.right * this.transform.localScale.x * 0.5f);
                }
                else
                {
                    zMin = true;
                }
            }
            else
            {
                if (scaleZ > scaleY)
                {
                    this.rb.AddForceAtPosition(-dir * downG, this.transform.position - Mathf.Sign(Vector3.Dot(dir, this.transform.up)) * this.transform.up * this.transform.localScale.y * 0.5f);
                }
                else
                {
                    zMin = true;
                }
            }
            if (zMin)
            {
                this.rb.AddForceAtPosition(-dir * downG, this.transform.position - Mathf.Sign(Vector3.Dot(dir, this.transform.forward)) * this.transform.forward * this.transform.localScale.z * 0.5f);
            }
        }

        float G = 0;
        Rigidbody rb = null;
        Transform waterObject = null;
        float minDepth = 0;
        float maxDepth = 0;
        float volume = 0;
    }

    public enum FloaterType
    {
        QuadPlane = 0,
        QuadSphere = 1,
        Max
    }
}