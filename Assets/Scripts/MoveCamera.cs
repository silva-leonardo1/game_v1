using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Referência ao transform que representa a posição desejada da câmera
    public Transform cameraPosition;

    // Update é chamado uma vez a cada frame
    private void Update()
    {
        // Move a posição deste objeto (normalmente a câmera) para a posição do 'cameraPosition'
        transform.position = cameraPosition.position;
    }
}