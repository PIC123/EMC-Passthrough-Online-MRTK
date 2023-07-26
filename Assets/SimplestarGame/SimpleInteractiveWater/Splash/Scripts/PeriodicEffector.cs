using UnityEngine;

namespace SimplestarGame.Splash
{
    public class PeriodicEffector : MonoBehaviour
    {
        internal void StartEffect(float period)
        {
            this.ps?.Play();
            this.isPlaying = true;
            this.period = period;
        }

        internal void StartPowerEffect(float period)
        {
            this.StartEffect(period);
        }

        void Start()
        {
            if (this.TryGetComponent(out ParticleSystem particleSystem))
            {
                this.ps = particleSystem;
            }
        }

        void Update()
        {
            if (!this.isPlaying)
            {
                return;
            }

            this.period -= Time.deltaTime;
            if (0 > this.period)
            {
                this.ps?.Stop();
                this.isPlaying = false;
            }
        }

        float period = 2.0f;
        ParticleSystem ps = null;
        bool isPlaying = false;
    }
}