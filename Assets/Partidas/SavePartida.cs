using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class SavePartida : MonoBehaviour
{
    private const string CLAVE = "Partida1";

    public static SavePartida Singleton;
    private DatosPartida actual;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Guardar(DatosPartida datos)
    {
        datos.ultimoGuardado = DateTime.Now.ToString("o");
        string json = JsonUtility.ToJson(datos);
        PlayerPrefs.SetString(CLAVE, json);
        PlayerPrefs.Save(); // fuerza la escritura
    }

    public DatosPartida Cargar()
    {
        if (!PlayerPrefs.HasKey(CLAVE)) return null;

        string json = PlayerPrefs.GetString(CLAVE);
        actual = JsonUtility.FromJson<DatosPartida>(json);
        return actual;
    }
    public void Guardar(Vector3 pos, Quaternion rot, int scene)
    {
        actual.ultimoGuardado = DateTime.Now.ToString("o");
        Debug.Log(actual.ultimoGuardado);
        actual.sceneNumber = scene;
        if (scene == 1)//voy al pueblo
        {
            actual.posicionCasa = pos;
            actual.rotacionCasa = rot;
        }
        else//voy a una casa
        {
            actual.posicion = pos;
            actual.rotacion = rot;
        }
        string json = JsonUtility.ToJson(actual);
        PlayerPrefs.SetString(CLAVE, json);
        PlayerPrefs.Save();
    }

    public bool ExisteGuardado() => PlayerPrefs.HasKey(CLAVE);

    public void Borrar()
    {
        PlayerPrefs.DeleteKey(CLAVE);
        PlayerPrefs.Save();
    }
}
