using System.Collections.Generic;
using UnityEngine;

public class PersonaFiinal : MonoBehaviour, IInteractive
{
    [SerializeField] private string[] itemsToCheck;   
    [SerializeField] private ConversationTemplate conversacionInical;
    [SerializeField] private ConversationTemplate acierto;
    [SerializeField] private ConversationTemplate error;

    private ConversationTemplate runtime;     
    private List<Line> lineasIniciales;        

    private void Awake()
    {
        lineasIniciales = new List<Line>(conversacionInical.converationLines);
        runtime = Instantiate(conversacionInical);
    }

    private void OnDestroy()
    {
        if (runtime != null) Destroy(runtime);
    }

    public ConversationTemplate GetDialogue()
    {
        bool todoBien = true;
        foreach (string item in itemsToCheck)
        {
            if (!Inventory.Singleton.HasItem(item)) { todoBien = false; break; }
        }

        var lineas = runtime.converationLines;
        lineas.Clear();
        lineas.AddRange(lineasIniciales);
        lineas.AddRange(todoBien ? acierto.converationLines : error.converationLines);
        return runtime;
    }

    public void Interact() { }
    public void OnExit() { }
    public void OnHover() { }
}