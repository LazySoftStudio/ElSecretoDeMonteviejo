using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class SavePartida : MonoBehaviour
{
    private  string llave;

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
        PlayerPrefs.SetString(llave, json);
        PlayerPrefs.Save(); // fuerza la escritura
    }

    public DatosPartida Cargar(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return null;
        llave = key;// actualiza la llave
        string json = PlayerPrefs.GetString(key);
        actual = JsonUtility.FromJson<DatosPartida>(json);
        Inventory.Singleton.Restore(actual.inventario);
        return actual;
    }
    public void Guardar(Vector3 pos, Quaternion rot, int scene)
    {
        actual.ultimoGuardado = DateTime.Now.ToString("o");
        actual.inventario = Inventory.Singleton.GetInventory();
        CicloDiaNoche ciclo = GameObject.FindWithTag("DiaNoche").GetComponent<CicloDiaNoche>();
        if (ciclo) actual.horaDia = ciclo.Hora;
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
        PlayerPrefs.SetString(llave, json);
        PlayerPrefs.Save();
    }

    public void QuickSave(Vector3 pos, Quaternion rot, int scene)
    {
        actual.ultimoGuardado = DateTime.Now.ToString("o");
        actual.inventario = Inventory.Singleton.GetInventory();
        CicloDiaNoche ciclo = GameObject.FindWithTag("DiaNoche").GetComponent<CicloDiaNoche>();
        if (ciclo) actual.horaDia = ciclo.Hora;
        actual.sceneNumber = scene;
        if (scene != 1)//voy al pueblo
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
        PlayerPrefs.SetString(llave, json);
        PlayerPrefs.Save();
    }

    public void AnimacionInicialHecha()
    {
        actual.animacionInicial = true;
        string json = JsonUtility.ToJson(actual);
        PlayerPrefs.SetString(llave, json);
        PlayerPrefs.Save();
    }
    public bool ExisteGuardado(string key) => PlayerPrefs.HasKey(key);

    public void Borrar(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }
    public void SetKey(string key)
    {
        llave = key;
    }
    public string GetKey()
    {
        return llave;
    }
}
