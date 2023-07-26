using UnityEngine;

namespace SimplestarGame.Example
{
    public class RandomObjectGen : MonoBehaviour
    {
        [SerializeField] Transform genPoint;
        [SerializeField] float interval = 1f;

        [SerializeField] GameObject[] prefabs;
        [SerializeField] Material[] materials;

        void Update()
        {
            if (this.lastGenTime + this.interval < Time.realtimeSinceStartup)
            {
                this.lastGenTime = Time.realtimeSinceStartup;
                var model = Instantiate(this.prefabs[Random.Range(0, this.prefabs.Length)], this.genPoint.position, 
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), null);
                GameObject.Destroy(model, 60f);
                model.transform.localScale = this.genPoint.localScale;
                if (model.TryGetComponent(out Renderer renderer))
                {
                    renderer.sharedMaterial = this.materials[Random.Range(0, this.materials.Length)];
                }
            }
        }

        float lastGenTime = 0;
    }
}