using UnityEngine;
using LSL;

public class EEGReceiver : MonoBehaviour
{
    private StreamInlet inlet;
    private float[,] chunkBuffer;
    private double[] timestamps;
    private int channelCount;
    private int maxChunkSize = 512; // adjust as needed

    void Start()
    {
        // Resolve by stream name
        var results = LSL.LSL.resolve_stream("name", "MNE-LSL-Player", 1, 5.0f);
        if (results.Length == 0)
        {
            Debug.LogError("No LSL stream named 'MNE-LSL-PI' found.");
            return;
        }

        inlet = new StreamInlet(results[0]);
        channelCount = inlet.info().channel_count();

        // Allocate fixed-size buffers (safe)
        chunkBuffer = new float[channelCount, maxChunkSize];
        timestamps = new double[maxChunkSize];

        Debug.Log($"Connected to 'MNE-LSL-PI' with {channelCount} channels.");
    }

    void Update()
    {
        if (inlet == null) return;

        // Pull a chunk safely
        int samplesRead = inlet.pull_chunk(chunkBuffer, timestamps, maxChunkSize);

        for (int i = 0; i < samplesRead; i++)
        {
            // Access the first channel as example
            float val0 = chunkBuffer[0, i];
            double ts = timestamps[i];

            // Process your EEG data here
            Debug.Log(val0);
        }
    }

    void OnDestroy()
    {
        inlet?.close_stream();
    }
}
