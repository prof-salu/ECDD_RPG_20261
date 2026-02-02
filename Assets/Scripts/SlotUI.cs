using UnityEngine;
using UnityEngine.UI; // Necessário para imagens
using TMPro; // Necessário para textos TextMeshPro

public class SlotUI : MonoBehaviour
{
    public Image imagemIcone;
    public TextMeshProUGUI textoQuantidade;

    // Esta função será chamada pelo Gerenciador para "vestir" o slot
    public void ConfigurarSlot(SlotInventario slot)
    {
        if (slot != null && slot.dadosDoItem != null)
        {
            // 1. Liga o ícone e define a imagem correta
            imagemIcone.enabled = true;
            imagemIcone.sprite = slot.dadosDoItem.icone;

            // 2. Define a quantidade
            if (slot.quantidade > 1)
                textoQuantidade.text = slot.quantidade.ToString();
            else
                textoQuantidade.text = ""; // Não mostra número se for só 1 item
        }
        else
        {
            // Se o slot estiver vazio (segurança)
            imagemIcone.enabled = false;
            textoQuantidade.text = "";
        }
    }
}