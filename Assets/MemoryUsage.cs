using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class MemoryUsage : MonoBehaviour
{
    public GameObject MemoryUsageObject;
    private System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Updating");
        process = System.Diagnostics.Process.GetCurrentProcess();
        //$"memsize64: {process.PrivateMemorySize64} working set: {process.WorkingSet64} "
        MemoryUsageObject.GetComponent<Text>().text = ((Profiler.usedHeapSizeLong / 1024f) / 1024f).ToString("#,##0 MB");

    }
}
