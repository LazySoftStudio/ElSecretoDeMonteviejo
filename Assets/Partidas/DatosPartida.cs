using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DatosPartida
{
    public Vector3 posicion = new Vector3(-1906.63f, -2f, 112.61f);//harcodeado posicion inicial
    //public Quaternion rotacion = new Quaternion(-0.6725f, 0.2185f, 0.2185f, -0.6725f);
    public Quaternion rotacion = Quaternion.Euler(0f, 216f, 90f);
    //inventario
    public Dictionary<string, ItemSO> inventario = new Dictionary<string, ItemSO>();
    public int sceneNumber = 1;//pueblo

    //si se ha entrado en alguna casa
    public Vector3 posicionCasa;
    public Quaternion rotacionCasa;
    //fecha
    public string ultimoGuardado;
}