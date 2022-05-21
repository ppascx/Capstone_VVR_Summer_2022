using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
// using UnityEditor.XR.Management; this is to test XR Mode in Editor


/* This was referenced from Unity's XR Management resource
https://docs.unity3d.com/Packages/com.unity.xr.management@4.0/manual/EndUser.html
*/
public class RuntimeXRLoaderManager : MonoBehaviour
{
    void Start()
    {
        //We always need to Stop and Deinitialize Loader
        StopXR();
        StartCoroutine(StartXR());
    }

    public IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    public void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }
}
 
