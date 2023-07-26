using UnityEngine;
using UnityEngine.XR.Management;

namespace SimplestarGame.XR
{
    public class AvatarActivator : MonoBehaviour
    {
        [SerializeField] XRAvatarController avatarController = null;
        [SerializeField] GameObject humanoid = null;

        void Start()
        {
            if (!(null == XRGeneralSettings.Instance
                || null == XRGeneralSettings.Instance.Manager
                || 0 == XRGeneralSettings.Instance.Manager.activeLoaders.Count))
            {
                this.avatarController.AttachGameObject(humanoid.gameObject);
            }
        }
    }
}