using UnityEngine;
using DG.Tweening; // Importa a biblioteca DOTween para animações suaves, usada para alterar o FOV e a rotação da câmera.

public class PlayerCam : MonoBehaviour
{
    // Sensibilidade do mouse para os eixos X (horizontal) e Y (vertical).
    [Header("Sensi")]
    public float sensX;
    public float sensY;

    // Referências de Transform para a orientação do jogador e o objeto que segura a câmera.
    [Header("References")]
    public Transform orientation; // Define a direção geral do jogador (por exemplo, para alinhar o movimento).
    public float initialFov;

    // Variáveis para armazenar as rotações acumuladas em cada eixo.
    private float xRotation; // Acumula a rotação vertical (pitch) da câmera.
    private float yRotation; // Acumula a rotação horizontal (yaw) da câmera.

    private void Start()
    {
        // Bloqueia o cursor no centro da tela e o torna invisível para a experiência do jogo.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<Camera>().DOFieldOfView(initialFov, 0);
    }

    void Update()
    {
        // Pega o movimento do mouse no eixo X multiplicado pela sensibilidade, limitando entre -10 e 10
        float mouseX = Mathf.Clamp(Input.GetAxisRaw("Mouse X") * sensX, -10f, 10f);
        // Pega o movimento do mouse no eixo Y multiplicado pela sensibilidade, limitando entre -10 e 10
        float mouseY = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * sensY, -10f, 10f);

        // Atualiza a rotação acumulada no eixo Y (horizontal) adicionando o movimento do mouse X
        yRotation += mouseX;
        // Atualiza a rotação acumulada no eixo X (vertical) subtraindo o movimento do mouse Y (inverte para o eixo vertical)
        xRotation -= mouseY;
        // Limita a rotação vertical para não ultrapassar -90 e 90 graus (evita olhar para trás verticalmente)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Verifica se xRotation é inválido (NaN ou infinito) e reseta para 0 para evitar bugs
        if (float.IsNaN(xRotation) || float.IsInfinity(xRotation)) xRotation = 0f;
        // Mesma validação para yRotation
        if (float.IsNaN(yRotation) || float.IsInfinity(yRotation)) yRotation = 0f;

        // Aplica a rotação da câmera no objeto camHolder com os valores de rotação X e Y, e zero no eixo Z
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Aplica a rotação horizontal no objeto orientation, que geralmente controla a direção do personagem no eixo Y
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}