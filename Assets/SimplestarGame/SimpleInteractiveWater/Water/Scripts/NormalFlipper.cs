﻿using UnityEngine;

namespace SimplestarGame.Water
{
    [RequireComponent(typeof(Renderer))]
    public class NormalFlipper : MonoBehaviour
    {
        /// <summary>
        /// Your Camera Transform
        /// </summary>
        [SerializeField] Transform mainCamera = null;

        void Start()
        {
            if (TryGetComponent(out Renderer renderer))
            {
                this.waterMaterial = renderer.material;
            }
        }

        void Update()
        {
            // If in the water, flip water surface normal vector.
            if (null != this.waterMaterial)
            {
                if (null != this.mainCamera)
                {
                    bool isInTheWater = this.transform.position.y > this.mainCamera.position.y;
                    if (this.lastIsInTheWater != isInTheWater)
                    {
                        this.lastIsInTheWater = isInTheWater;
                        this.waterMaterial.SetFloat("_FlipNormal", this.lastIsInTheWater ? -1 : 1);
                    }
                }
            }
        }

        bool lastIsInTheWater = false;
        /// <summary>
        /// Water Shader Material
        /// </summary>
        Material waterMaterial = null;
    }
}
