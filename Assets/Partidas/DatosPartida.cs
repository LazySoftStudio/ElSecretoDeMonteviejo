using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DatosPartida
{
    public Vector3 posicion = new Vector3(-1894.44f, -2.579f, 99.81f);//harcodeado posicion inicial
    public Quaternion rotacion = new Quaternion(0f, 0.4f, 0f, 0.9f);
    //inventario
    public int sceneNumber = 1;//pueblo

    //si se ha entrado en alguna casa
    public Vector3 posicionCasa;
    public Quaternion rotacionCasa;
    //fecha
    public string ultimoGuardado = string.Empty;
}