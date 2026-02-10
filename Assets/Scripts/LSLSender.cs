using UnityEngine;
using UnityEngine.InputSystem;
using LSL;
using System.Collections.Generic;

public class LSLSender : MonoBehaviour
{
    InputAction sendDataAction;
    StreamOutlet outlet;

    void Start()
    {
        // Create a marker / message stream
        StreamInfo info = new StreamInfo(
            "UnityCommands",   // stream name
            "Markers",         // stream type
            1,                 // one channel
            0,                 // irregular sampling rate
            channel_format_t.cf_string,
            "unity_commands"
        );

        outlet = new StreamOutlet(info);
        sendDataAction = InputSystem.actions.FindAction("SendData");
    }

    void Update()
    {
        if (sendDataAction.IsPressed())
        {
            string[] sample = new string[] { "message from Unity" };
            outlet.push_sample(sample);
            Debug.Log("Sent message to Python");
        }
    }

    Dictionary<string, StreamOutlet> outlets;

    public void StartStream(string streamName)
    {
        // Create a marker / message stream
        StreamInfo info = new StreamInfo(
            streamName,   // stream name
            "Markers",         // stream type
            1,                 // one channel
            0,                 // irregular sampling rate
            channel_format_t.cf_string,
            "unity_commands"
        );
        outlets[streamName] = new StreamOutlet(info);
    }

    public void SendData(string streamName, float[] data)
    {
        outlets[streamName].push_sample(data);
    }
}
