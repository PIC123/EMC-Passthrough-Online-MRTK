using System;
using UnityEngine;

namespace SimplestarGame.Wave
{
    /// <summary>
    /// Wave Update Event Provider
    /// </summary>
    public class WaveUpdater : MonoBehaviour
    {
        [SerializeField, Range(1, 5)] int updateTimes = 1;
        /// <summary>
        /// Add Waves Event
        /// </summary>
        internal Action onAddWaves;
        /// <summary>
        /// Update Texture Event
        /// </summary>
        internal Action onUpdateTexture;
        /// <summary>
        /// Replace Texture Event
        /// </summary>
        internal Action onReplaceTexture;

        /// <summary>
        /// Singleton Instance
        /// </summary>
        static internal WaveUpdater Instance { get; private set; }

        void Awake()
        {
            WaveUpdater.Instance = this;
        }

        void FixedUpdate()
        {
            this.onAddWaves?.Invoke();
            for (int counter = 0; counter < this.updateTimes; counter++)
            {
                this.onUpdateTexture?.Invoke();
                this.onReplaceTexture?.Invoke();
            }
        }
    }
}