using SimplestarGame.Wave;
using UnityEngine;
using UnityEngine.UI;

namespace SimplestarGame.Example 
{ 
    public class PanelParams : MonoBehaviour
    {
        [SerializeField] Slider sliderSpeed;
        [SerializeField] Slider sliderPower;

        void Start()
        {
            this.simulators = FindObjectsOfType<WaveSimulator>();
            float speed = 0;
            float power = 0;
            int index = 0;
            foreach (var simulator in this.simulators)
            {
                if (0 == index)
                {
                    speed = simulator.waveSpeed;
                    power = simulator.wavePower;
                    this.sliderSpeed.minValue = 0.01f;
                    this.sliderSpeed.maxValue = 0.07f;
                    this.sliderSpeed.value = speed;

                    this.sliderPower.minValue = 0.1f;
                    this.sliderPower.maxValue = 10f;
                    this.sliderPower.value = power;
                }
                
                index++;
            }
            this.sliderSpeed.onValueChanged.AddListener(this.OnSpeedChanged);
            this.sliderPower.onValueChanged.AddListener(this.OnPowerChanged);
        }

        void OnSpeedChanged(float speed)
        {
            foreach (var simulator in this.simulators)
            {
                simulator.waveSpeed = speed;
            }
        }

        void OnPowerChanged(float power)
        {
            foreach (var simulator in this.simulators)
            {
                simulator.wavePower = power;
            }
        }

        WaveSimulator[] simulators = null;
    }
}