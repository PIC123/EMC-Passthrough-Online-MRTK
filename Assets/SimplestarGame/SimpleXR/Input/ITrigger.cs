using UnityEngine;

namespace SimplestarGame.XR
{
    public abstract class ITrigger : MonoBehaviour
    {
        internal abstract void OnTrigger(bool performed, float value);
    }
}