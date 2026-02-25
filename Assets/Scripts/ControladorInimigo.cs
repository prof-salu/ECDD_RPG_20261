using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorInimigo : MonoBehaviour
{
    // 1. DEFINIÇÃO DOS ESTADOS
    public enum EstadoInimigo
    {
        Idle,   // Parado
        Patrulha, // Andando
        Perseguicao   // Correndo atrás do player
    }

    [Header("Combate e Identificação")]
    [Tooltip("Dê um nome único. Ex: Lobo_Floresta_03")]
    public string idUnico;

    [Header("Combate")]
    [Tooltip("Arraste os PREFABS dos inimigos da aba PROJECT para cá")]
    public GameObject prefabDaArena; // Substitui a antiga string 'meuNomeID'

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

    private Rigidbody2D rb;
    private Collider2D meuCollider;

    // Variáveis para a ponte entre Update e FixedUpdate
    private Vector2 destinoMovimento;
    private float velocidadeAtual;
    private bool estaSeMovendo = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pegando o componente visual
        
        rb = GetComponent<Rigidbody2D>();
        meuCollider = GetComponent<Collider2D>();

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

        // Verifica se já foi derrotado antes de começar a patrulhar
        if (DadosGlobais.inimigosDerrotados.Contains(idUnico))
        {
            gameObject.SetActive(false);
            return;
        }

    }

    void Update()
    {
        estaSeMovendo = false; // Reseta todo frame. Só fica true se chamarmos MoverFisico()

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

    void FixedUpdate()
    {
        // Aqui o motor de física trabalha (corre em sincronia com as colisões)
        if (estaSeMovendo)
        {
            // NOTA: Usamos Time.fixedDeltaTime em vez de deltaTime!
            Vector2 novaPosicao = Vector2.MoveTowards(rb.position, destinoMovimento, velocidadeAtual * Time.fixedDeltaTime);
            rb.MovePosition(novaPosicao);
        }
    }

    // Esta função agora apenas "prepara" o movimento e atualiza a animação
    void MoverFisico(Vector3 destino, float velocidadeMovimento)
    {
        Vector3 direcao = (destino - transform.position).normalized;

        // Animação e Flip (Processados no Update)
        animator.SetBool("Andando", true);
        animator.SetFloat("Horizontal", direcao.x);
        animator.SetFloat("Vertical", direcao.y);

        if (direcao.x < -0.1f) spriteRenderer.flipX = true;
        else if (direcao.x > 0.1f) spriteRenderer.flipX = false;

        // "Avisa" o FixedUpdate o que ele deve fazer
        destinoMovimento = destino;
        velocidadeAtual = velocidadeMovimento;
        estaSeMovendo = true;
    }

    void Patrulhar()
    {
        // MODO FANTASMA: Permite atravessar paredes para não ficar preso ao voltar
        if (meuCollider != null) meuCollider.isTrigger = true;

        Transform alvo = pontosDePatrulha[indicePontoAtual];

        if (Vector2.Distance(transform.position, alvo.position) < 0.1f)
        {
            animator.SetBool("Andando", false);
            cronometroEspera += Time.deltaTime;
            if (cronometroEspera >= tempoDeEspera)
            {
                cronometroEspera = 0;
                indicePontoAtual++;
                if (indicePontoAtual >= pontosDePatrulha.Length) indicePontoAtual = 0;
            }
        }
        else
        {
            MoverFisico(alvo.position, velocidade);
        }
    }

    void Perseguir()
    {
        // MODO SÓLIDO: Na perseguição, bate nas paredes
        if (meuCollider != null) meuCollider.isTrigger = false;

        MoverFisico(transformJogador.position, velocidade * 1.5f);
    }

    void IniciarCombate()
    {
        IniciadorBatalha iniciador = GetComponentInParent<IniciadorBatalha>();

        // Empacota o nosso único inimigo numa lista só para ele!
        List<GameObject> listaInimigos = new List<GameObject>();
        listaInimigos.Add(prefabDaArena);

        if (iniciador != null)
            iniciador.DispararBatalha(transformJogador.gameObject, idUnico, listaInimigos);
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