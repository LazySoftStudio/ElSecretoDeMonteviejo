using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;//singleton
    public TextMeshProUGUI textComponet;
    public TextMeshProUGUI nombre;
    public GameObject padre1;
    public GameObject padre2;
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public GameObject option4;
    private List<string> lines;
    public float textSpeed = 0.2f;
    private List<string> nameQHabla;

    public List<Option> opcionesActuales;
    public ConversationTemplate ct;
    private int index;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nameQHabla = new List<string>();
        lines = new List<string>();
        opcionesActuales = new List<Option>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Empezar()
    {
        textComponet.text = string.Empty;
        nombre.text = string.Empty;
        option1.SetActive(false);
        option2.SetActive(false);
        option3.SetActive(false);
        option4.SetActive(false);
        player.GetComponent<PlayerMovement>().lockMovement = true;
        StartDialogue();//cambiar
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponet.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponet.text = lines[index];
            }
        }

    }

    private void StartDialogue()
    {
        index = 0;
        nameQHabla.Clear();
        opcionesActuales.Clear();
        lines.Clear();
        if (ct == null)
        {
            padre1.SetActive(false);
            padre2.SetActive(false);
            player.GetComponent<PlayerMovement>().lockMovement = false;
            ct = null;
            return;
        }
        foreach (Line l in ct.converationLines)
        {
            lines.Add(l.dialogueLine);
            nameQHabla.Add(l.speakerName);
        }
        GetOptions();

        padre1.SetActive(true);
        padre2.SetActive(true);
        StartCoroutine(TypeLIne());
    }

    private void GetOptions()
    {
        opcionesActuales.Clear();
        foreach (Option o in ct.converationLines[index].options)
        {
            opcionesActuales.Add(o);
        }
        if (opcionesActuales.Count == 0)
        {
            return;
        }
        TextMeshProUGUI[] l = option1.gameObject.GetComponent<TMPGetter>().GetTexts();
        if (opcionesActuales.Count == 1)
        {
            option1.SetActive(true);
        }
        if (opcionesActuales.Count == 2)
        {
            option2.SetActive(true);
            l = option2.gameObject.GetComponent<TMPGetter>().GetTexts();
        }
        if (opcionesActuales.Count == 3)
        {
            option3.SetActive(true);
            l = option3.gameObject.GetComponent<TMPGetter>().GetTexts();
        }
        if (opcionesActuales.Count == 4)
        {
            option4.SetActive(true);
            l = option4.gameObject.GetComponent<TMPGetter>().GetTexts();
        }
        //poner textos
        Debug.Log(opcionesActuales.Count + " - "+l.Length);
        for (int i = 0; i < l.Length; i++)
        {
            l[i].text = opcionesActuales[i].optionText;
        }

    }

    IEnumerator TypeLIne()
    {
        nombre.text = nameQHabla[index];
        foreach (char c in lines[index].ToCharArray())
        {
            textComponet.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }

    private void NextLine()
    {
        option1.SetActive(false);
        option2.SetActive(false);
        option3.SetActive(false);
        option4.SetActive(false);
        if (index < lines.Count - 1)
        {
            index++;
            textComponet.text = string.Empty;
            GetOptions();
            StartCoroutine(TypeLIne());
        }
        else
        {
            //nada mas que decir
            padre1.SetActive(false);
            padre2.SetActive(false);
            player.GetComponent<PlayerMovement>().lockMovement = false;
            ct = null;
        }
    }

    public void DecirDialogo(ConversationTemplate con)
    {
        StopAllCoroutines();
        ct = con;
        Empezar();
    }

    public void Opcion1() 
    {
        DecirDialogo(opcionesActuales[0].nextDialogue);
    }
    public void Opcion2()
    {
        DecirDialogo(opcionesActuales[1].nextDialogue);
    }
    public void Opcion3()
    {
        DecirDialogo(opcionesActuales[2].nextDialogue);
    }
    public void Opcion4()
    {
        DecirDialogo(opcionesActuales[3].nextDialogue);
    }


}
