using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine.UI;


public class LoadImage : MonoBehaviour
{
    // [SerializeField] InputField input;
    [SerializeField] GameObject image;

    public Process process;
    public StreamWriter streamWriter;
    public Material material;
    public Sprite SpriteMain;

    private Thread thread;

    public bool bInitialized = false;

    private List<string> liLines = new List<string>();
    private List<string> liErrors = new List<string>();

    public Sprite Ergebnis;
    public Texture ErgebnisInpaint;


    private OutputStatus outputStatus = OutputStatus.Unfinished;

    private enum OutputStatus { Unfinished, Broken, BrokenNeedsRestart, FinishedSuccessfully }

    public bool loaded = false;

    public void Start()
    {
        //StartCoroutine(loadImage());
        loaded = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(loadImage());
        }
    }


    private void ProcessOutput(string _strOutput)
    {
        UnityEngine.Debug.Log(">>>>>>>> " + _strOutput);

        if (_strOutput.StartsWith("* Initialization done!"))
        {
            bInitialized = true;

        }

        else if (outputStatus == OutputStatus.Unfinished && _strOutput.StartsWith("Outputs:"))
            outputStatus = OutputStatus.FinishedSuccessfully;
        else if (_strOutput.StartsWith(">> Could not generate image."))
            outputStatus = OutputStatus.Broken;
        else if (_strOutput.StartsWith("dream> CUDA out of memory"))
            outputStatus = OutputStatus.BrokenNeedsRestart;
    }


    public IEnumerator loadImage()
    {
        FileInfo fileLatestPng = new DirectoryInfo("C:/Projekte/ComfyUI_windows_portable/ComfyUI/output").GetFiles().Where(x => Path.GetExtension(x.Name) == ".jpg").OrderByDescending(f => f.LastWriteTime).First();
        //FileInfo fileLatestPng = new DirectoryInfo("D:/Projects/AAGEPlus/ComfyUI_windows_portable/ComfyUI/output").GetFiles().Where(x => Path.GetExtension(x.Name) == ".png").OrderByDescending(f => f.LastWriteTime).First();

        UnityEngine.Debug.Log($" New file appeared! Loading {fileLatestPng.Name}");

        yield return new WaitUntil(() => !Utility.IsFileLocked(fileLatestPng));
        yield return new WaitForSeconds(0.1f);

        UnityEngine.Debug.Log($"Finished loading image.");

        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = Utility.texLoadImageSecure(fileLatestPng.FullName, mat.mainTexture as Texture2D);
        SpriteMain = Sprite.Create(mat.mainTexture as Texture2D, new Rect(0.0f, 0.0f, mat.mainTexture.width, mat.mainTexture.height), new Vector2(0.5f, 0.5f), 100.0f);


        Ergebnis = SpriteMain;
        ErgebnisInpaint = mat.mainTexture;
        image.GetComponent<Image>().sprite = Ergebnis;

        loaded = true;
    }

}
