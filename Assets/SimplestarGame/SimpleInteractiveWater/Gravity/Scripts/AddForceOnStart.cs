using UnityEngine;

namespace SimplestarGame.Gravity
{
    public class AddForceOnStart : MonoBehaviour
    {
        [SerializeField] float force = 150;
        void Start()
        {
            if(this.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForce(Vector3.right * this.force);
            }
        }
    }
}