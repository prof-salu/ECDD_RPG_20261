using UnityEngine;

public class ControleUI : MonoBehaviour
{
    [Header("Paineis da UI")]
    public GameObject painelInventario;
    public GameObject painelStatus;
    public GameObject painelCrafting; // Arraste seu botão ou painel de craft aqui

    void Update()
    {
        // Se apertar I...
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Fecha o crafting para não ficar um por cima do outro
            painelCrafting.SetActive(false);
            painelStatus.SetActive(false);

            // Inverte o estado do inventário (Abre/Fecha)
            painelInventario.SetActive(!painelInventario.activeSelf);
        }

        // Se apertar C...
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Fecha o inventário
            painelInventario.SetActive(false);
            painelStatus.SetActive(false);

            // Inverte o estado do crafting
            painelCrafting.SetActive(!painelCrafting.activeSelf);
        }

        // Se apertar P...
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Fecha o inventário
            painelCrafting.SetActive(false);
            painelInventario.SetActive(false);

            // Inverte o estado do crafting
            painelStatus.SetActive(!painelStatus.activeSelf);

            // Se a janela estiver aberta, desenha os números em tempo real!
            if (painelStatus.activeSelf)
            {
                painelStatus.GetComponent<FichaStatus>().AtualizarFicha();
            }

        }

        // Dica Extra: ESC fecha tudo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            painelInventario.SetActive(false);
            painelCrafting.SetActive(false);
            painelStatus.SetActive(false);
        }
    }
}