using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public enum DialogueEventType
{
    None, 
    ChangeScene, 
    GiveItem
}
[System.Serializable]
public struct Option
{
    [TextArea(2, 3)]
    public string optionText;
    public ConversationTemplate nextDialogue;
}

[System.Serializable]
public struct Line
{
    public string speakerName;
    [TextArea(2, 3)]
    public string dialogueLine;
    public DialogueEventType eventType;
    public string optionalParameter;
    public List<Option> options;
    //public List<DyalogueItemReward> itemsToGive; //por hacer
}
[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class ConversationTemplate : ScriptableObject
{
    public List<Line> converationLines;
    
}
