using UnityEngine;
using TMPro;

public class HUDMissao : MonoBehaviour
{
    [Header("HUD da Miss�zo")]
    public TextMeshProUGUI textoTrackerQuest;

    void Update()
    {
        if (textoTrackerQuest == null) return;

        if (DadosGlobais.historiaConcluida)
        {
            textoTrackerQuest.text = "Historia Concluida!";
        }
        else if (DadosGlobais.questAtiva != null)
        {

            if (DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.CacarMonstros || DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.ColetarItens)
                textoTrackerQuest.text = "Missao Ativa: " + DadosGlobais.questAtiva.descricaoNoHUD + " (" + DadosGlobais.progressoQuestAtual + "/" + DadosGlobais.questAtiva.quantidadeObjetivo + " " + DadosGlobais.questAtiva.nomeItemColeta + ")";
            else
                textoTrackerQuest.text = "Missao Ativa: Procure pelo(a) " + DadosGlobais.questAtiva.nomeNpcDestino;
        }
        else if (DadosGlobais.questDisponivel != null)
        {
            textoTrackerQuest.text = "Nova Missao: Procure o triangulo azul no(a) " + DadosGlobais.questDisponivel.nomeNpcEmissor;
        }
        else
        {
            textoTrackerQuest.text = "Nenhuma Missao ativa.";
        }
    }
}