using UnityEngine;

public class TesteInventario : MonoBehaviour
{
    public SistemaInventario inventario;

    public DadosItem espada;
    public DadosItem escudo;
    public DadosItem pocao;
    public DadosItem item;

    private void Start()
    {
        inventario.AdicionarItem(pocao, 3);

        inventario.AdicionarItem(item, 1);

        inventario.AdicionarItem(pocao, 5);


        // 1. Equipa a espada velha
        inventario.AdicionarItem(espada, 1);

        // 2. Acha um escudo
        Debug.Log("--- O JOGADOR ABRIU UM BAÚ DOURADO! ---");
        inventario.AdicionarItem(escudo, 1);
    }
}
