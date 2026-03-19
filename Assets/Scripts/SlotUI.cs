using UnityEngine;
using UnityEngine.UI; // Necessario para imagens
using TMPro; // Necessario para textos TextMeshPro

public class SlotUI : MonoBehaviour
{
    public Image imagemIcone;
    public TextMeshProUGUI textoQuantidade;

    // Esta funcao sera chamada pelo Gerenciador para "vestir" o slot
    public void ConfigurarSlot(SlotInventario slot)
    {
        if (slot != null && slot.dadosDoItem != null)
        {
            // 1. Liga o icone e define a imagem correta
            imagemIcone.enabled = true;
            imagemIcone.sprite = slot.dadosDoItem.icone;

            // 2. Define a quantidade
            if (slot.quantidade > 1)
                textoQuantidade.text = slot.quantidade.ToString();
            else
                textoQuantidade.text = ""; // Nao mostra numero se for so 1 item
        }
        else
        {
            // Se o slot estiver vazio (seguranca)
            imagemIcone.enabled = false;
            textoQuantidade.text = "";
        }
    }
}