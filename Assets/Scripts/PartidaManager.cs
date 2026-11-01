using HeyHeyThere.SceneTransitionsFree;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartidaManager : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button delete;
    [SerializeField] private Button partida1;
    [SerializeField] private Button partida2;
    [SerializeField] private TextMeshProUGUI date1;
    [SerializeField] private TextMeshProUGUI date2;
    private int partidaSeleccionada = -1;

    private void Awake()
    {
        // no hay seleccionado, no se puede interactuar
        play.interactable = false;
        delete.interactable = false;
        
    }
    private void Start()
    {
        RellenarTextos();
    }

    private void Update()
    {
        if (partidaSeleccionada == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            partida1.Select();
        }
        else if(partidaSeleccionada == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
            partida2.Select();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            play.interactable = false;
            delete.interactable = false;
        }
    }

    public void MarcarPartida(int id)
    {
        partidaSeleccionada = id;
        play.interactable = true;
        delete.interactable = true;

    }

    void RellenarTextos()
    {
        string date;
        if (!SavePartida.Singleton.ExisteGuardado("Partida0"))
        {
            date = "--/--/---";
        }
        else
        {
            date = SavePartida.Singleton.Cargar("Partida0").ultimoGuardado;
            DateTime fecha = DateTime.Parse(date, null, DateTimeStyles.RoundtripKind);
            date = fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        date1.text = date;

        if (!SavePartida.Singleton.ExisteGuardado("Partida1"))
        {
            date = "--/--/---";
        }
        else
        {
            date = SavePartida.Singleton.Cargar("Partida1").ultimoGuardado;
            if (date != null) {
                DateTime fecha = DateTime.Parse(date, null, DateTimeStyles.RoundtripKind);
                date = fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                date = "--/--/---";
            }
        }

        date2.text = date;
    }

    public void JugarSeleccionado()
    {
        if (partidaSeleccionada == -1) return;
        int scene;
        //generacion de llave
        string key = "Partida";
        key += partidaSeleccionada.ToString();
        
        if (!SavePartida.Singleton.ExisteGuardado(key))
        {
            SavePartida.Singleton.SetKey(key);
            SavePartida.Singleton.Guardar(new DatosPartida());
            scene = 1;
        }
        else
        {
            scene = SavePartida.Singleton.Cargar(key).sceneNumber;
        }
            
        SceneTransition.ChangeScene(scene, "diamonds");
    }

    public void EliminarSeleccionado()
    {
        //añade el por defecto
        SavePartida.Singleton.Borrar("Partida" + partidaSeleccionada.ToString());

        partidaSeleccionada = -1;
        RellenarTextos();
    }
}
