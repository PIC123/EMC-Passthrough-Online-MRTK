using UnityEngine;

namespace SimplestarGame.Wave
{
    public class WaveHightChecker : MonoBehaviour
    {
        void Start()
        {
            this.waterLayer = LayerMask.NameToLayer("Water");
        }

        void Update()
        {
            if(Physics.Raycast(new Ray(this.transform.position + Vector3.up * 10f, Vector3.down), out RaycastHit hit, 20f, (1 << this.waterLayer)))
            {
                if (hit.collider.gameObject.TryGetComponent(out WaveSimulator waveSimulator))
                {
                    float height = waveSimulator.GetWaveHeight(hit.point);
                    this.transform.position = new Vector3(this.transform.position.x, height + hit.collider.gameObject.transform.position.y, this.transform.position.z);
                }
            }
        }

        int waterLayer;
    }
}