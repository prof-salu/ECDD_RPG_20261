using UnityEngine;

public class TesteInventario : MonoBehaviour
{
    public SistemaInventario inventario;

    public AtributosPersonagem meuPersonagem; // Arraste o componente aqui no Inspector

    public DadosEquipamento espada;
    public DadosEquipamento escudo;
    public DadosConsumivel pocao;
    public DadosItem item;

    private void Start()
    {
        inventario.AdicionarItem(pocao, 3);

        inventario.AdicionarItem(item, 1);

        inventario.AdicionarItem(pocao, 5);

        Debug.Log($"Dano Inicial (Sem arma): {meuPersonagem.CalcularDanoTotal()}");

        // 1. Equipa a espada velha
        inventario.AdicionarItem(espada, 1);
        meuPersonagem.EquiparArma(espada);
        Debug.Log($"Novo Dano: {meuPersonagem.CalcularDanoTotal()}");

        // 2. Acha um escudo
        Debug.Log("--- O JOGADOR ABRIU UM BAÚ DOURADO! ---");
        inventario.AdicionarItem(escudo, 1);
        meuPersonagem.EquiparEscudo(escudo);
        Debug.Log($"Defesa Final: {meuPersonagem.CalcularDefesaTotal()}");
    }
}
