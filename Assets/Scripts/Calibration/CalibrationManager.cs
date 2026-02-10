using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationManager : MonoBehaviour
{
    [SerializeField] List<Button> buttons;
    private string calibrationRunning = "";

    public bool RequestCalibration(string calibrationName)
    {
        var canRunCalibration = calibrationRunning == "";
        if (canRunCalibration)
        {
            calibrationRunning = calibrationName;
            buttons.ForEach(button => button.interactable = false);
            // TODO: deactivate other buttons in the scene
            return true;
        }
        return false;
    }

    public void FinishedCalibration()
    {
        calibrationRunning = "";
        buttons.ForEach(button => button.interactable = true);
    }
}
