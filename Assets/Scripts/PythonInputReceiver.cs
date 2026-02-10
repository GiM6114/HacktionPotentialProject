using UnityEngine;
using LSL;
using System;
using System.Collections.Generic;
using System.Collections;

public class LSLInputListener : MonoBehaviour
{
    private StreamInlet inlet;
    private int[] sample;
    private double timestamp;

    void Start()
    {
        StartCoroutine("LookForStreams");
    }

    IEnumerator LookForStreams()
    {
        // wait for the python subprocess to be launched
        yield return new WaitForSeconds(1);

        // Resolve LSL streams of type "Markers"
        Debug.Log("Looking for LSL stream...");
        StreamInfo[] results = LSL.LSL.resolve_stream("name", "InputStream", 1, 5.0);

        if (results.Length == 0)
        {
            Debug.LogError("No LSL stream found.");
            yield break;
        }

        inlet = new StreamInlet(results[0]);
        sample = new int[1];

        Debug.Log("LSL stream connected.");
    }

    void Update()
    {
        if (inlet == null)
            return;

        // Non-blocking pull
        timestamp = inlet.pull_sample(sample, 0.0f);

        if (timestamp != 0.0)
        {
            int value = sample[0];
            Debug.Log("Received LSL message: " + value);

            if (value == 0)
            {
                OnLeft();
            }
            else if (value == 1)
            {
                OnRight();
            }
        }
    }

    private void OnLeft()
    {
        Debug.Log("Left action triggered");
    }

    private void OnRight()
    {
        Debug.Log("Right action triggered");
    }

    void OnDestroy()
    {
        if (inlet != null)
        {
            inlet.close_stream();
            inlet = null;
        }
    }
}
