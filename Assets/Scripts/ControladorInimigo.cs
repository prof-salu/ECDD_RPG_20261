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

    [Header("Sensores")]
    public float raioVisao = 5.0f;       // Distância para começar a seguir (Vermelho)
    public float raioPerseguicao = 8.0f; // Distância para desistir (Amarelo)
    public float distanciaAtaque = 1.0f; // Distância para iniciar o combate (Toque)


    private Animator animator;
    private SpriteRenderer spriteRenderer; // Necessário para o Flip
    private Transform transformJogador;



    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pegando o componente visual

        // --- BUSCA AUTOMÁTICA ---
        // Procura na cena um objeto com a etiqueta "Player"
        GameObject objetoPlayer = GameObject.FindGameObjectWithTag("Player");

        // Segurança: Só pegamos o Transform se o objeto existir
        if (objetoPlayer != null)
        {
            transformJogador = objetoPlayer.transform;
        }
        // Configuração inicial dos pontos de patrulha (Container)
        estadoAtual = EstadoInimigo.Patrulha;

    }

    void Update()
    {
        // Segurança: Se o player morreu ou não foi encontrado, não faz nada
        if (transformJogador == null) return;

        // 1. O SENSOR (Calcula a distância o tempo todo)
        float distancia = Vector2.Distance(transform.position, transformJogador.position);

        switch (estadoAtual)
        {
            case EstadoInimigo.Idle:
                animator.SetBool("Andando", false);
                break;

            case EstadoInimigo.Patrulha:
                // --- REGRA 1: TE VI! ---
                // Se o jogador entrou no círculo vermelho (Visão)...
                if (distancia < raioVisao)
                {
                    estadoAtual = EstadoInimigo.Perseguicao;
                }

                // Continua patrulhando normalmente
                Patrulhar();
                break;

            case EstadoInimigo.Perseguicao:
                // --- REGRA 2: FUGIU! ---
                // Se o jogador saiu do círculo amarelo (Perseguição)...
                if (distancia > raioPerseguicao)
                {
                    estadoAtual = EstadoInimigo.Patrulha;
                }

                // --- REGRA 3: PEGUEI! ---
                // Se chegou muito perto (encostou), inicia o combate
                if (distancia < distanciaAtaque)
                {
                    IniciarCombate();
                }
                else
                {
                    // Se ainda não pegou, corre atrás
                    Perseguir();
                }
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

    void Perseguir()
    {
        // 1. CALCULAR DIREÇÃO (Do Inimigo -> Para o Jogador)
        Vector3 direcao = (transformJogador.position - transform.position).normalized;

        // 2. ATUALIZAR ANIMATOR
        // Reutilizamos a lógica das Blend Trees!
        animator.SetBool("Andando", true);
        animator.SetFloat("Horizontal", direcao.x);
        animator.SetFloat("Vertical", direcao.y);

        // 3. FLIP (Espelhamento do Sprite)
        if (direcao.x < -0.1f) spriteRenderer.flipX = true;
        else if (direcao.x > 0.1f) spriteRenderer.flipX = false;

        // 4. MOVER (Um pouco mais rápido que a patrulha!)
        // Multiplicamos por 1.5 para dar sensação de urgência
        transform.position = Vector2.MoveTowards(transform.position, transformJogador.position, velocidade * 1.5f * Time.deltaTime);
    }

    void IniciarCombate()
    {
        // Por enquanto, apenas paramos o jogo para simular a entrada no turno
        Debug.Log("COMBATE INICIADO! (Jogador capturado)");

        // Congela o tempo do jogo (Pausa Total)
        Time.timeScale = 0;

        // (Na semana 05 faremos a troca de cena real aqui)
    }





    // Este método desenha na Scene do Unity automaticamente (não aparece no jogo final)
    void OnDrawGizmosSelected()
    {
        // Desenha o Raio de Visão (Vermelho)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioVisao);

        // Desenha o Raio de Perseguição (Amarelo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioPerseguicao);
    }

}