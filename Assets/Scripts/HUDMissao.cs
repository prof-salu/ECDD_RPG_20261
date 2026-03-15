using UnityEngine;
using TMPro;

public class HUDMissao : MonoBehaviour
{
    [Header("HUD da Miss�o")]
    public TextMeshProUGUI textoTrackerQuest;

    void Update()
    {
        if (textoTrackerQuest == null) return;

        if (DadosGlobais.historiaConcluida)
        {
            textoTrackerQuest.text = "Hist�ria Conclu�da!";
        }
        else if (DadosGlobais.questAtiva != null)
        {

            if (DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.CacarMonstros || DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.ColetarItens)
                textoTrackerQuest.text = "Miss�o Ativa: " + DadosGlobais.questAtiva.descricaoNoHUD + " (" + DadosGlobais.progressoQuestAtual + "/" + DadosGlobais.questAtiva.quantidadeObjetivo + " " + DadosGlobais.questAtiva.nomeItemColeta + ")";
            else
                textoTrackerQuest.text = "Miss�o Ativa: " + DadosGlobais.questAtiva.descricaoNoHUD;
        }
        else if (DadosGlobais.questDisponivel != null)
        {
            textoTrackerQuest.text = "Nova Miss�o: Procure o tri�ngulo azul no(a) " + DadosGlobais.questDisponivel.nomeNpcEmissor;
        }
        else
        {
            textoTrackerQuest.text = "Nenhuma miss�o ativa.";
        }
    }
}