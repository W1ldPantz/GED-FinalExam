using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class test : MonoBehaviour
{
    
    [DllImport("MyTestingDLL")] public static extern void MyDemo(float time);
    [DllImport("MyTestingDLL")] public static extern float Mutate();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        MyDemo(transform.position.z);
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            transform.position += Vector3.forward * Mutate();
        }
    }
}
