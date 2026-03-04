using UnityEngine;
using UnityEngine.UI;

public class AtributosCombate : MonoBehaviour
{
    public string nomePersonagem;
    
    [Header("Nível do Personagem")]
    [Tooltip("Herói começa no 1. Para inimigos, defina a dificuldade manual!")]
    public int nivel = 1; 

    [Header("Status Base (No Nível 1)")]
    public int hpBase = 100;
    public int danoBase = 10;
    
    [Header("Status Calculados (Não Mexer)")]
    public int hpMaximo;
    public int hpAtual;
    public int danoAtual;

    [Header("UI")]
    public Slider minhaBarraDeVida;


    void Start() 
    {
        CalcularStatus();

        if (gameObject.CompareTag("Player") && DadosGlobais.hpAtualJogador != -1)
        {
            hpAtual = DadosGlobais.hpAtualJogador;
        }
        else
        {
            hpAtual = hpMaximo; // Nasce com vida cheia (Inimigos ou início de jogo)
        }

        AtualizarBarra();

    }

    public void ReceberDano(int valorDano)
    {
        hpAtual -= valorDano;
        
        Debug.Log(nomePersonagem + " recebeu " + valorDano + " de dano! HP: " + hpAtual);

        if (hpAtual <= 0)
        {
            hpAtual = 0;
            gameObject.SetActive(false);
        }

        AtualizarBarra();
    }

    public void Curar(int valorCura)
    {
        hpAtual += valorCura;

        Debug.Log(nomePersonagem + " recebeu " + valorCura + " de cura! HP: " + hpAtual);

        // Impede que a vida ultrapasse o máximo!
        if (hpAtual > hpMaximo) hpAtual = hpMaximo;

        AtualizarBarra();
    }

    public void AtualizarBarra()
    {
        if (minhaBarraDeVida != null)
        {
            minhaBarraDeVida.maxValue = hpMaximo;
            minhaBarraDeVida.value = hpAtual;
        }
    }

    public void CalcularStatus()
    {
        // A matemática da evolução: Ganha +20 HP e +5 Dano por cada nível extra!
        hpMaximo = hpBase + ((nivel - 1) * 20);
        danoAtual = danoBase + ((nivel - 1) * 5);

        if (hpAtual > hpMaximo) hpAtual = hpMaximo;

    }
}