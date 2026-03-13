using UnityEngine;
using TMPro;
public class HUDcontroller : MonoBehaviour
{
    public static HUDcontroller instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_Text interactionText;

    public void enableInteractionText(string text)
    {
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }
    public void disableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

}
