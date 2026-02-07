using UnityEngine;

public class ControladorInimigo : MonoBehaviour
{
    // 1. DEFINIÇÃO DOS ESTADOS
    public enum EstadoInimigo
    {
        Idle,   // Parado
        Patrulha, // Andando
        Perseguicao   // Correndo atrás do player
    }

    [Header("IA do Inimigo")]
    public EstadoInimigo estadoAtual;

    [Header("Movimentação")]
    public float velocidade = 2.0f;
    public Transform[] pontosDePatrulha;
    private int indicePontoAtual = 0;

    [Header("Espera")]
    public float tempoDeEspera = 2.0f; // Tempo que fica parado em cada ponto
    private float cronometroEspera = 0f; // Contador interno


    private Animator animator;
    private SpriteRenderer spriteRenderer; // Necessário para o Flip


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        estadoAtual = EstadoInimigo.Idle;
    }

    void Update()
    {
        switch (estadoAtual)
        {
            case EstadoInimigo.Idle:
                animator.SetBool("Andando", false);
                Debug.Log($"[{gameObject.name}] ==> Mudando para o estado: {estadoAtual}");
                break;

            case EstadoInimigo.Patrulha:
                // Lógica na próxima aula
                animator.SetBool("Andando", true);
                Patrulhar();
                Debug.Log($"[{gameObject.name}] ==> Mudando para o estado: {estadoAtual}");
                break;

            case EstadoInimigo.Perseguicao:
                Debug.Log($"[{gameObject.name}] ==> Mudando para o estado: {estadoAtual}");
                break;
        }
    }

    void Patrulhar()
    {
        Transform alvo = pontosDePatrulha[indicePontoAtual];

        // 1. CHECAR DISTÂNCIA (Chegou no ponto?)
        if (Vector2.Distance(transform.position, alvo.position) < 0.1f)
        {
            // --- LÓGICA DE ESPERA ---
            // Cheguei! Paro a animação.
            animator.SetBool("Andando", false);

            // Conto o tempo
            cronometroEspera += Time.deltaTime;

            // Se o tempo passou do limite...
            if (cronometroEspera >= tempoDeEspera)
            {
                // Reseta o timer
                cronometroEspera = 0;
                // Vai para o próximo ponto
                indicePontoAtual++;

                // Loop: Se chegou no último, volta para o zero
                if (indicePontoAtual >= pontosDePatrulha.Length)
                {
                    indicePontoAtual = 0;
                }
            }
        }
        else
        {
            // --- LÓGICA DE MOVER ---
            // Se não chegou, continua andando

            // 2. CALCULAR A DIREÇÃO
            Vector3 direcao = (alvo.position - transform.position).normalized;

            // 3. ATUALIZAR O ANIMATOR
            animator.SetBool("Andando", true);
            animator.SetFloat("Horizontal", direcao.x);
            animator.SetFloat("Vertical", direcao.y);

            // 4. FLIP DO SPRITE
            if (direcao.x < -0.1f) spriteRenderer.flipX = true;
            else if (direcao.x > 0.1f) spriteRenderer.flipX = false;

            // 5. MOVER
            transform.position = Vector2.MoveTowards(transform.position, alvo.position, velocidade * Time.deltaTime);
        }
    }

}