using UnityEngine;
using TMPro; 
public class BotaoVendaUI : MonoBehaviour
{
    public TextMeshProUGUI textoBotao;

    private DadosItem itemDesteBotao;
    private NPCMercador mercadorVinculado;

    // Funcao chamada pelo Mercador na hora que o botao nasce!
    public void ConfigurarBotao(DadosItem item, int quantidade, NPCMercador mercador)
    {
        itemDesteBotao = item;
        mercadorVinculado = mercador;

        // O Mercador so paga METADE do valor do item!
        // OBS: Troque 'valor' para 'valorEmOuro' se for o nome exato da sua variavel no DadosItem
        int precoDeVendaAoMercador = item.valorEmOuro / 2;

        // Ex: "Garra (x3) - Vender por 7 Ouro"
        textoBotao.text = item.name + " (x" + quantidade + ") - " + precoDeVendaAoMercador + " Ouro";
    }

    // Funcao que voce deve linkar no evento OnClick() deste prefab!
    public void ClicouVender()
    {
        mercadorVinculado.ExecutarVenda(itemDesteBotao);
    }
}