using UnityEngine;

public class CraftingSimples : MonoBehaviour
{
    public SistemaInventario inventario;
    [Header("Receita de Flechas")]
    public DadosItem madeira;
    public DadosItem flecha;

    // Regra: 1 Madeira vira 5 Flechas. Simples e direto.
    public int custoMadeira = 1;
    public int quantidadeProduzida = 5;

    public void CraftarFlechas()
    {
        if (inventario.TemItem(madeira, custoMadeira))
        {
            inventario.RemoverItem(madeira, custoMadeira);

            // Cria 5 flechas de uma vez
            inventario.AdicionarItem(flecha, quantidadeProduzida);

            Debug.Log($"Sucesso! {quantidadeProduzida} Flechas criadas!");
        }
        else
        {
            Debug.Log("Falha: Você precisa de madeira!");
        }
    }
}