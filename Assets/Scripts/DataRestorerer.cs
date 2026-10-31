using System.Collections.Generic;
using UnityEngine;

public class DataRestorerer : MonoBehaviour
{
    public static DataRestorerer Singleton;

    private Vector3 lastPos;
    private Quaternion lastRotation;
    private const int ESCENA_PUEBLO = 1;

    private Vector3 initialPos = new Vector3(-1894.44f, -2.579f, 99.81f);
    private Quaternion initialRotation = new Quaternion(0f, 0.4f, 0f, 0.9f);
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            lastPos = initialPos;
            lastRotation = initialRotation;
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SetData(Vector3 pos, Quaternion rot, int newScene)
    {
        if (newScene == ESCENA_PUEBLO) return;
        lastPos = pos;
        lastRotation = rot;
        Debug.Log("Guardado data, escena - " + newScene);
    }
    public Vector3 GetLastPos()
    {
        return lastPos;
    }
    public Quaternion GetLastRotation()
    {
        return lastRotation;
    }
}
