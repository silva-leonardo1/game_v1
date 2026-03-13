using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// moveSpeed = 6
// groundDrag = 5
// jumpForce = 7
// jumpCooldown = 0.25
// airMultiplier = 0.4
// playerHeight = 2


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; 
    // Velocidade de movimento do jogador.
    // Se aumentar jogador anda mais rápido
    // Se diminuir jogador anda mais devagar

    public float groundDrag; 
    // Atrito aplicado quando o jogador está no chão.
    // Maior valor para mais rápido
    // Menor valor desliza mais

    public float jumpForce; 
    // Força do pulo.
    // Maior valor pula mais alto
    // Menor valor pula mais baixo

    public float jumpCooldown; 
    // Tempo mínimo entre pulos.
    // Maior valor demora mais para poder pular novamente

    public float airMultiplier; 
    // Multiplicador de controle no ar.
    // Maior valor mais controle no ar
    // Menor valor movimento mais "duro" no ar

    bool readyToJump; 
    // Controla se o jogador pode pular ou não

    [HideInInspector] public float walkSpeed;
    // Velocidade de caminhada (oculta no inspector)

    [HideInInspector] public float sprintSpeed;
    // Velocidade de corrida (oculta no inspector)

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    // Tecla usada para pular (padrão: espaço)

    [Header("Ground Check")]
    public float playerHeight;
    // Altura do jogador usada para detectar o chão

    public LayerMask whatIsGround;
    // Define quais layers contam como chão

    bool grounded;
    // Diz se o jogador está tocando o chão

    public Transform orientation;
    // Referência da direção da câmera ou do player
    // Define para onde "frente" está

    float horizontalInput;
    // Input A/D ou seta esquerda/direita

    float verticalInput;
    // Input W/S ou seta cima/baixo

    Vector3 moveDirection;
    // Direção final que o jogador vai se mover

    Rigidbody rb;
    // Rigidbody usado para movimentação física

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Pega o Rigidbody do jogador

        rb.freezeRotation = true;
        // Impede o jogador de tombar ao colidir

        readyToJump = true;
        // Permite pular no início
    }

    private void Update()
    {
        // Verifica se o jogador está no chão usando Raycast
        grounded = Physics.Raycast(
            transform.position, 
            Vector3.down, 
            playerHeight * 0.5f + 0.3f, 
            whatIsGround
        );

        MyInput(); 
        // Lê os inputs do jogador

        SpeedControl(); 
        // Limita a velocidade máxima

        // controla o atrito
        if (grounded)
            rb.linearDamping = groundDrag;
            // Mais atrito quando no chão
        else
            rb.linearDamping = 0;
            // Sem atrito no ar
    }

    private void FixedUpdate()
    {
        MovePlayer();
        // Movimento baseado em física deve ficar no FixedUpdate
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        // Input A/D

        verticalInput = Input.GetAxisRaw("Vertical");
        // Input W/S

        // verifica se pode pular
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            // trava pulo até cooldown acabar

            Jump();
            // executa o pulo

            Invoke(nameof(ResetJump), jumpCooldown);
            // libera pulo novamente após cooldown
        }
    }

    private void MovePlayer()
    {
        // calcula direção do movimento baseada na orientação
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // movimento no chão
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // movimento no ar
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        // pega velocidade horizontal (ignora eixo Y)
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limita velocidade máxima
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;

            rb.linearVelocity = new Vector3(
                limitedVel.x,
                rb.linearVelocity.y,
                limitedVel.z
            );
        }
    }

    private void Jump()
    {
        // zera velocidade vertical antes de pular
        // evita pulos inconsistentes
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        // aplica força para cima
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        // libera o pulo novamente
        readyToJump = true;
    }
}