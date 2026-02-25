using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoBatalha { Preparacao, TurnoJogador, TurnoInimigo, Vitoria, Derrota }

public class SistemaDeTurnos : MonoBehaviour
{
    public EstadoBatalha estadoAtual;

    [Header("Lutadores na Arena")]
    private AtributosCombate atributosHeroi;
    private AtributosCombate atributosInimigo;

    void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;
        StartCoroutine(ConfigurarBatalha());
    }

    IEnumerator ConfigurarBatalha()
    {
        Debug.Log("Preparando a Batalha...");
        yield return new WaitForSeconds(1f);

        // Encontra quem está na Arena usando Tags
        atributosHeroi = GameObject.FindGameObjectWithTag("Player").GetComponent<AtributosCombate>();
        atributosInimigo = GameObject.FindGameObjectWithTag("Inimigo").GetComponent<AtributosCombate>();

        estadoAtual = EstadoBatalha.TurnoJogador;
        IniciarTurnoJogador();
    }

    void IniciarTurnoJogador()
    {
        Debug.Log("A sua vez, Herói! Pressione ESPAÇO para atacar.");
    }

    void Update()
    {
        switch (estadoAtual)
        {
            case EstadoBatalha.TurnoJogador:

                // --- AÇÃO 1: ATAQUE BÁSICO ---
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Herói atacou com fúria!");
                    atributosInimigo.ReceberDano(atributosHeroi.danoBase);

                    VerificarFimDeTurnoJogador(); // Consome o turno
                }
                break;
        }
    }

    void VerificarFimDeTurnoJogador()
    {
        if (atributosInimigo.hpAtual <= 0)
        {
            estadoAtual = EstadoBatalha.Vitoria;
            StartCoroutine(FinalizarBatalha(true));
        }
        else
        {
            estadoAtual = EstadoBatalha.TurnoInimigo;
            StartCoroutine(TurnoDoInimigo());
        }
    }

    IEnumerator TurnoDoInimigo()
    {
        Debug.Log("Inimigo está pensando...");
        yield return new WaitForSeconds(2f);

        Debug.Log("O monstro atacou o Herói!");
        atributosHeroi.ReceberDano(atributosInimigo.danoBase);

        if (atributosHeroi.hpAtual <= 0)
        {
            estadoAtual = EstadoBatalha.Derrota;
            StartCoroutine(FinalizarBatalha(false));
        }
        else
        {
            estadoAtual = EstadoBatalha.TurnoJogador;
            IniciarTurnoJogador();
        }
    }

    IEnumerator FinalizarBatalha(bool jogadorVenceu)
    {
        yield return new WaitForSeconds(2f); // Dá tempo para o jogador ler o aviso

        if (jogadorVenceu)
        {
            Debug.Log("VITÓRIA! A regressar à exploração...");

            // O CEMITÉRIO: Anota o ID do monstro para ele não voltar!
            DadosGlobais.inimigosDerrotados.Add(DadosGlobais.idInimigoEmCombate);

            SceneManager.LoadScene("Mundo");
        }
        else
        {
            Debug.Log("DERROTA... Fim de Jogo.");
            SceneManager.LoadScene("GameOver");
        }
    }
}