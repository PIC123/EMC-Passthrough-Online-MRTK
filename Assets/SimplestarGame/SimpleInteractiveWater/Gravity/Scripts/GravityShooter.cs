using System.Collections;
using UnityEngine;

namespace SimplestarGame.Gravity
{
    public class GravityShooter : MonoBehaviour
    {
        [SerializeField] Gravity[] gravities;
        [SerializeField] GameObject prefabReciever;
        [SerializeField] float power = 0.75f;

        void Start()
        {
            this.defaultSize = this.transform.localScale;
        }

        void Update()
        {
            if (this.lastShootTime + this.interval < Time.realtimeSinceStartup)
            {
                this.lastShootTime = Time.realtimeSinceStartup;
                this.interval = Mathf.Max(this.interval * 0.8f, 0.2f);
                this.Shot();
            }
        }

        void Shot()
        {
            StartCoroutine(this.CoSizeAnimation(1.5f, 0.1f));
            this.transform.rotation = Quaternion.Euler(Random.Range(-30, 30), 0, 0);
            var reciever = Instantiate(this.prefabReciever,
                this.transform.position + this.transform.forward * this.transform.localScale.z / 2f + new Vector3(0, Random.Range(-0.01f, 0.01f), 0),
                Quaternion.identity, null);
            GameObject.Destroy(reciever, 60f);
            foreach (var gravity in this.gravities)
            {
                var gravityReceiver = reciever.AddComponent<GravityReceiver>();
                gravityReceiver.SetGravity(gravity);
            }
            if (reciever.TryGetComponent(out Rigidbody rigidbody))
            {
                var direction = this.transform.forward;
                rigidbody.AddForce(direction * this.power);
            }
        }

        IEnumerator CoSizeAnimation(float size, float time)
        {
            this.transform.localScale = this.defaultSize * size;
            var endTime = Time.realtimeSinceStartup + time;
            while (endTime > Time.realtimeSinceStartup)
            {
                this.transform.localScale = this.defaultSize * Mathf.Lerp(1f, size, (endTime - Time.realtimeSinceStartup) / time);
                yield return null;
            }
            this.transform.localScale = this.defaultSize;
        }

        float interval = 12;
        Vector3 defaultSize = Vector3.one;
        float lastShootTime = 0;
    }
}