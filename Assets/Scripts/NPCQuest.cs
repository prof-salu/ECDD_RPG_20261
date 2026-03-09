using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    [Header("Identidade")]
    public string meuNome; 
    
    [Header("Apenas para o 1º NPC do Jogo")]
    public Quest questInicialDoJogo; 

    [Header("Visuais")]
    [Tooltip("Arraste o Sprite Renderer do triângulo branco que fica acima do NPC")]
    public SpriteRenderer indicadorVisual; 

    private bool jogadorPerto = false;
    private GameObject jogadorRef;

    void Start()
    {
        if (questInicialDoJogo != null && DadosGlobais.questDisponivel == null && DadosGlobais.questAtiva == null && !DadosGlobais.historiaConcluida)
            DadosGlobais.questDisponivel = questInicialDoJogo;
    }

    void Update() 
    { 
        AtualizarIconeVisual();
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E)) Interagir(); 
    }

    void AtualizarIconeVisual()
    {
        if (indicadorVisual == null) return;
        indicadorVisual.gameObject.SetActive(false);

        if (DadosGlobais.historiaConcluida) return;

        // AZUL (Nova Quest)
        if (DadosGlobais.questAtiva == null && DadosGlobais.questDisponivel != null && DadosGlobais.questDisponivel.nomeNpcEmissor == meuNome)
        {
            indicadorVisual.color = Color.blue; 
            indicadorVisual.gameObject.SetActive(true);
        }
        // LARANJA (Entregar)
        else if (DadosGlobais.questAtiva != null && DadosGlobais.questAtiva.nomeNpcDestino == meuNome)
        {
            indicadorVisual.color = new Color(1f, 0.5f, 0f); 
            indicadorVisual.gameObject.SetActive(true);
        }
    }

    public void Interagir()
    {
        // -------------------------------------------------------------
        // TODO O DIÁLOGO VAI PARA O CONSOLE NESTA AULA! (Logica pura)
        // -------------------------------------------------------------
        
        if (DadosGlobais.historiaConcluida) 
        {
            Debug.Log(meuNome + " diz: A paz reina na nossa floresta graças a você!");
            return;
        }

        // CENA 1: JÁ TEMOS UMA MISSÃO
        if (DadosGlobais.questAtiva != null) 
        {
            Quest quest = DadosGlobais.questAtiva;

            if (quest.nomeNpcDestino == meuNome) 
            {
                bool terminouCaca = (quest.tipoDeMissao == TipoQuest.CacarMonstros && DadosGlobais.progressoQuestAtual >= quest.quantidadeObjetivo);
                bool terminouColeta = (quest.tipoDeMissao == TipoQuest.ColetarItens && DadosGlobais.progressoQuestAtual >= quest.quantidadeObjetivo);
                bool terminouEntrega = (quest.tipoDeMissao == TipoQuest.EncontrarNPC);

                if (terminouCaca || terminouColeta || terminouEntrega)
                {
                    Debug.Log(meuNome + " diz: " + quest.falaConclusao + " (Recebeu " + quest.recompensaOuro + " Ouro!)");
                    EntregarRecompensa(quest);
                }
                else
                {
                    // INCLUINDO O NOME DO ITEM AQUI!
                    Debug.Log(meuNome + " diz: " + quest.falaAndamento + " (Progresso: " + DadosGlobais.progressoQuestAtual + "/" + quest.quantidadeObjetivo + " " + quest.nomeItemColeta + ")");
                }
            }
            else 
            {
                Debug.Log(meuNome + " diz: O " + quest.nomeNpcDestino + " está à sua espera. Não perca tempo aqui!");
            }
            return;
        }

        // CENA 2: NOVA MISSÃO NO MUNDO
        if (DadosGlobais.questDisponivel != null)
        {
            if (DadosGlobais.questDisponivel.nomeNpcEmissor == meuNome)
            {
                Debug.Log(meuNome + " diz: " + DadosGlobais.questDisponivel.falaInicio);
                DadosGlobais.questAtiva = DadosGlobais.questDisponivel; 
                DadosGlobais.questDisponivel = null; 
                DadosGlobais.progressoQuestAtual = 0;
            }
            else
            {
                Debug.Log(meuNome + " diz: Ouvi dizer que o " + DadosGlobais.questDisponivel.nomeNpcEmissor + " está à sua procura.");
            }
            return;
        }

        // CENA 3: VAZIO
        Debug.Log(meuNome + " diz: Olá aventureiro! O dia está lindo hoje.");
    }

    void EntregarRecompensa(Quest questConcluida)
    {
        DadosGlobais.moedasJogador += questConcluida.recompensaOuro;
        
        // A Mágica: A próxima quest é liberada!
        DadosGlobais.questAtiva = null; 
        DadosGlobais.questDisponivel = questConcluida.proximaQuestDisponivel;

        if (DadosGlobais.questDisponivel == null) DadosGlobais.historiaConcluida = true;
    }

    private void OnTriggerEnter2D(Collider2D c) { if (c.CompareTag("Player")) { jogadorPerto = true; jogadorRef = c.gameObject; } }
    private void OnTriggerExit2D(Collider2D c) { if (c.CompareTag("Player")) { jogadorPerto = false; } }
}