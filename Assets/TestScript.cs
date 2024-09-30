using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public Object m_object;

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log(m_object);
            Debug.Log(m_object.GetHashCode());
            Debug.Log(m_object.name);
            Debug.Log(m_object.ToString());
        }
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
            public double time { get; set; }
            public double? amplitude { get; set; }
            public double? frequency { get; set; }
            public Emphasis emphasis { get; set; }

            public class Emphasis
            {
                public double amplitude { get; set; }
                public double frequency { get; set; }
            }
        }
    }

    static void FormatEnvelopes(string filename, string path)
    {
        // Check if the input file exists, if not, create it
        if (!File.Exists(filename))
        {
            File.Create(filename).Close();
        }

        Root dictionary;
        using (StreamReader r = new StreamReader(filename))
        {
            // Read the JSON data from the file
            string json = r.ReadToEnd();
            if (string.IsNullOrWhiteSpace(json))
            {
                // Initialize with default structure if the file is empty
                dictionary = new Root();
            }
            else
            {
                // Deserialize the JSON data into the Root object
                dictionary = JsonConvert.DeserializeObject<Root>(json);
                Debug.Log(dictionary);
                return;
            }
        }

        var envelopes = dictionary.signals.continuous.envelopes;

        // Combine amplitude and frequency lists
        var values = new List<Root.Envelope>();
        values.AddRange(envelopes.amplitude);
        values.AddRange(envelopes.frequency);

        // Sort the combined list by the "time" component
        values.Sort((x, y) => x.time.CompareTo(y.time));

        // Construct the output file path
        string outputFile = Path.Combine(path, Path.GetFileNameWithoutExtension(filename) + ".txt");

        // Ensure the output directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

        // Write the sorted data to the output file
        using (StreamWriter file = new StreamWriter(outputFile))
        {
            foreach (var item in values)
            {
                if (item.amplitude.HasValue && item.emphasis != null)
                {
                    // Write entries with amplitude and emphasis
                    file.WriteLine($"e {item.time} {item.amplitude} {item.emphasis.amplitude} {item.emphasis.frequency}");
                }
                else if (item.amplitude.HasValue)
                {
                    // Write entries with amplitude only
                    file.WriteLine($"a {item.time} {item.amplitude}");
                }
                else if (item.frequency.HasValue)
                {
                    // Write entries with frequency only
                    file.WriteLine($"f {item.time} {item.frequency}");
                }
            }
        }
    }
}