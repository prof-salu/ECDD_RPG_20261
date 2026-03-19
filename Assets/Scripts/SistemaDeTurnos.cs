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

    private GameObject jogador;
    private AtributosCombate atributosJogador;
    private ProgressoJogador progressoJogador;
    private SistemaInventario inventarioJogador;

    // A fila de monstros vivos
    private List<AtributosCombate> inimigosVivos = new List<AtributosCombate>();

    [Tooltip("Arraste o arquivo Scriptable Object da Poção aqui")]
    public DadosItem pocaoDeVida;

    void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;
        StartCoroutine(ConfigurarBatalha());
        jogador = GameObject.FindGameObjectWithTag("Player");
        atributosJogador = jogador.GetComponent<AtributosCombate>();
        progressoJogador = jogador.GetComponent<ProgressoJogador>();
        inventarioJogador = jogador.GetComponent<SistemaInventario>();
    }

    IEnumerator ConfigurarBatalha()
    {
        yield return new WaitForSeconds(0.5f);
                
        atributosJogador.minhaBarraDeVida = sliderHeroiUI;
        atributosJogador.AtualizarBarra();

        // 2. Preenche a fila de inimigos procurando pela Tag
        GameObject[] objsInimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        foreach (GameObject obj in objsInimigos)
        {
            inimigosVivos.Add(obj.GetComponent<AtributosCombate>());
        }

        IniciarTurnoJogador();
    }

    void IniciarTurnoJogador() { estadoAtual = EstadoBatalha.TurnoJogador; }
    
    public void BotaoAtacar()
    {
        // Protecao: Sa funciona se for o turno do Jogador
        if (estadoAtual != EstadoBatalha.TurnoJogador) return;

        // Pega sempre o primeiro da fila
        AtributosCombate alvo = inimigosVivos[0];
        alvo.ReceberDano(atributosJogador.danoAtual);

        // Se a vida dele chegou a zero, remove-o da fila
        if (alvo.hpAtual <= 0)
        {
            
            // 1. Busca os novos scripts separados
            RecompensaInimigo loot = alvo.GetComponent<RecompensaInimigo>();
            ProgressoJogador progresso = atributosJogador.GetComponent<ProgressoJogador>();

            // 2. Transfere a recompensa
            if (loot != null && progresso != null)
            {

                // 3. Salva as recompensas
                progresso.GanharXP(loot.xpDrop);
                inventarioJogador.ModificarMoedas(loot.moedasDrop);
                

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

        foreach (SlotInventario slot in inventarioJogador.inventario)
        {
            if (slot.dadosDoItem == pocaoDeVida && slot.quantidade > 0)
            {
                inventarioJogador.ConsumirItem(slot.dadosDoItem);
                consumiuApenasUma = true;
                break; // Para o loop para não gastar 2 poções de uma vez!
            }
        }

        // 2. Aplica a cura se o jogador tinha a poção!
        if (consumiuApenasUma)
        {
            atributosJogador.Curar(50);     // Cura
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
        // Se a fila de inimigos ficou vazia, o Heroi ganhou!
        if (inimigosVivos.Count == 0)
        {
            estadoAtual = EstadoBatalha.Vitoria;
            StartCoroutine(FinalizarBatalha(true));
        }
        else
        {
            // Se ainda ha monstros, passa-lhes a vez
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
            atributosJogador.ReceberDano(inimigo.danoBase);

            // Se o heroi morrer a meio dos ataques inimigos, para o ciclo imediatamente
            if (atributosJogador.hpAtual <= 0) break;
        }

        // Verifica se o heroi sobreviveu a rodada
        if (atributosJogador.hpAtual <= 0)
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
        if (jogadorVenceu) 
        {
            IniciadorBatalha.SalvarDadosJogador(jogador);
            yield return new WaitForSeconds(2f);
            // O CEMITERIO: Anota o ID do monstro para ele nao voltar!
            DadosGlobais.inimigosDerrotados.Add(DadosGlobais.idInimigoEmCombate);
            SceneManager.LoadScene("Mundo"); 
        }
        else 
        { 
            SceneManager.LoadScene("GameOver"); 
        }
    }
}