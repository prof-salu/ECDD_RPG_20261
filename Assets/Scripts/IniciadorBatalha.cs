using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciadorBatalha : MonoBehaviour
{
    // Esta função "captura" os dados de quem a chamou e executa a transição!
    public void DispararBatalha(GameObject player, string idUnico, List<GameObject> inimigosDesteGrupo, List<int> niveis)
    {
        if (inimigosDesteGrupo == null || inimigosDesteGrupo.Count == 0) return;

        // Configura o mundo
        DadosGlobais.posicaoRetornoJogador = player.transform.position;
        DadosGlobais.idInimigoEmCombate = idUnico;
        // Garante que os monstros vão para a Arena!
        DadosGlobais.prefabsInimigos = new List<GameObject>(inimigosDesteGrupo);
        DadosGlobais.niveisInimigosParaArena = new List<int>(niveis);

        // ---> O SAVE AQUI! (Tira a foto do herói na floresta) <---
        IniciadorBatalha.SalvarDadosJogador(player);

        SceneManager.LoadScene("CenaBatalha");
    }

    // ==========================================
    // 2. FUNÇÕES DE SAVE / LOAD CENTRALIZADAS
    // ==========================================

    // Usamos 'static' para podermos chamar de qualquer lugar sem precisar da referência do objeto!
    public static void SalvarDadosJogador(GameObject player)
    {
        if (player == null) return;

        AtributosCombate atributos = player.GetComponent<AtributosCombate>();
        ProgressoJogador progresso = player.GetComponent<ProgressoJogador>();
        SistemaInventario inventario = player.GetComponent<SistemaInventario>();
        if (atributos != null)
        {
            DadosGlobais.hpAtualJogador = atributos.hpAtual;
            DadosGlobais.nivelJogador = atributos.nivel;
            DadosGlobais.moedasJogador = inventario.moedas;
        }

        if (progresso != null)
        {
            DadosGlobais.xpJogador = progresso.xpAtual;
        }

        Debug.Log("IniciadorBatalha: Dados do Herói salvos com sucesso.");
    }

    public static void CarregarDadosJogador(GameObject player)
    {
        // Se a vida atual é -1, significa que o jogo acabou de abrir, então não carregamos nada.
        if (player == null || DadosGlobais.hpAtualJogador == -1) return;

        AtributosCombate atributos = player.GetComponent<AtributosCombate>();
        ProgressoJogador progresso = player.GetComponent<ProgressoJogador>();

        if (atributos != null)
        {
            atributos.nivel = DadosGlobais.nivelJogador;
            atributos.CalcularStatus(); // Recalcula o máximo de HP/Dano com o nível atual

            atributos.hpAtual = DadosGlobais.hpAtualJogador; // Devolve as feridas
            atributos.AtualizarBarra();
        }

        if (progresso != null)
        {
            progresso.xpAtual = DadosGlobais.xpJogador;
        }

        Debug.Log("IniciadorBatalha: Dados do Herói carregados com sucesso.");
    }
}