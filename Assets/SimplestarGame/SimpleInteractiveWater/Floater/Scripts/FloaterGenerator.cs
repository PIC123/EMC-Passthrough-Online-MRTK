using UnityEngine;

namespace SimplestarGame.Floater
{
    public class FloaterGenerator : MonoBehaviour
    {
        [SerializeField] FloaterType floaterType = FloaterType.QuadPlane;
        [SerializeField] GameObject prefabFloaterObject;
        [SerializeField] Transform generatePoint;
        [SerializeField] Material litMaterial;
        [SerializeField] float interval = 1f;
        [SerializeField] Vector3 minSize;
        [SerializeField] Vector3 maxSize;

        void Update()
        {
            if (this.lastTime + this.interval < Time.realtimeSinceStartup)
            {
                this.lastTime = Time.realtimeSinceStartup;
                var floater = Instantiate(this.prefabFloaterObject, 
                    this.generatePoint.position + new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f)), 
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
                floater.transform.localScale = new Vector3(
                    Random.Range(this.minSize.x, this.maxSize.x),
                    Random.Range(this.minSize.y, this.maxSize.y),
                    Random.Range(this.minSize.z, this.maxSize.z)
                );
                if (floater.TryGetComponent(out Floater floaterComponent))
                {
                    floaterComponent.SetFloaterType(this.floaterType);
                }
                if (floater.TryGetComponent(out Renderer renderer))
                {
                    renderer.sharedMaterial = this.litMaterial;
                }
                if (floater.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.mass = 0.7f * floater.transform.localScale.x * floater.transform.localScale.y * floater.transform.localScale.z;
                }
                Destroy(floater, 60f);
            }
        }

        float lastTime;
    }
}