using UnityEngine;

public class MoedaColetavel : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("Quantidade de ouro que esta moeda vale ao ser coletada")]
    public int valor = 1; // Quanto vale essa moeda?

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se quem encostou na moeda tem a tag "Player" (o Herói)
        if (collision.CompareTag("Player"))
        {
            DadosGlobais.moedasJogador += valor;
            Destroy(gameObject);
        }
    }
}