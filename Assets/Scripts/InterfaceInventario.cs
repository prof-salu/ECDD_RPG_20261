using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InterfaceInventario : MonoBehaviour
{
    public SistemaInventario sistemaInventario; // Referencia ao cerebro (Semana 01)
    public Transform containerDoGrid;         // O objeto que tem o Grid Layout
    public GameObject prefabSlot;             // O desenho do slot que criamos

    [Header("Economia UI")]
    public TextMeshProUGUI textoMoedas; // Referencia ao texto de Ouro


    void Start()
    {
       // Sempre que o inventario mudar, a interface se redesenha automaticamente.
        sistemaInventario.OnInventarioMudou += AtualizarInterface;

        AtualizarInterface(); // Atualiza a primeira vez ao iniciar
    }

    void Update()
    {
        if (textoMoedas != null)
        {
            textoMoedas.text = "Ouro: " + sistemaInventario.moedas.ToString();
        }
    }


    private void AtualizarInterface()
    {
        foreach (Transform filho in containerDoGrid)
        {
            Destroy(filho.gameObject);
        }

        // 3. Reconstrucao: Cria um slot novo para cada item na lista de dados
        foreach (SlotInventario slot in sistemaInventario.inventario)
        {
            // Instancia o desenho dentro do Grid
            GameObject novoSlot = Instantiate(prefabSlot, containerDoGrid);

            // Manda o slot desenhar o item correto
            novoSlot.GetComponent<SlotUI>().ConfigurarSlot(slot);
        }
    }
}