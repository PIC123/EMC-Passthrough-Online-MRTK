using UnityEngine;

namespace SimplestarGame.XR
{
    public class Shooter : ITrigger
    {
        [SerializeField] GameObject prefabObject;
        [SerializeField, Range(0, 2000f)] float shootPower = 500f;
        [SerializeField, Range(0.02f, 5f)] float interval = 0.2f;
        

        void Update()
        {
            if (0.1f < this.triggerValue)
            {
                if (this.lastShootTime + this.interval / this.triggerValue < Time.realtimeSinceStartup)
                {
                    this.lastShootTime = Time.realtimeSinceStartup;
                    this.Shoot();
                }
            }
        }

        void Shoot()
        {
            var ball = Instantiate(this.prefabObject,
                this.transform.position + new Vector3(0, Random.Range(-0.01f, 0.01f), 0),
                this.transform.rotation * Quaternion.Euler(new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f))), null);
            GameObject.Destroy(ball, 10f);
            if (ball.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                var direction = -this.transform.up;
                rigidbody.AddForce(direction * this.shootPower);
            }
        }

        internal override void OnTrigger(bool performed, float value)
        {
            this.triggerValue = value;
        }

        float triggerValue = 0.0f;
        float lastShootTime = 0;
    }
}