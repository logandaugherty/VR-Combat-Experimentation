using System.IO;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticClipPlayer : MonoBehaviour
{

    [SerializeField]private string m_fileName;
    [SerializeField]private HapticImpulsePlayer m_player;

    private CountdownTimer countdownTimer;
    private StreamReader reader;
    private float m_activeTime;

    private char m_activeType;
    private float m_value;
    private float m_active_amp;
    private float m_active_freq;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        countdownTimer = GetComponent<CountdownTimer>();
        if (File.Exists(m_fileName))
        {
            reader = new StreamReader(m_fileName);
            PrepareNextStep();
        }
        else
        {
            Debug.LogError("File does not exist: " + m_fileName);
        }
    }

    private void Update()
    {
        m_activeTime += Time.deltaTime;
    }

    void TriggerChange()
    {
        switch (m_activeType)
        {
            case 'a':
                m_active_amp = m_value;
                break;
            case 'f':
                m_active_freq = m_value;
                break;
            case 'e':
                break;
        }

        m_player.SendHapticImpulse(m_active_amp, 1, m_active_freq);
        PrepareNextStep();
    }

    void PrepareNextStep()
    {
        string line = ReadNextLine(reader);
        if (line == null)
        {
            Debug.Log("Line does not exist!");
            return;
        }

        string[] components = line.Split();
        
        m_activeType = line[0];

        float time_left;

        if (float.TryParse(components[1], out time_left))
        {
        }
        else
        {
            Console.WriteLine("Conversion failed.");
        }

        if (float.TryParse(components[2], out m_value))
        {
        }
        else
        {
            Console.WriteLine("Conversion failed.");
        }

        time_left -= m_activeTime;

        if (time_left <= 0)
        {
            TriggerChange();
            return;
        }

        countdownTimer.StartTimer(time_left, TriggerChange);
    }

    static string ReadNextLine(StreamReader reader)
    {
        try
        {
            return reader.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            return null;
        }
    }
}
