using System.Diagnostics;
using System.IO;
using UnityEngine;

public class LaunchPython : MonoBehaviour
{
    [SerializeField] string pythonFile;
    [SerializeField] bool createWindow;
    private Process pythonProcess;

    string pythonPath;

    void Awake()
    {
        string[] lines = File.ReadAllLines("python_config.toml");
        foreach (var line in lines)
        {
            if (line.StartsWith("executable"))
            {
                pythonPath = line.Split('=')[1].Trim().Trim('"');
            }
        }
        StartPython();
    }

    void StartPython()
    {
        pythonProcess = new Process();
        pythonProcess.StartInfo.FileName = pythonPath;   // or full path to python.exe
        pythonProcess.StartInfo.Arguments = pythonFile;
        pythonProcess.StartInfo.WorkingDirectory = "";
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.StartInfo.CreateNoWindow = !createWindow;
        pythonProcess.StartInfo.RedirectStandardOutput = true;
        pythonProcess.StartInfo.RedirectStandardError = true;

        pythonProcess.OutputDataReceived += (s, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                UnityEngine.Debug.Log("[PY] " + e.Data);
        };

        pythonProcess.ErrorDataReceived += (s, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            if (e.Data.StartsWith("Traceback") ||
                e.Data.Contains("Error") ||
                e.Data.Contains("Exception"))
            {
                UnityEngine.Debug.LogError("[PY] " + e.Data);
            }
            else
            {
                UnityEngine.Debug.Log("[PY] " + e.Data);
            }
        };

        pythonProcess.Start();
        pythonProcess.BeginOutputReadLine();
        pythonProcess.BeginErrorReadLine();
    }

    void OnApplicationQuit()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
            pythonProcess.Kill();
    }
}