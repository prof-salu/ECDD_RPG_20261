using UnityEngine;

public class MovimentoExploracao : MonoBehaviour
{
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private Vector2 movimento;
    private SpriteRenderer spriteRenderer;
    private Animator anim; // Para a animação no Passo 5

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Captura o Input (WASD ou Setas)
        // GetAxisRaw retorna -1, 0 ou 1 (movimento mais "seco" e preciso para Pixel Art)
        movimento.x = Input.GetAxisRaw("Horizontal");
        movimento.y = Input.GetAxisRaw("Vertical");

        // 2. Controle da Animação
        if (movimento != Vector2.zero)
        {
            // Diz para o Animator para onde estamos olhando
            anim.SetFloat("Horizontal", movimento.x);
            anim.SetFloat("Vertical", movimento.y);
            anim.SetBool("Andando", true);

            // --- LÓGICA DE ESPELHAMENTO (FLIP) ---
            // Como o nosso boneco só tem desenho olhando para a DIREITA,
            // precisamos inverte-lo via código quando ele vai para a ESQUERDA.

            if (movimento.x > 0)
            {
                // Indo para a Direita -> Normal (Não inverte)
                spriteRenderer.flipX = false;
            }
            else if (movimento.x < 0)
            {
                // Indo para a Esquerda -> Invertido (Espelho)
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            anim.SetBool("Andando", false);
        }
    }

    void FixedUpdate()
    {
        // 3. Aplica a Física
        // MovePosition é mais seguro que Translate para evitar atravessar paredes.
        // .normalized garante que andar na diagonal não seja mais rápido.
        rb.MovePosition(rb.position + movimento.normalized * velocidade * Time.fixedDeltaTime);
    }
}