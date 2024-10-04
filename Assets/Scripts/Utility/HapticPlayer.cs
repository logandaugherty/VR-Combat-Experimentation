using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticPlayer : MonoBehaviour
{
    // Import the Meta Haptic clip Json
    [SerializeField] private UnityEngine.Object m_hapticClip;
    
    // Unity Haptics Output to controller
    [SerializeField] private HapticImpulsePlayer m_player;

    // Control loop details
    [SerializeField] private bool m_loop;
    [SerializeField] private bool m_playOnStart;

    // Playback speed and volume (applied to amplitude)
    [Range(0.1f, 5f)]
    [SerializeField] private float m_speed = 1;

    [Range(0f, 1f)]
    [SerializeField] private float m_volume = 1;

    // Navigate through elemenst in the Json
    private CountdownTimer m_countdownTimer;
    private float m_activeTime;
    private int m_index;

    private bool m_isPlaying;
    
    // Store the list of haptic signals
    private List<Root.Envelope> m_values = new List<Root.Envelope>();

    // Store the current signal to send 
    private Root.Envelope m_envelope = new Root.Envelope();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store the countdown timer based on the component
        m_countdownTimer = GetComponent<CountdownTimer>();
        
        SetClip(m_hapticClip);

        // If play on start is selected, wait for project initialization, then play
        if (m_playOnStart)
        {
            Invoke(nameof(Play),0.1f);
        }
    }


    private void Update()
    {
        // If player is active, increment time at the scale of speed
        if(m_isPlaying)
            m_activeTime += Time.deltaTime * m_speed;
    }

    // Reset details and play clip
    public void Play()
    {
        if (m_hapticClip == null)
            return;

        // Prepare next step increments index by 1. To start at 0, set index at -1
        m_index = -1;

        // Reset time tracker
        m_activeTime = 0;

        m_isPlaying = true;

        m_envelope.amplitude = 0;
        m_envelope.frequency = 0;

        // Prepare first signal for haptics
        IncrementHaptic();
    }


    public void Pause()
    {
        m_isPlaying = false;
    }

    public void Resume()
    {
        m_isPlaying = true;
        IncrementHaptic();
    }

    public void SetLoop(bool loop)
    {
        m_loop = loop;
    }

    public void SetClip(UnityEngine.Object hapticClip)
    {
        if (hapticClip == null)
            return;
        m_hapticClip = hapticClip;
        ConvertDictionary();
    }

    public void PrintDictionary()
    {
        Debug.Log($"There are {m_values.Count} recorded values in the dictionary");
    }

    // Coordinate the current haptic feedback item to look at
    void IncrementHaptic()
    {
        // If the index exceeds a valid range...
        if (++m_index >= m_values.Count)
        {
            // ... and the programmer wants to wrap to the start, reset the clip
            if (m_loop)
                Play();

            // ... exit out of this loop
            return;
        }

        // Store the time left based on the new index subtracted by the current time
        float time_left = m_values[m_index].time - m_activeTime;
       
        // If the time is below or at 0, send signal now
        if (time_left <= 0)
        {
            SendHaptic();
            return;
        }

        // Wait for this time left and send the signal, and increment the haptic
        m_countdownTimer.StartTimer(time_left, () => {
            SendHaptic(); IncrementHaptic();
            });
    }

    // Send haptic feedback to controller by a given index
    void SendHaptic()
    {
        // If the player is not active, do not send haptic feedback
        if (!m_isPlaying)
            return;

        // Store next haptic feedback
        Root.Envelope envelope = m_values[m_index];

        // Add amplitude or frequency to internal envelope, depending on which is available
        if (envelope.amplitude != null)
            m_envelope.amplitude = envelope.amplitude * m_volume;
        else if (envelope.frequency != null)
            m_envelope.frequency = envelope.frequency;

        // Send haptic feedback to controller
        Debug.Log($"Sending Haptic Pulse: a={m_envelope.amplitude}, f={m_envelope.frequency}");
        //m_player.SendHapticImpulse(m_envelope.amplitude.Value, 0.1f, m_envelope.frequency.Value);
    }

    public async void ConvertDictionary()
    {
        Debug.Log("Attempting to Convert Dictionary");

        if (m_hapticClip == null)
            return;

        string filePath = AssetDatabase.GetAssetPath(m_hapticClip.GetInstanceID());

        Root dictionary;
        Debug.Log($"Converting to Dictionary at file {filePath}");
        Debug.Log($"Starting with {m_values.Count} items");
        using (StreamReader r = new StreamReader(filePath))
        {

            string json = await r.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                // Initialize with default structure if the file is empty
                dictionary = new Root();
                return;
            }
            else
            {
                // Deserialize the JSON data into the Root object
                dictionary = JsonConvert.DeserializeObject<Root>(json);
            }
        }

        var envelopes = dictionary.signals.continuous.envelopes;

        // Combine amplitude and frequency lists
        m_values = new List<Root.Envelope>();

        m_values.AddRange(envelopes.amplitude);
        m_values.AddRange(envelopes.frequency);


        // Sort the combined list by the "time" component
        m_values.Sort((x, y) => x.time.CompareTo(y.time));


        Debug.Log($"Conversion Successful with {m_values.Count} items!");
    }

    [Serializable]
    public class Root
    {
        public Signals signals { get; set; } = new Signals();

        public class Signals
        {
            public Continuous continuous { get; set; } = new Continuous();

            public class Continuous
            {
                public Envelopes envelopes { get; set; } = new Envelopes();

                public class Envelopes
                {
                    public List<Envelope> amplitude { get; set; } = new List<Envelope>();
                    public List<Envelope> frequency { get; set; } = new List<Envelope>();
                }
            }
        }

        public class Envelope
        {
            public float time { get; set; }
            public float? amplitude { get; set; }
            public float? frequency { get; set; }
            public Emphasis emphasis { get; set; }

            public class Emphasis
            {
                public float amplitude { get; set; }
                public float frequency { get; set; }
            }
        }
    }
}