using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum EstadoBatalha { Preparacao, TurnoJogador, TurnoInimigo, Vitoria, Derrota }

public class SistemaDeTurnos : MonoBehaviour
{
    public EstadoBatalha estadoAtual;
    public Slider sliderHeroiUI;

    private AtributosCombate atributosHeroi;

    // A fila de monstros vivos
    private List<AtributosCombate> inimigosVivos = new List<AtributosCombate>();

    void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;
        StartCoroutine(ConfigurarBatalha());
    }

    IEnumerator ConfigurarBatalha()
    {
        yield return new WaitForSeconds(0.5f);

        // 1. Configura o Herói e a sua Barra de Vida
        atributosHeroi = GameObject.FindGameObjectWithTag("Player").GetComponent<AtributosCombate>();
        atributosHeroi.minhaBarraDeVida = sliderHeroiUI;
        atributosHeroi.AtualizarBarra();

        // 2. Preenche a fila de inimigos procurando pela Tag
        GameObject[] objsInimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        foreach (GameObject obj in objsInimigos)
        {
            inimigosVivos.Add(obj.GetComponent<AtributosCombate>());
        }

        IniciarTurnoJogador();
    }

    void IniciarTurnoJogador() { estadoAtual = EstadoBatalha.TurnoJogador; }

    // --- FUNÇÕES DOS BOTÕES (O 'Update' já não mora aqui!) ---

    public void BotaoAtacar()
    {
        // Proteção: Só funciona se for o turno do Jogador
        if (estadoAtual != EstadoBatalha.TurnoJogador) return;

        // Pega sempre o primeiro da fila
        AtributosCombate alvo = inimigosVivos[0];
        alvo.ReceberDano(atributosHeroi.danoBase);

        // Se a vida dele chegou a zero, remove-o da fila
        if (alvo.hpAtual <= 0) inimigosVivos.RemoveAt(0);

        VerificarFimDeTurnoJogador();
    }

    public void BotaoPocao()
    {
        if (estadoAtual != EstadoBatalha.TurnoJogador) return;

        // Chama a função Curar que criámos no AtributosCombate!
        atributosHeroi.Curar(30);

        VerificarFimDeTurnoJogador();
    }
    // -----------------------------------------------------------

    void VerificarFimDeTurnoJogador()
    {
        // Se a fila de inimigos ficou vazia, o Herói ganhou!
        if (inimigosVivos.Count == 0)
        {
            estadoAtual = EstadoBatalha.Vitoria;
            StartCoroutine(FinalizarBatalha(true));
        }
        else
        {
            // Se ainda há monstros, passa-lhes a vez
            estadoAtual = EstadoBatalha.TurnoInimigo;
            StartCoroutine(TurnoDoInimigo());
        }
    }

    IEnumerator TurnoDoInimigo()
    {
        // O Contra-Ataque: Cada monstro vivo ataca e espera 1 segundo
        foreach (AtributosCombate inimigo in inimigosVivos)
        {
            yield return new WaitForSeconds(1f);
            atributosHeroi.ReceberDano(inimigo.danoBase);

            // Se o herói morrer a meio dos ataques inimigos, para o ciclo imediatamente
            if (atributosHeroi.hpAtual <= 0) break;
        }

        // Verifica se o herói sobreviveu à rodada
        if (atributosHeroi.hpAtual <= 0)
        {
            estadoAtual = EstadoBatalha.Derrota;
            StartCoroutine(FinalizarBatalha(false));
        }
        else
        {
            IniciarTurnoJogador();
        }
    }

    IEnumerator FinalizarBatalha(bool jogadorVenceu)
    {
        // SALVANDO A VIDA: O Herói vai para o mapa com as feridas da batalha!
        DadosGlobais.hpAtualJogador = atributosHeroi.hpAtual;

        yield return new WaitForSeconds(2f);

        if (jogadorVenceu) 
        {
            // O CEMITÉRIO: Anota o ID do monstro para ele não voltar!
            DadosGlobais.inimigosDerrotados.Add(DadosGlobais.idInimigoEmCombate);
            SceneManager.LoadScene("Mundo"); 
        }
        else 
        { 
            SceneManager.LoadScene("GameOver"); 
        }
    }
}