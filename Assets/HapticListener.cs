using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.UI;

public class HapticListener : HapticImpulsePlayer
{   
    
    [SerializeField] private HapticChannelScriptableObjectScript m_hapticChannel;
    [SerializeField] private Slider m_ampSlider;
    [SerializeField] private Slider m_freqSlider;

    private List<EmissionDetails> m_emissions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_hapticChannel.AddListener(OnEmission);
    }

    private void Update()
    {
        float maxAmp = 0;
        float maxFreq = 0;
        foreach (var item in m_emissions){
            maxAmp = Mathf.Max(item.freq, maxAmp);
            maxFreq = Mathf.Max(item.amp, maxFreq);
        }
        if (maxAmp != 0 && maxFreq != 0)
        {
            m_ampSlider.value = maxAmp;
            m_freqSlider.value = maxFreq;
            SendHapticImpulse(maxAmp, 0.1f, maxFreq);
        }

    }

    void OnEmission(Vector3 source, float amp, float freq, float min, float max)
    {
        m_emissions.Add( new EmissionDetails(source, freq, amp, min, max) );
        float distance = Mathf.Min(Vector3.Distance(source, transform.position), min);
        float scalar = distance + (max - distance);
        SendHapticImpulse(amp*scalar, 0.1f, freq * scalar);
    }

    struct EmissionDetails
    {
        public Vector3 source;
        public float freq;
        public float amp;
        public float min;
        public float max;

        public EmissionDetails(Vector3 source, float freq, float amp, float min, float max)
        {
            this.source = source;
            this.freq = freq;
            this.amp = amp;
            this.min = min;
            this.max = max;
        }
    }
}
