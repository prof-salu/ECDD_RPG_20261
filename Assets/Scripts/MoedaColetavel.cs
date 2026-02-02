using UnityEngine;

public class MoedaColetavel : MonoBehaviour
{
    public int valor = 1; // Quanto vale essa moeda?

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SistemaInventario inventario = FindFirstObjectByType<SistemaInventario>();
            if (inventario != null)
            {
                inventario.ModificarMoedas(valor);
                Destroy(gameObject); // Some com a moeda
            }
        }
    }
}