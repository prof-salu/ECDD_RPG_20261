using UnityEngine;
using TMPro; 
public class BotaoVendaUI : MonoBehaviour
{
    public TextMeshProUGUI textoBotao;

    private DadosItem itemDesteBotao;
    private NPCMercador mercadorVinculado;

    // Função chamada pelo Mercador na hora que o botão nasce!
    public void ConfigurarBotao(DadosItem item, int quantidade, NPCMercador mercador)
    {
        itemDesteBotao = item;
        mercadorVinculado = mercador;

        // O Mercador só paga METADE do valor do item!
        // OBS: Troque 'valor' para 'valorEmOuro' se for o nome exato da sua variável no DadosItem
        int precoDeVendaAoMercador = item.valorEmOuro / 2;

        // Ex: "Garra (x3) - Vender por 7 Ouro"
        textoBotao.text = item.name + " (x" + quantidade + ") - " + precoDeVendaAoMercador + " Ouro";
    }

    // Função que você deve linkar no evento OnClick() deste prefab!
    public void ClicouVender()
    {
        mercadorVinculado.ExecutarVenda(itemDesteBotao);
    }
}