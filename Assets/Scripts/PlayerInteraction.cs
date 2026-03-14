using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    public LayerMask layerToIgnore; 
    private Interactable currentInteractable;

    void Update()
    {
        CheckInteraction();

        // Interação ao apertar E
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        // Importante: Usamos o transform da câmera (onde o script deve estar)
        Ray ray = new Ray(transform.position, transform.forward);

        // O '~' inverte a máscara para ignorar o que você selecionou
        if (Physics.Raycast(ray, out hit, playerReach, ~layerToIgnore))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable != null && newInteractable.enabled)
                {
                    if (newInteractable != currentInteractable)
                    {
                        SetNewCurrentInteractable(newInteractable);
                    }
                    return; // Retorna aqui para manter o texto ativo
                }
            }
        }

        // Só desativa se o raio NÃO encontrou um Interactable válido
        if (currentInteractable != null)
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.enableOutline();
        
        if (HUDcontroller.instance != null)
        {
            HUDcontroller.instance.enableInteractionText(currentInteractable.message);
        }
    }

    void DisableCurrentInteractable()
    {
        if (HUDcontroller.instance != null)
        {
            HUDcontroller.instance.disableInteractionText();
        }

        if (currentInteractable != null)
        {
            currentInteractable.disableOutline();
            currentInteractable = null;
        }
    }
}