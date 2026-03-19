using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Animations;

public class FichaStatus : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI txtNome;
    public TextMeshProUGUI txtNivel;
    public TextMeshProUGUI txtAtaque;
    public TextMeshProUGUI txtVida;

    [Header("Barra de Experiência")]
    public Slider barraXP;
    public TextMeshProUGUI txtNumerosXP;

    private ProgressoJogador progressoHeroi;
    private AtributosCombate atributosHeroi;
    private GameObject player;


    void Start()
    {
        gameObject.SetActive(false);
        // Busca o Herói que está vivo no mapa!
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            progressoHeroi = player.GetComponent<ProgressoJogador>();
            atributosHeroi = player.GetComponent<AtributosCombate>();
        }
    }
    public void AtualizarFicha()
    {
        if (atributosHeroi == null || progressoHeroi == null) return;

        // 1. Textos baseados nos Atributos Reais
        txtNome.text = $"Herói: {atributosHeroi.nomePersonagem}";
        txtNivel.text = $"Nível Atual: {atributosHeroi.nivel}";

        int ataqueTotal = atributosHeroi.danoAtual + DadosGlobais.bonusAtaque;
        int vidaTotal = atributosHeroi.hpMaximo + DadosGlobais.bonusVidaMax;

        txtAtaque.text = $"Ataque:  {ataqueTotal} ({atributosHeroi.danoAtual} + {DadosGlobais.bonusAtaque}) ";
        txtVida.text = $"HP:  {vidaTotal} ({atributosHeroi.hpMaximo} + {DadosGlobais.bonusVidaMax}) ";

        // 2. Atualiza a Barra de XP PUXANDO DO JOGADOR
        if (barraXP != null)
        {
            int metaDeXP = 0;
            int nivelHeroi = atributosHeroi.nivel;

            if (nivelHeroi <= progressoHeroi.xpNecessariaPorNivel.Length)
            {
                metaDeXP = progressoHeroi.xpNecessariaPorNivel[nivelHeroi - 1];
            }
            else
            {
                metaDeXP = progressoHeroi.xpNecessariaPorNivel[progressoHeroi.xpNecessariaPorNivel.Length - 1];
            }

            barraXP.maxValue = metaDeXP;
            barraXP.value = progressoHeroi.xpAtual; // Puxa a XP fresquinha!
            txtNumerosXP.text = $"{progressoHeroi.xpAtual} / {metaDeXP} XP";
        }
    }
}
