using UnityEngine;
using System.Collections.Generic;
using System;

public class SistemaInventario : MonoBehaviour
{
    [Header("Economia")]
    public int moedas = 0; // Nosso dinheiro

    public List<SlotInventario> inventario = new List<SlotInventario>();

    public event Action OnInventarioMudou;

    void Awake()
    {
        // 1. Se a memoria global estiver vazia (inicio do jogo), salva os itens iniciais do Inspector nela
        if (DadosGlobais.inventarioAtual.Count == 0 && inventario.Count > 0)
        {
            DadosGlobais.inventarioAtual = new List<SlotInventario>(inventario);
        }        
        inventario = DadosGlobais.inventarioAtual;
        
        moedas = DadosGlobais.moedasJogador;
    }

    public void AdicionarItem(DadosItem itemParaAdicionar, int quantidade)
    {
        //1. Verificar se o item e empilhavel
        if (itemParaAdicionar.ehEmpilhavel)
        {
            //1.1 Verifica se a mochila possui um item desse tipo
            for (int i = 0; i < inventario.Count; i++) 
            {
                if (inventario[i].dadosDoItem == itemParaAdicionar)
                {
                    inventario[i].AdicionarQuantidade(quantidade);
                    Debug.Log($"Adicionado + {quantidade} ao item {itemParaAdicionar.nomeDoItem}");

                    if (OnInventarioMudou != null)
                    {
                        OnInventarioMudou.Invoke();
                    }

                    return;
                }
            }
        }

        //2. Item nao empilhavel ou ainda nao possui um igual
        //Criando um novo slot
        SlotInventario novoSlot = new SlotInventario(itemParaAdicionar, quantidade);

        //Adicionado o slot ao inventario
        inventario.Add(novoSlot);

        Debug.Log($"Novo slot criado: {itemParaAdicionar.nomeDoItem} (x{quantidade})");

        if (OnInventarioMudou != null)
        {
            OnInventarioMudou.Invoke();
        }
    }
     
    public void RemoverItem(DadosItem itemParaRemover, int quantidade)
    {        
        //1. Verifica se a mochila possui um item desse tipo
        for (int i = 0; i < inventario.Count; i++)
        {
            if (inventario[i].dadosDoItem == itemParaRemover)
            {
                inventario[i].quantidade -= quantidade;

                if (inventario[i].quantidade > 0)
                {
                    Debug.Log($"Removido + {quantidade} ao item {itemParaRemover.nomeDoItem}");
                }
                else
                {
                    inventario.RemoveAt(i);
                    Debug.Log($"Item removido do inventario: {itemParaRemover.nomeDoItem}");
                }

                if (OnInventarioMudou != null)
                {
                    OnInventarioMudou.Invoke();
                }

                return;
            }
        }
    }

    // Funcoes auxiliares para Crafting
    public bool TemItem(DadosItem item, int qtdNecessaria)
    {
        foreach (SlotInventario slot in inventario)
        {
            if (slot.dadosDoItem == item && slot.quantidade >= qtdNecessaria)
                return true;
        }
        return false;
    }

    public bool ConsumirItem(DadosItem item)
    {
        foreach (SlotInventario slot in inventario)
        {
            if (slot.dadosDoItem == item)
            {
                slot.quantidade--; // Gasta 1

                // Se a quantidade chegar a zero, removemos o slot da lista
                if (slot.quantidade <= 0)
                {
                    inventario.Remove(slot);
                }

                // Avisa a Interface para se redesenhar
                if (OnInventarioMudou != null) OnInventarioMudou.Invoke();

                return true; // Sucesso!
            }
        }
        return false; // O jogador nao tinha esse item
    }

    public void ModificarMoedas(int valor)
    {
        moedas += valor;
        Debug.Log("Voce encontrou " + valor + " moedas!");
        if (moedas < 0)
        {
            moedas = 0; // Nao deixa ficar negativo
        }

        // Avisa a UI para atualizar o texto
        if (OnInventarioMudou != null) 
        {
            OnInventarioMudou.Invoke();
        };
    }

    // --- MAGICA PARA O EDITOR ---
    // Esta funcao e chamada automaticamente pela Unity quando voce altera
    // um valor no Inspector. Assim, podemos ver a UI mudar em tempo real!
    private void OnValidate()
    {
        // So executa se o jogo estiver rodando para evitar erros
        if (Application.isPlaying && OnInventarioMudou != null)
        {
            OnInventarioMudou.Invoke();
        }
    }
}
