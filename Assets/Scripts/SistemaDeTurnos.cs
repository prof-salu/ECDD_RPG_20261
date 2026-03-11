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

    [Tooltip("Arraste o arquivo Scriptable Object da Poção aqui")]
    public DadosItem pocaoDeVida;

    void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;
        StartCoroutine(ConfigurarBatalha());
    }

    IEnumerator ConfigurarBatalha()
    {
        yield return new WaitForSeconds(0.5f);

        // 1. Configura o Her�i e a sua Barra de Vida
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

    // --- FUN��ES DOS BOT�ES (O 'Update' j� n�o mora aqui!) ---

    public void BotaoAtacar()
    {
        // Prote��o: S� funciona se for o turno do Jogador
        if (estadoAtual != EstadoBatalha.TurnoJogador) return;

        // Pega sempre o primeiro da fila
        AtributosCombate alvo = inimigosVivos[0];
        alvo.ReceberDano(atributosHeroi.danoAtual);

        // Se a vida dele chegou a zero, remove-o da fila
        if (alvo.hpAtual <= 0)
        {
            
            // 1. Busca os novos scripts separados
            RecompensaInimigo loot = alvo.GetComponent<RecompensaInimigo>();
            ProgressoJogador progresso = atributosHeroi.GetComponent<ProgressoJogador>();

            // 2. Transfere a recompensa
            if (loot != null && progresso != null)
            {
                progresso.GanharXP(loot.xpDrop);
                DadosGlobais.moedasJogador += loot.moedasDrop;
                Debug.Log("Voc� encontrou " + loot.moedasDrop + " moedas!");

                // 3. Salva as recompensas
                DadosGlobais.xpJogador = progresso.xpAtual;
                DadosGlobais.nivelJogador = atributosHeroi.nivel;

                Debug.Log("Voc� encontrou " + loot.moedasDrop + " moedas!");

                // --- RASTREADOR DE MISSÕES (Caça ou Coleta) ---
                if (DadosGlobais.questAtiva != null) 
                {
                    if (DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.CacarMonstros || DadosGlobais.questAtiva.tipoDeMissao == TipoQuest.ColetarItens)
                    {
                        DadosGlobais.progressoQuestAtual++;
                        Debug.Log("Quest Atualizada no Console: " + DadosGlobais.progressoQuestAtual + "/" + DadosGlobais.questAtiva.quantidadeObjetivo);
                    }
                }
            }

            // 4. Tira o monstro da fila
            inimigosVivos.RemoveAt(0);
        }

        VerificarFimDeTurnoJogador();
    }

    public void BotaoPocao()
    {
        if (estadoAtual != EstadoBatalha.TurnoJogador) return;

        bool consumiuApenasUma = false;

        foreach (SlotInventario slot in DadosGlobais.inventarioAtual)
        {
            if (slot.dadosDoItem == pocaoDeVida && slot.quantidade > 0)
            {
                slot.quantidade--; // Gasta 1
                consumiuApenasUma = true;

                // Limpa da lista se a quantidade chegar a zero
                if (slot.quantidade <= 0)
                {
                    DadosGlobais.inventarioAtual.Remove(slot);
                }

                break; // Para o loop para não gastar 2 poções de uma vez!
            }
        }

        // 2. Aplica a cura se o jogador tinha a poção!
        if (consumiuApenasUma)
        {
            atributosHeroi.Curar(50);     // Cura
            Debug.Log("Você bebeu a poção deliciosa!");
            VerificarFimDeTurnoJogador(); // Passa o turno
        }
        else
        {
            Debug.LogWarning("Inventário vazio! Você não tem mais Poções de Vida!");
        }
    }

    void VerificarFimDeTurnoJogador()
    {
        // Se a fila de inimigos ficou vazia, o Her�i ganhou!
        if (inimigosVivos.Count == 0)
        {
            estadoAtual = EstadoBatalha.Vitoria;
            StartCoroutine(FinalizarBatalha(true));
        }
        else
        {
            // Se ainda h� monstros, passa-lhes a vez
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

            // Se o her�i morrer a meio dos ataques inimigos, para o ciclo imediatamente
            if (atributosHeroi.hpAtual <= 0) break;
        }

        // Verifica se o her�i sobreviveu � rodada
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
        // SALVANDO A VIDA: O Her�i vai para o mapa com as feridas da batalha!
        if (atributosHeroi != null)
        {
            // 1. Puxa os dados corretos dos componentes atualizados do Her�i
            ProgressoJogador progresso = atributosHeroi.GetComponent<ProgressoJogador>();

            // 2. Salva Vida e N�vel
            DadosGlobais.hpAtualJogador = atributosHeroi.hpAtual;
            DadosGlobais.nivelJogador = atributosHeroi.nivel;

            if (progresso != null)
            {
                DadosGlobais.xpJogador = progresso.xpAtual;
            }
        }

        yield return new WaitForSeconds(2f);

        if (jogadorVenceu) 
        {
            // O CEMIT�RIO: Anota o ID do monstro para ele n�o voltar!
            DadosGlobais.inimigosDerrotados.Add(DadosGlobais.idInimigoEmCombate);
            SceneManager.LoadScene("Mundo"); 
        }
        else 
        { 
            SceneManager.LoadScene("GameOver"); 
        }
    }
}