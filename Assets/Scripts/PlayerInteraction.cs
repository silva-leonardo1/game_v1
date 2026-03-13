using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interactable currentInteractable;
    void Update()
    {
        checkInteraction();
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.interact();
        }

        void checkInteraction()
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out hit, playerReach))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable newInteractable = hit.collider.GetComponent<Interactable>();
                    if (currentInteractable && newInteractable != currentInteractable)
                    {
                        currentInteractable.disableOutline();
                    }
                    if (newInteractable.enabled)
                    {
                        setNewCurrentInteractable(newInteractable);
                    }
                    else
                    {
                        disableCurrentInteractable();
                    }
                }
                else
                {
                    disableCurrentInteractable();
                }
            }
            else
            {
                disableCurrentInteractable();
            }
        }

        void setNewCurrentInteractable(Interactable newInteractable)
        {
                currentInteractable = newInteractable;
                currentInteractable.enableOutline();
                HUDcontroller.instance.enableInteractionText(currentInteractable.message);
        }
    }

    void disableCurrentInteractable()
    {
        HUDcontroller.instance.disableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.disableOutline();
            currentInteractable = null;
        }
    }
}