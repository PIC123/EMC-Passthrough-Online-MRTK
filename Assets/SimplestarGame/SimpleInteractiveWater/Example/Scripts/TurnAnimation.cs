using UnityEngine;

namespace SimplestarGame.Example
{
    public class TurnAnimation : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float rotationSpeed = 0.001f;
        [SerializeField] float rotationRudius = 3;
        [SerializeField] float offsetHeight = 0.2f;

        void Update()
        {
            if (null != this.target)
            {
                this.currentRadian += this.rotationSpeed;
                this.transform.position = this.target.position
                    + new Vector3(Mathf.Cos(this.currentRadian), this.offsetHeight, Mathf.Sin(this.currentRadian)) * this.rotationRudius;
                this.transform.LookAt(this.target.position, Vector3.up);
            }
        }

        float currentRadian = 0;
    }
}