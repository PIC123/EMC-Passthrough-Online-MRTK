using UnityEngine;

namespace SimplestarGame.Water
{
    [RequireComponent(typeof(Renderer))]
    public class ReflectionProbeCubeTexture : MonoBehaviour
    {
        [SerializeField] Cubemap reflectionMap;
        void Start()
        {
            if (this.TryGetComponent(out Renderer renderer))
            {
                renderer.material.SetTexture("_ReflectionCubeMap", this.reflectionMap);
            }
        }
    }
}
