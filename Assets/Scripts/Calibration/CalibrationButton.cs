using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LSL;

public class CalibrationButton : MonoBehaviour
{
    [SerializeField] CalibrationManager calibrationManager;
    [SerializeField] string calibrationName;
    [SerializeField] int nSecondsBeforeCalibration;
    [SerializeField] int nSecondsCalibration;
    [SerializeField] TMP_Text infoText;

    // lsl data stream tied to existence of StreamOutlet
    StreamOutlet outlet;

    void Awake()
    {
        StreamInfo info = new StreamInfo(
            calibrationName,   // stream name
            "Markers",         // stream type
            1,                 // one channel
            0,                 // irregular sampling rate
            channel_format_t.cf_int32,
            "UnityControl"
        );
        outlet = new StreamOutlet(info);
    }

    public void StartCalibration()
    {
        // check if other calibration is going on
        if (calibrationManager.RequestCalibration(calibrationName))
        {
            StartCoroutine("Calibration");
        }
    }

    IEnumerator Calibration()
    {
        for(int i = nSecondsBeforeCalibration; i > 0; i--)
        {
            infoText.text = "Starting " + calibrationName.ToLower() + " calibration in " + i.ToString();
            yield return new WaitForSeconds(1);
        }

        // tells python script to start looking for EEG activity related to calibrating "calibrationName"
        int[] startMsg = new int[] { 1 };
        outlet.push_sample(startMsg);
        
        for(int i = nSecondsCalibration; i > 0; i--)
        {
            infoText.text = "Recording " + calibrationName.ToLower() + " ...\n Time left: " + i.ToString();
            yield return new WaitForSeconds(1);
        }

        int[] stopMsg = new int[] { 0 };
        outlet.push_sample(stopMsg);

        infoText.text = calibrationName + " sample acquired !";
        calibrationManager.FinishedCalibration();
    }
}
