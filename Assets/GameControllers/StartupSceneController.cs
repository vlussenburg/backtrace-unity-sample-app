using Backtrace.Unity;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
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
    private void Application_lowMemory()
    {
        if (_handled)
        {
            Debug.LogWarning("letting it crash");
            _doOom = true;
            return;
        }

        _doOom = false;
        _handled = true;
        _textures.Clear();
        Resources.UnloadUnusedAssets();
        _client.Send("Ok, we received a memory warning");
    }

    public void Start()
    {
        _client = BacktraceClient.Instance;
        //_client.Send("Startup", attributes: new Dictionary<string, string> { { "testtest", Time.time.ToString() } });
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
            //_client.Metrics.AddSummedEvent("handle-execption", null);
            Debug.LogError("Throwing a handled exception object");
            InternalFileReader();
        }
        catch (Exception e)
        {
            _client.Send(e, attributes: attributes);
        }
        _client.Send("foo-bar");
        //_client.Session.Send();
    }

    public void UnhandledException()
    {
        _client[LastAction] = "UNHANDLED exception";
        Debug.LogWarning("Throwing an unhandled exception ✅🔮⛱😂object!^㊛ﺅɏ耀借俠⃐㫪ၩ�\b〫鴰䌯尺;C:\\Program Files (x86)\\Wi");

        string backtraceCrashHelperPath = string.Format("{0}.{1}", "backtrace.io.backtrace_unity_android_plugin", "BacktraceCrashHelper");
        var backtraceCrashHelper = new AndroidJavaObject(backtraceCrashHelperPath);
        //throwBackgroundJvmException
        backtraceCrashHelper.Call("triggerAnr");
        backtraceCrashHelper.Call("throwBackgroundJvmException");
        backtraceCrashHelper.Call("throwJvmException");

        InternalFileReader();
    }

    private const string BacktraceGameObjectName = "Backtrace";
    public unsafe void Crash()
    {
        _client[LastAction] = "Crash";
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        
        var size = UnsafeUtility.SizeOf<int>();
        var alignment = UnsafeUtility.AlignOf<int>();
        var ptr = UnsafeUtility.Malloc(size, alignment, Allocator.Persistent);
        UnsafeUtility.Free(ptr, Allocator.Persistent);
        /* Invalid Operation */
        UnsafeUtility.Free(ptr, Allocator.Persistent);
#endif
        //UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
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
        Debug.LogWarning("Unsupported in Unity editor");
        Test(count);

#endif

    }

    public int count = 200;

    void Test(int count)
    {
        if (--count >= 0)
        {
            Test(count);
        }
        else
        {
            Debug.LogError("Error message");
        }
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
        Debug.LogWarning("Starting ANR in managed Unity thread.");
        FreezeMainThread();
#else
        Debug.LogWarning("Anr is unsupported");
#endif
    }

    public void NextScene()
    {
        _client[LastAction] = "NEXT SCENE";
        SceneManager.LoadScene(1);
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
        _client[LastAction] = "Exit";
        //Debug.LogException(new Exception("quit me"));
        Application.Quit();

        //if (BacktraceClient.Instance != null && BacktraceClient.Instance.Enabled)
        //{
        //    Destroy(BacktraceDatabase.Instance.gameObject);
        //    Destroy(BacktraceClient.Instance.gameObject);
        //}
        //else
        //{
        //    Application.Quit();
        //}
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
