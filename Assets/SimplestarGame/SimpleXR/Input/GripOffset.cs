using UnityEngine;

namespace SimplestarGame.XR
{
    public class GripOffset : MonoBehaviour
    {
        [SerializeField] internal bool useGravity = false;
        [SerializeField] internal float gripRadius = 0.1f;
        [SerializeField] internal Vector3 position;
        [SerializeField] internal Vector3 rotationEuler;
    }
}