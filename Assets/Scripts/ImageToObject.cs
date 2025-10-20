using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Threading;

public class ImageToObject : MonoBehaviour
{
    public Process process;
    public StreamWriter streamWriter;
    private Thread thread;

    private List<string> liLines = new List<string>();
    private List<string> liErrors = new List<string>();


    public void Start()
    {
        process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };


        process.Start();
        process.BeginOutputReadLine();

        streamWriter = process.StandardInput;
        if (streamWriter.BaseStream.CanWrite)
        {
            //RunCI();
        }
        Run();
    }

    public void Run()
    {
        streamWriter.WriteLine($"D:");
        streamWriter.WriteLine($"cd D:/Projects/VisualContentGenerationAR/vcgAR/Python");
        //streamWriter.WriteLine($"cd C:/Projekte/AIArtExtendedPlus/Python");
        
        streamWriter.WriteLine($"python segmentation_workflow.py --x=1260.0 --y=600.0");
        UnityEngine.Debug.Log("Writing: " + $"queueing img2obj");
        
            
    }
}