using System.Collections;
using UnityEngine;

// Olha as "caixinhas" do desenho de vocês virando código aqui!
public enum EstadoBatalha { Preparacao, TurnoJogador, TurnoInimigo, Vitoria, Derrota }

public class SistemaDeTurnos : MonoBehaviour
{
    public EstadoBatalha estadoAtual;

    void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;
        StartCoroutine(ConfigurarBatalha());
    }

    IEnumerator ConfigurarBatalha()
    {
        Debug.Log("Preparando a Batalha...");
        yield return new WaitForSeconds(1f);

        // Setinha do fluxograma: Preparação -> Turno Jogador
        estadoAtual = EstadoBatalha.TurnoJogador;
        IniciarTurnoJogador();
    }

    void IniciarTurnoJogador()
    {
        Debug.Log("Sua vez, Herói! Pressione ESPAÇO para atacar.");
    }

    // --- TESTE DE FLUXO ---
    void Update()
    {
        if (estadoAtual == EstadoBatalha.TurnoJogador && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jogador atacou o monstro!");

            // Setinha do fluxograma: Turno Jogador -> Turno Inimigo
            estadoAtual = EstadoBatalha.TurnoInimigo;
            StartCoroutine(TurnoDoInimigo());
        }
    }

    IEnumerator TurnoDoInimigo()
    {
        Debug.Log("Inimigos estão pensando...");
        yield return new WaitForSeconds(2f);

        Debug.Log("Inimigo atacou o herói!");

        // Setinha do fluxograma: Turno Inimigo -> Turno Jogador
        estadoAtual = EstadoBatalha.TurnoJogador;
        IniciarTurnoJogador();
    }
}