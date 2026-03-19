using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public DadosItem itemParaDar; // Qual ScriptableObject e esse?
    public int quantidade = 1;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Auto-configuracao: Pega o sprite do ScriptableObject e coloca no chao
        if (itemParaDar != null)
        {
            spriteRenderer.sprite = itemParaDar.icone;
        }
    }

    // Funcao nativa da Unity para quando algo entra no Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se quem bateu foi o Player
        if (other.CompareTag("Player"))
        {
            // Tenta achar o inventario no Gerenciador Global
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