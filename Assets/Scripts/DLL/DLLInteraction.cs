using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DLL
{
    public class DLLInteraction : MonoBehaviour
    {
        [DllImport("GameStatsDLL")] private static extern void CaptureScreenshot();
        [DllImport("GameStatsDLL")] private static extern void InitializeGDIPlus();
        [DllImport("GameStatsDLL")] private static extern void ShutdownGDIPlus();

        private IEnumerator Start()
        {
            while (Application.isPlaying)
            {
                yield return new WaitForSeconds(3);
                CaptureScreenshot();
                Debug.Log("CaptureScreenshot called");
            }

            
        }

        private void OnEnable()
        {
            InitializeGDIPlus();
        }

        private void OnDisable()
        {
            ShutdownGDIPlus();
        }
    }
}
