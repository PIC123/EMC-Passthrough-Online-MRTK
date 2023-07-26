using UnityEngine;
namespace SimplestarGame.Gravity
{
    public class GravityReceiver : MonoBehaviour
    {
        [SerializeField] Gravity gravity;

        internal void SetGravity(Gravity gravity)
        {
            this.gravity = gravity;
        }

        void Start()
        {
            if (this.TryGetComponent(out Rigidbody rigidbody))
            {
                this.rb = rigidbody;
            }
        }

        void FixedUpdate()
        {
            if (null == this.rb || null == this.gravity)
            {
                return;
            }
            var dir = (this.gravity.transform.position - this.transform.position).normalized;
            var distance = Vector3.Distance(this.gravity.transform.position, this.transform.position);
            var r = Mathf.Max(this.gravity.transform.localScale.x, this.gravity.transform.localScale.y, this.gravity.transform.localScale.z);
            if (distance < r)
            {
                var g = this.gravity.G * this.gravity.Mass * this.rb.mass / (r * r);
                this.rb.AddForce(dir * g * distance / r);
            }
            else
            {
                this.rb.AddForce(dir * this.gravity.G * this.gravity.Mass * this.rb.mass / (distance * distance));
            }
        }

        Rigidbody rb;
    }
}