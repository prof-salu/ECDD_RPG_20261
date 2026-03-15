using TMPro;
using UnityEngine;
using TMPro;

public class NPCMercador : MonoBehaviour
{
    [Header("Interface da Loja")]
    public GameObject painelLoja;
    public TextMeshProUGUI textoFeedback;
    
    [Header("Integração com Inventário")]
    public SistemaInventario sistemaInventario; // Para entregarmos a poção na mochila
    public DadosItem pocaoDeVida;
    public int precoPocao = 50;

    private bool jogadorPerto = false;
    
    [Header("Upgrades Permanentes")]
    public int precoAfiarEspada = 100;
    public int precoArmadura = 150;

    void Update()
    {
        // Se o jogador estiver perto e apertar 'E', abre a loja
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            AbrirLoja();
        }
    }

    public void AbrirLoja()
    {
        painelLoja.SetActive(true);
        textoFeedback.text = "Bem-vindo!\nO que vai querer hoje?";
    }

    public void FecharLoja()
    {
        painelLoja.SetActive(false);
    }

    // --- LÓGICA DE COMPRA ---
    public void ComprarPocao()
    {
        // 1. Verifica se o jogador tem dinheiro suficiente no Banco Central (DadosGlobais)
        if (DadosGlobais.moedasJogador >= precoPocao)
        {
            // 2. Cobra o valor
            DadosGlobais.moedasJogador -= precoPocao;
            
            // 3. Entrega o item
            sistemaInventario.AdicionarItem(pocaoDeVida, 1);
            
            // 4. Feedback visual
            textoFeedback.text = "Poção comprada com sucesso!";
            Debug.Log("Comprou Poção! Saldo atual: " + DadosGlobais.moedasJogador);
        }
        else
        {
            // O jogador é pobre
            textoFeedback.text = "Ouro insuficiente! Vá caçar monstros.";
        }
    }
    
    // Adicione esta função abaixo da ComprarPocao()
    public void ComprarMelhoriaEspada()
    {
        if (DadosGlobais.moedasJogador >= precoAfiarEspada)
        {
            DadosGlobais.moedasJogador -= precoAfiarEspada;
            DadosGlobais.bonusAtaque += 10; // O herói dará +10 de dano para sempre!
            
            // Deixa mais caro para a próxima vez (Inflação do RPG!)
            precoAfiarEspada += 50; 
            
            textoFeedback.text = "Espada afiada! (+10 Ataque)";
        }
        else
        {
            textoFeedback.text = "Ouro insuficiente para melhorar a espada!";
        }
    }

    public void ComprarMelhoriaArmadura()
    {
        if (DadosGlobais.moedasJogador >= precoArmadura)
        {
            DadosGlobais.moedasJogador -= precoArmadura;
            DadosGlobais.bonusVidaMax += 25; // Herói terá +25 de Vida Máxima
            
            precoArmadura += 75; 
            textoFeedback.text = "Armadura reforçada! (+25 Vida)";
        }
        else
        {
            textoFeedback.text = "Ouro insuficiente para a armadura!";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) jogadorPerto = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = false;
            FecharLoja(); // Fecha a loja se o jogador for embora
        }
    }
}