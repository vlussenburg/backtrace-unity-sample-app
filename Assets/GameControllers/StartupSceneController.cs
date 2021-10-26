using Backtrace.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;

public class StartupSceneController : MonoBehaviour
{
    [DllImport("backtrace-native", EntryPoint = "Crash")]
    public static extern void NativeCrash();

    private const string LastAction = "action.last";

    private BacktraceClient _client;

    bool _handled = false;

    public void Start()
    {
        _client = BacktraceClient.Instance;
    }
    public void HandledException()
    {
        _client[LastAction] = "HandleException";
        var attributes = new Dictionary<string, string>()
                {
                    { "Controller" , "StartupScene"},
                    { "Time", Time.time.ToString() },
                };

        _client.Breadcrumbs.Info("info");
        _client.Breadcrumbs.Warning("warning");
        _client.Breadcrumbs.Debug("debug");
        _client.Breadcrumbs.Exception("exception");


        _client.Breadcrumbs.Info("info", attributes);
        _client.Breadcrumbs.Warning("warning", attributes);
        _client.Breadcrumbs.Debug("debug", attributes);
        _client.Breadcrumbs.Exception("exception", attributes);
        try
        {
            _client.Metrics.AddSummedEvent("handle-execption", null);
            Debug.LogError("Throwing a handled exception object");
            InternalFileReader();
        }
        catch (Exception e)
        {
            _client.Send(e, attributes: attributes);
        }
        _client.Send("foo-bar");
        _client.Metrics.Send();
    }

    public void UnhandledException()
    {
        _client[LastAction] = "UNHANDLED exception";
        Debug.LogWarning("Throwing an unhandled exception. Specific character test: ✅🔮⛱😂object!^㊛ﺅɏ耀借俠⃐㫪ၩ�\b〫鴰䌯尺;");

#if UNITY_ANDROID
        string backtraceCrashHelperPath = string.Format("{0}.{1}", "backtrace.io.backtrace_unity_android_plugin", "BacktraceCrashHelper");
        var backtraceCrashHelper = new AndroidJavaObject(backtraceCrashHelperPath);
        backtraceCrashHelper.Call("throwBackgroundJavaException");
        backtraceCrashHelper.Call("throwRuntimeException");
        backtraceCrashHelper.CallStatic("StartAnr");
#endif
        InternalFileReader();
    }

    public unsafe void Crash()
    {
        _client[LastAction] = "Crash";
#if UNITY_EDITOR
        return;
#endif
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
    }

    public void Oom()
    {
        _client[LastAction] = "OOM";
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        Debug.LogWarning("Starting OOM");
        StartCoroutine(StartOom());
#elif !UNITY_EDITOR
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.AccessViolation);
#else
        Debug.LogWarning("Unsupported in the Unity editor");
#endif
    }

    private bool _doOom = true;
    private List<Texture2D> _textures = new List<Texture2D>();
    private IEnumerator StartOom()
    {
        while (_doOom)
        {
            var texture = new Texture2D(512, 512, TextureFormat.ARGB32, true);
            texture.Apply();
            _textures.Add(texture);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    public void StartAnr()
    {
        _client[LastAction] = "ANR";
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN
        Debug.LogWarning("Starting ANR in the managed Unity thread.");
        FreezeMainThread();
#else
        Debug.LogWarning("Anr is unsupported");
#endif
    }

    public void NextScene()
    {
        _client[LastAction] = "NEXT SCENE";
        SceneManager.LoadScene(5);
    }

    public void Pause()
    {
        _client[LastAction] = "Pause";
        _client["pause.time"] = Time.realtimeSinceStartup.ToString();
        if (Time.timeScale == 0)
        {
            Debug.Log("Unpausing the game");
        }
        else
        {
            Debug.LogWarning("pausing the game");
        }
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void FreezeMainThread()
    {
        const int anrTime = 11000;
        System.Threading.Thread.Sleep(anrTime);
    }

    /// <summary>
    /// Use this function to extend stack trace information
    /// </summary>
    private void InternalFileReader()
    {
        ReadFile();
    }
    private void ReadFile()
    {
        System.IO.File.ReadAllBytes("path/to/not/existing/file");
    }

}
