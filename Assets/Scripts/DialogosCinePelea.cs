using System.Collections;
using TMPro;
using UnityEngine;

public class DialogosCinePelea : MonoBehaviour
{
    [SerializeField] private CinePelea cine;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text textoQuien;
    [SerializeField] private TMP_Text textoLinea;
    [SerializeField] private float segundosPorLetra = 0.03f;

    private CinePelea.PuntoDialogo punto;
    private int indice;
    private bool escribiendo;
    private Coroutine rutina;

    void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    void OnEnable()
    {
        if (cine != null) cine.DialogoEmpieza += AlEmpezar;
    }

    void OnDisable()
    {
        if (cine != null) cine.DialogoEmpieza -= AlEmpezar;
    }

    void AlEmpezar(CinePelea.PuntoDialogo p)
    {
        punto = p;
        indice = 0;
        panel.SetActive(true);
        MostrarLinea();
    }

    void MostrarLinea()
    {
        var linea = punto.lineas[indice];
        textoQuien.text = linea.quien;
        if (rutina != null) StopCoroutine(rutina);
        rutina = StartCoroutine(Escribir(linea.texto));
    }

    IEnumerator Escribir(string texto)
    {
        escribiendo = true;
        textoLinea.text = "";
        foreach (char c in texto)
        {
            textoLinea.text += c;
            yield return new WaitForSecondsRealtime(segundosPorLetra);
        }
        escribiendo = false;
    }

    void Update()
    {
        if (punto == null) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (escribiendo)
            {
                // completa la línea al instante
                StopCoroutine(rutina);
                textoLinea.text = punto.lineas[indice].texto;
                escribiendo = false;
            }
            else
            {
                indice++;
                if (indice < punto.lineas.Count) MostrarLinea();
                else Terminar();
            }
        }
    }

    void Terminar()
    {
        punto = null;
        panel.SetActive(false);
        cine.ContinuarTrasDialogo();
    }
}