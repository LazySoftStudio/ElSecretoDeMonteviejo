
public interface IInteractive
{
    void Interact();//metodo para interactuar
    void OnHover();//si se mantiene
    void OnExit();// al salir

    ConversationTemplate GetDialogue();
}
