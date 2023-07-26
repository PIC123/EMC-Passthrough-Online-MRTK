using UnityEngine;
namespace SimplestarGame.Gravity
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField] float mass = 1f;
        [SerializeField] float gravity = 9.8f;
        internal float Mass => this.mass;
        internal float G => this.gravity;
    }
}
