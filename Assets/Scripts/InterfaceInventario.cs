using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InterfaceInventario : MonoBehaviour
{
    public SistemaInventario sistemaInventario; // Referência ao cérebro (Semana 01)
    public Transform containerDoGrid;         // O objeto que tem o Grid Layout
    public GameObject prefabSlot;             // O desenho do slot que criamos

    [Header("Economia UI")]
    public TextMeshProUGUI textoMoedas; // Referência ao texto de Ouro


    void Start()
    {
       // Sempre que o inventário mudar, a interface se redesenha automaticamente.
        sistemaInventario.onInventarioMudou += AtualizarInterface;

        AtualizarInterface(); // Atualiza a primeira vez ao iniciar
    }

    void Update()
    {
        // NOVO: O Inventário agora lê a fortuna global do jogador o tempo todo!
        if (textoMoedas != null)
        {
            textoMoedas.text = "Ouro: " + DadosGlobais.moedasJogador.ToString();
        }
    }


    public void AtualizarInterface()
    {
        foreach (Transform filho in containerDoGrid)
        {
            Destroy(filho.gameObject);
        }

        // 3. Reconstrução: Cria um slot novo para cada item na lista de dados
        foreach (SlotInventario slot in sistemaInventario.inventario)
        {
            // Instancia o desenho dentro do Grid
            GameObject novoSlot = Instantiate(prefabSlot, containerDoGrid);

            // Manda o slot desenhar o item correto
            novoSlot.GetComponent<SlotUI>().ConfigurarSlot(slot);
        }
    }
}