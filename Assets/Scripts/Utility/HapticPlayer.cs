using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticPlayer : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object hapticClip;
    [SerializeField] private HapticImpulsePlayer m_player;

    private CountdownTimer m_countdownTimer;
    private string filePath;
    private List<Root.Envelope> m_values = new List<Root.Envelope>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConvertDictionary();
        m_countdownTimer = GetComponent<CountdownTimer>();
    }

    public void SetClip(UnityEngine.Object hapticClip)
    {
        this.hapticClip = hapticClip;
    }

    private void GenerateFilePath()
    {
        if (hapticClip == null)
        {
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(hapticClip.GetInstanceID());
        filePath = assetPath;
    }

    public void PrintDictionary()
    {
        Debug.Log($"There are {m_values.Count} recorded values in the dictionary");
    }

    public async void ConvertDictionary()
    {
        GenerateFilePath();

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