using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using static TestScript.Root;

//[ExecuteInEditMode]
public class StreamingImage : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Object hapticClip;

    [SerializeField] private string filePath;
    [SerializeField] private HapticImpulsePlayer m_player;
    private int index = 0;
#endif

    private List<Root.Envelope> values;


    private CountdownTimer countdownTimer;
    private float m_activeTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdownTimer = GetComponent<CountdownTimer>();
        PrepareNextStep();
    }

    private void Update()
    {
        m_activeTime += Time.deltaTime;
    }

    void TriggerChange()
    {
        //m_player.SendHapticImpulse(m_active_amp, 1, m_active_freq);
        Debug.Log($"New Haptic Pulse: amp: {values[index].amplitude}, freq: {values[index].frequency}, time: {values[index].time}");
        PrepareNextStep();
    }

    void PrepareNextStep()
    {
        float time_left = values[index].time;

        time_left -= m_activeTime;

        if (time_left <= 0)
        {
            TriggerChange();
            return;
        }

        countdownTimer.StartTimer(time_left, TriggerChange);
    }

    public async void ConvertDictionary()
    {
        Root dictionary;
        Debug.Log("The button was pressed!");
        Debug.Log($"File Path is {filePath}");
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
        values = new List<Root.Envelope>();

        values.AddRange(envelopes.amplitude);
        values.AddRange(envelopes.frequency);


        // Sort the combined list by the "time" component
        values.Sort((x, y) => x.time.CompareTo(y.time));
        /*
                foreach (var envelope in values)
                {
                    Debug.Log($"Envelope Details: {envelope.time}, {envelope.amplitude}, {envelope.frequency}");
                }*/
    }

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