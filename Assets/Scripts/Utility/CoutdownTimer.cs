using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private Action timer_event;
    private float startTime;
    private float time;
    private bool loop;

    public void StartTimer(float time, Action timer_event, bool loop = false)
    {
        this.timer_event = timer_event;
        this.startTime = time;
        this.time = time;
        this.loop = loop;
    }

    private void Update()
    {

        if (time > 0f)
        {
            time -= Time.deltaTime;

            if (IsTimerComplete())
            {
                if (loop)
                    time = startTime;
                timer_event();
            }

        }
    }

    bool IsTimerComplete()
    {
        return time < 0f;
    }
}
