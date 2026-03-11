using UnityEngine;
using TMPro;

public class HUDMissao : MonoBehaviour
{
    [Header("HUD da Missão")]
    public TextMeshProUGUI textoTrackerQuest;

    void Update()
    {
        if (textoTrackerQuest == null) return;

        if (DadosGlobais.historiaConcluida)
        {
            textoTrackerQuest.text = "História Concluída!";
        }
        else if (DadosGlobais.questAtiva != null)
        {

            if (DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.CacarMonstros || DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.ColetarItens)
                textoTrackerQuest.text = "Missão Ativa: " + DadosGlobais.questAtiva.descricaoNoHUD + " (" + DadosGlobais.progressoQuestAtual + "/" + DadosGlobais.questAtiva.quantidadeObjetivo + " " + DadosGlobais.questAtiva.nomeItemColeta + ")";
            else
                textoTrackerQuest.text = "Missão Ativa: " + DadosGlobais.questAtiva.descricaoNoHUD;
        }
        else if (DadosGlobais.questDisponivel != null)
        {
            textoTrackerQuest.text = "Nova Missão: Procure o triângulo azul no(a) " + DadosGlobais.questDisponivel.nomeNpcEmissor;
        }
        else
        {
            textoTrackerQuest.text = "Nenhuma missão ativa.";
        }
    }
}