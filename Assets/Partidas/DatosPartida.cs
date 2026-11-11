using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DatosPartida
{
    public Vector3 posicion = new Vector3(-1877.35f, -2.489f, 69.33f);//harcodeado posicion inicial
    //public Quaternion rotacion = new Quaternion(-0.6725f, 0.2185f, 0.2185f, -0.6725f);
    public Quaternion rotacion = Quaternion.Euler(0f, 247f, 90f);
    //inventario
    public List<string> inventario = new List<string>();
    public int sceneNumber = 1;//pueblo

    //si se ha entrado en alguna casa
    public Vector3 posicionCasa;
    public Quaternion rotacionCasa;
    //fecha
    public string ultimoGuardado;
    //animacion inicial
    public bool animacionInicial = false;
    public float horaDia = 10;
}