using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorInimigo : MonoBehaviour
{
    // 1. DEFINICAO DOS ESTADOS
    public enum EstadoInimigo
    {
        Idle,   // Parado
        Patrulha, // Andando
        Perseguicao   // Correndo atras do player
    }

    [Header("Combate e Identificacao")]
    [Tooltip("De um nome unico. Ex: Lobo_Floresta_03")]
    public string idUnico;

    [Header("Combate")]
    [Tooltip("Arraste os PREFABS dos inimigos da aba PROJECT para ca")]
    public GameObject prefabDaArena; // Substitui a antiga string 'meuNomeID'

    [Header("IA do Inimigo")]
    public EstadoInimigo estadoAtual;

    [Header("Movimentacao")]
    public float velocidade = 2.0f;
    public Transform[] pontosDePatrulha;
    private int indicePontoAtual = 0;

    [Header("Espera")]
    public float tempoDeEspera = 2.0f; // Tempo que fica parado em cada ponto
    private float cronometroEspera = 0f; // Contador interno

    [Header("Sensores")]
    public float raioVisao = 5.0f;       // Distancia para comecar a seguir (Vermelho)
    public float raioPerseguicao = 8.0f; // Distancia para desistir (Amarelo)
    public float distanciaAtaque = 1.0f; // Distancia para iniciar o combate (Toque)


    private Animator animator;
    private SpriteRenderer spriteRenderer; // Necessario para o Flip
    private Transform transformJogador;

    private Rigidbody2D rb;
    private Collider2D meuCollider;

    // Variaveis para a ponte entre Update e FixedUpdate
    private Vector2 destinoMovimento;
    private float velocidadeAtual;
    private bool estaSeMovendo = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pegando o componente visual
        
        rb = GetComponent<Rigidbody2D>();
        meuCollider = GetComponent<Collider2D>();

        // --- BUSCA AUTOMATICA ---
        // Procura na cena um objeto com a etiqueta "Player"
        GameObject objetoPlayer = GameObject.FindGameObjectWithTag("Player");

        // Seguranca: So pegamos o Transform se o objeto existir
        if (objetoPlayer != null)
        {
            transformJogador = objetoPlayer.transform;
        }
        // Configuracao inicial dos pontos de patrulha (Container)
        estadoAtual = EstadoInimigo.Patrulha;

        // Verifica se ja foi derrotado antes de comecar a patrulhar
        if (DadosGlobais.inimigosDerrotados.Contains(idUnico))
        {
            gameObject.SetActive(false);
            return;
        }

    }

    void Update()
    {
        // Reseta o frame. So fica true se chamarmos MoverFisico()
        estaSeMovendo = false; 

        // Seguranca: Se o player morreu ou nao foi encontrado, nao faz nada
        if (transformJogador == null) return;

        // 1. O SENSOR (Calcula a distancia o tempo todo)
        float distancia = Vector2.Distance(transform.position, transformJogador.position);

        switch (estadoAtual)
        {
            case EstadoInimigo.Idle:
                animator.SetBool("Andando", false);
                break;

            case EstadoInimigo.Patrulha:
                // --- REGRA 1: TE VI! ---
                // Se o jogador entrou no circulo vermelho (Visao)...
                if (distancia < raioVisao)
                {
                    estadoAtual = EstadoInimigo.Perseguicao;
                }

                // Continua patrulhando normalmente
                Patrulhar();
                break;

            case EstadoInimigo.Perseguicao:
                // --- REGRA 2: FUGIU! ---
                // Se o jogador saiu do circulo amarelo (Perseguicao)...
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
                    // Se ainda nao pegou, corre atras
                    Perseguir();
                }
                break;
        }
    }

    void FixedUpdate()
    {
        // Aqui o motor de fisica trabalha (corre em sincronia com as colisoes)
        if (estaSeMovendo)
        {
            // NOTA: Usamos Time.fixedDeltaTime em vez de deltaTime!
            Vector2 novaPosicao = Vector2.MoveTowards(rb.position, destinoMovimento, velocidadeAtual * Time.fixedDeltaTime);
            rb.MovePosition(novaPosicao);
        }
    }

    // Esta funcao agora apenas "prepara" o movimento e atualiza a animacao
    void MoverFisico(Vector3 destino, float velocidadeMovimento)
    {
        Vector3 direcao = (destino - transform.position).normalized;

        // Animacao e Flip (Processados no Update)
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
        // MODO FANTASMA: Permite atravessar paredes para nao ficar preso ao voltar
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
        // MODO SOLIDO: Na perseguicao, bate nas paredes
        if (meuCollider != null) meuCollider.isTrigger = false;

        MoverFisico(transformJogador.position, velocidade * 1.5f);
    }

    void IniciarCombate()
    {
        IniciadorBatalha iniciador = GetComponentInParent<IniciadorBatalha>();

        // O EXTRATOR MAGICO 1: Le o nivel do proprio AtributosCombate na cena!
        AtributosCombate meusAtributos = GetComponent<AtributosCombate>();
        int meuNivelCena = (meusAtributos != null) ? meusAtributos.nivel : 1;

        if (iniciador != null)
        {
            iniciador.DispararBatalha(transformJogador.gameObject, idUnico,
                new List<GameObject> { prefabDaArena },
                new List<int> { meuNivelCena });
        }
    }

    // Este metodo desenha na Scene do Unity automaticamente (nao aparece no jogo final)
    void OnDrawGizmosSelected()
    {
        // Desenha o Raio de Visao (Vermelho)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioVisao);

        // Desenha o Raio de Perseguicao (Amarelo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioPerseguicao);
    }

}