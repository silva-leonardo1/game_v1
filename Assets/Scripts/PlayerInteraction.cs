using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    public LayerMask layerToIgnore; // No Inspector, selecione a layer 'whatisplayer' aqui
    private Interactable currentInteractable;

    void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        // O '~' inverte a máscara, fazendo o raio ignorar a layer selecionada
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach, ~layerToIgnore))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                // Se mudamos de um interactable para outro, desativa o outline do anterior
                if (currentInteractable != null && newInteractable != currentInteractable)
                {
                    currentInteractable.disableOutline();
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
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