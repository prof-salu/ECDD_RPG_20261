using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FichaStatus : MonoBehaviour
{
    public GameObject janelaStatus;

    [Header("Textos")]
    public TextMeshProUGUI txtNome;
    public TextMeshProUGUI txtNivel;
    public TextMeshProUGUI txtAtaque;
    public TextMeshProUGUI txtVida;
    public TextMeshProUGUI txtLevelUpAviso;

    [Header("Barra de Experiência")]
    public Slider barraXP;
    public TextMeshProUGUI txtNumerosXP;

    private ProgressoJogador progressoHeroi;
    private int nivelAnterior;

    void Start()
    {
        janelaStatus.SetActive(false);
        nivelAnterior = DadosGlobais.nivelJogador;
        if (txtLevelUpAviso != null) txtLevelUpAviso.gameObject.SetActive(false);

        // Busca o Herói que está vivo no mapa!
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer != null)
        {
            progressoHeroi = objPlayer.GetComponent<ProgressoJogador>();
        }
    }

    void Update()
    {
        // Pressione 'I' para abrir/fechar o Status do Personagem
        if (Input.GetKeyDown(KeyCode.I))
        {
            janelaStatus.SetActive(!janelaStatus.activeSelf);
        }

        // Se a janela estiver aberta, desenha os números em tempo real!
        if (janelaStatus.activeSelf)
        {
            AtualizarFicha();
        }

        VerificarAnimacaoLevelUp();
    }

    void AtualizarFicha()
    {
        txtNome.text = "Herói: Aventureiro";
        txtNivel.text = "Nível Atual: " + DadosGlobais.nivelJogador;

        // Mostramos o cálculo de RPG clássico para o jogador
        int ataqueTotal = 15 + DadosGlobais.bonusAtaque; // Assumindo Base 15
        int vidaTotal = 100 + DadosGlobais.bonusVidaMax; // Assumindo Base 100

        txtAtaque.text = "Ataque: " + ataqueTotal + " (Base 15 + " + DadosGlobais.bonusAtaque + " Bônus)";
        txtVida.text = "Vida Max: " + vidaTotal + " (Base 100 + " + DadosGlobais.bonusVidaMax + " Bônus)";

        // Atualiza a Barra de XP PUXANDO DO JOGADOR
        if (barraXP != null && progressoHeroi != null)
        {
            int metaDeXP = 0;
            int nivelHeroi = DadosGlobais.nivelJogador;

            if (nivelHeroi <= progressoHeroi.xpNecessariaPorNivel.Length)
            {
                // Busca no Array de evolução o nível do Herói
                metaDeXP = progressoHeroi.xpNecessariaPorNivel[nivelHeroi - 1];
            }
            else
            {
                // Se chegou ao nível máximo, usa a última meta disponível
                metaDeXP = progressoHeroi.xpNecessariaPorNivel[progressoHeroi.xpNecessariaPorNivel.Length - 1];
            }

            barraXP.maxValue = metaDeXP;
            barraXP.value = DadosGlobais.xpJogador;
            txtNumerosXP.text = DadosGlobais.xpJogador + " / " + metaDeXP + " XP";
        }
    }

    void VerificarAnimacaoLevelUp()
    {
        // Feedback Visual simples se o nível subir!
        if (DadosGlobais.nivelJogador > nivelAnterior)
        {
            if (txtLevelUpAviso != null)
            {
                txtLevelUpAviso.gameObject.SetActive(true);
                Invoke("EsconderAvisoLevelUp", 3f); // Some após 3 segundos
            }
            nivelAnterior = DadosGlobais.nivelJogador;
        }
    }

    void EsconderAvisoLevelUp() { 
        txtLevelUpAviso.gameObject.SetActive(false); 
    }
}
