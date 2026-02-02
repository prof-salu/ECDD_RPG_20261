using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public DadosItem itemParaDar; // Qual ScriptableObject é esse?
    public int quantidade = 1;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Auto-configuração: Pega o sprite do ScriptableObject e coloca no chão
        if (itemParaDar != null)
        {
            spriteRenderer.sprite = itemParaDar.icone;
        }
    }

    // Função nativa da Unity para quando algo entra no Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se quem bateu foi o Player
        if (other.CompareTag("Player"))
        {
            // Tenta achar o inventário no Gerenciador Global
            // FindFirstObjectByType procura o script na cena inteira
            SistemaInventario inventario = FindFirstObjectByType<SistemaInventario>();

            if (inventario != null)
            {
                inventario.AdicionarItem(itemParaDar, quantidade);
                Destroy(gameObject); // O item some do mundo
            }
        }
    }
}