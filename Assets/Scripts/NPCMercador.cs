using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NPCMercador : MonoBehaviour
{
    [Header("Interface da Loja")]
    public GameObject painelHub;
    public GameObject painelCompra; 
    public GameObject painelVenda;
    public TextMeshProUGUI textoFeedbackVenda;
    public TextMeshProUGUI textoFeedbackCompra;

    [Header("Gerador de Vendas")]
    public Transform containerVendas;
    public GameObject prefabBotaoVenda;

    [Header("Integração com Inventário")]
    public SistemaInventario sistemaInventario; // Para entregarmos a poção na mochila
    public AtributosCombate atributosCombate; // Para entregarmos a poção na mochila
    public DadosItem pocaoDeVida;
    public int precoPocao = 50;

    private bool jogadorPerto = false;
    
    [Header("Upgrades Permanentes")]
    public int precoBonusAtaque = 10;
    public int precoBonusDefesa = 5;

    private void Start()
    {
        if (DadosGlobais.precoBonusAtaque == 0)
        {
            DadosGlobais.precoBonusAtaque = precoBonusAtaque;
        }

        if (DadosGlobais.precoBonusDefesa == 0)
        {
            DadosGlobais.precoBonusDefesa = precoBonusDefesa;
        }
    }

    void Update()
    {
        // Se o jogador estiver perto e apertar 'E', abre a loja
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            AbrirLoja();
        }
    }

    private void AbrirLoja()
    {
        painelHub.SetActive(true);
        painelCompra.SetActive(false);
        painelVenda.SetActive(false);
    }

    public void AbrirAbaCompra()
    {
        painelHub.SetActive(false);
        painelCompra.SetActive(true);
    }

    public void AbrirAbaVenda()
    {
        painelHub.SetActive(false);
        painelVenda.SetActive(true);
        textoFeedbackVenda.text = "O que você encontrou na floresta?";
        GerarListaDeVendas(); // Vasculha a mochila!
    }

    public void FecharTudo()
    {
        painelHub.SetActive(false);
        painelCompra.SetActive(false);
        painelVenda.SetActive(false);
        
        textoFeedbackVenda.text = "";
        textoFeedbackCompra.text = "";

        DadosGlobais.precoBonusAtaque = precoBonusAtaque;
        DadosGlobais.precoBonusDefesa = precoBonusDefesa;
    }


    // --- LÓGICA DE COMPRA ---
    public void ComprarPocao()
    {
        // 1. Verifica se o jogador tem dinheiro suficiente no Banco Central (DadosGlobais)
        if (sistemaInventario.moedas >= precoPocao)
        {
            // 2. Cobra o valor
            sistemaInventario.ModificarMoedas(-precoPocao);
            
            // 3. Entrega o item
            sistemaInventario.AdicionarItem(pocaoDeVida, 1);

            // 4. Feedback visual
            textoFeedbackCompra.text = "Poção comprada com sucesso!";
            Debug.Log("Comprou Poção! Saldo atual: " + DadosGlobais.moedasJogador);
        }
        else
        {
            // O jogador é pobre
            textoFeedbackCompra.text = "Ouro insuficiente! Vá caçar monstros.";
        }
    }
    
    // Adicione esta função abaixo da ComprarPocao()
    public void ComprarBonusAtaque()
    {
        if (sistemaInventario.moedas >= precoBonusAtaque)
        {
            sistemaInventario.ModificarMoedas(-precoBonusAtaque);;
            atributosCombate.bonusAtaque += 10;
            
            // Deixa mais caro para a próxima vez (Inflação do RPG!)
            precoBonusAtaque *= 2;

            textoFeedbackCompra.text = "Espada afiada! (+10 Ataque)";
            
            DadosGlobais.precoBonusAtaque = precoBonusAtaque;
        }
        else
        {
            textoFeedbackCompra.text = "Ouro insuficiente para melhorar a espada!";
        }
    }

    public void ComprarBonusDefesa()
    {
        if (sistemaInventario.moedas >= precoBonusDefesa)
        {
            sistemaInventario.ModificarMoedas(-precoBonusDefesa);
            atributosCombate.bonusDefesa += 25;
            //Cura o player com o valor do bonus
            atributosCombate.hpAtual += 25;
            
            precoBonusDefesa *= 2; 
            textoFeedbackCompra.text = "Armadura reforçada! (+25 Vida)";
            DadosGlobais.precoBonusDefesa = precoBonusDefesa;
        }
        else
        {
            textoFeedbackCompra.text = "Ouro insuficiente para a armadura!";
        }
    }

    // --- LÓGICA DE VENDA DINÂMICA ---
    private void GerarListaDeVendas()
    {
        // 1. Destrói os botões antigos para não duplicar a lista
        foreach (Transform filho in containerVendas)
        {
            Destroy(filho.gameObject);
        }

        // 2. Vasculha a Memória Global
        foreach (SlotInventario slot in DadosGlobais.inventarioAtual)
        {
            // Só cria o botão se a metade do valor do item for pelo menos 1 (item.valor >= 2)
            if (slot.dadosDoItem.valorEmOuro >= 2 && slot.quantidade > 0)
            {
                // Clona o botão e coloca dentro do Container
                GameObject novoBotao = Instantiate(prefabBotaoVenda, containerVendas);

                // Entrega as informações para o botão
                novoBotao.GetComponent<BotaoVendaUI>().ConfigurarBotao(slot.dadosDoItem, slot.quantidade, this);
            }
        }
    }

    // Chamado pelo Botão quando o jogador clica!
    public void ExecutarVenda(DadosItem itemParaVender)
    {
        // O jogador recebe apenas metade do valor do item!
        int lucro = itemParaVender.valorEmOuro / 2;

        // 1. Atualiza o dinheiro global
        sistemaInventario.ModificarMoedas(lucro);

        if (sistemaInventario != null)
        {
            // 2. Usa a sua função pronta que já remove da lista e invoca a UI do inventário!
            sistemaInventario.RemoverItem(itemParaVender, 1);
        }

        textoFeedbackVenda.text = "Vendeu " + itemParaVender.name + " por " + lucro + " Ouro!";

        // 3. Recria os botões de venda para atualizar a quantidade exibida ou remover o botão se acabou!
        GerarListaDeVendas();
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
            FecharTudo(); // Fecha a loja se o jogador for embora
        }
    }
}