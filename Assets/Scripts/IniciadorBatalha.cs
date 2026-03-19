using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciadorBatalha : MonoBehaviour
{
    // Esta funcao "captura" os dados de quem a chamou e executa a transicao!
    public void DispararBatalha(GameObject player, string idUnico, List<GameObject> inimigosDesteGrupo, List<int> niveis)
    {
        if (inimigosDesteGrupo == null || inimigosDesteGrupo.Count == 0) return;

        // Configura o mundo
        DadosGlobais.posicaoRetornoJogador = player.transform.position;
        DadosGlobais.idInimigoEmCombate = idUnico;
        // Garante que os monstros vao para a Arena!
        DadosGlobais.prefabsInimigos = new List<GameObject>(inimigosDesteGrupo);
        DadosGlobais.niveisInimigosParaArena = new List<int>(niveis);

        // ---> O SAVE AQUI! (Tira a foto do heroi na floresta) <---
        IniciadorBatalha.SalvarDadosJogador(player);

        SceneManager.LoadScene("CenaBatalha");
    }

    // ==========================================
    // 2. FUNCOES DE SAVE / LOAD CENTRALIZADAS
    // ==========================================

    // Usamos 'static' para podermos chamar de qualquer lugar sem precisar da referencia do objeto!
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
            DadosGlobais.bonusAtaque = atributos.bonusAtaque;
            DadosGlobais.bonusDefesa = atributos.bonusDefesa;
        }

        if (progresso != null)
        {
            DadosGlobais.xpJogador = progresso.xpAtual;
        }

        Debug.Log("IniciadorBatalha: Dados do Heroi salvos com sucesso.");
    }

    public static void CarregarDadosJogador(GameObject player)
    {
        // Se a vida atual for -1, significa que o jogo acabou de abrir, entao nao carregamos nada.
        if (player == null || DadosGlobais.hpAtualJogador == -1) return;

        AtributosCombate atributos = player.GetComponent<AtributosCombate>();
        ProgressoJogador progresso = player.GetComponent<ProgressoJogador>();

        if (atributos != null)
        {
            atributos.nivel = DadosGlobais.nivelJogador;
            atributos.bonusAtaque = DadosGlobais.bonusAtaque;
            atributos.bonusDefesa = DadosGlobais.bonusDefesa;
            atributos.CalcularStatus(); // Recalcula o maximo de HP/Dano com o nivel atual

            atributos.hpAtual = DadosGlobais.hpAtualJogador; // Devolve as feridas
            atributos.AtualizarBarra();
        }

        if (progresso != null)
        {
            progresso.xpAtual = DadosGlobais.xpJogador;
        }

        Debug.Log("IniciadorBatalha: Dados do Heroi carregados com sucesso.");
    }
}