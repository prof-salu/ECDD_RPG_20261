using UnityEngine;

public class MoedaColetavel : MonoBehaviour
{
    [Header("Configuracao")]
    [Tooltip("Quantidade de ouro que esta moeda vale ao ser coletada")]
    public int valor = 1; // Quanto vale essa moeda?

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se quem encostou na moeda tem a tag "Player" (o Heroi)
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<SistemaInventario>().ModificarMoedas(valor);
            Destroy(gameObject);
        }
    }
}