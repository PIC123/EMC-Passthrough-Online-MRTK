using System.Collections;
using UnityEngine;

namespace SimplestarGame.XR
{
    public class DelayActivator : MonoBehaviour
    {
        [SerializeField] float delay = 15f;
        [SerializeField] GameObject[] targetObjects;

        void Start()
        {
            StartCoroutine(this.CoActivateObjects(this.delay));
        }

        IEnumerator CoActivateObjects(float delay)
        {
            yield return new WaitForSeconds(delay);
            foreach (var obj in this.targetObjects)
            {
                obj.SetActive(true);
            }
        }
    }
}