using UnityEngine;
using System.Collections.Generic;
using System;

public class SistemaInventario : MonoBehaviour
{
    [Header("Economia")]
    public int moedas = 0; // Nosso dinheiro

    public List<SlotInventario> inventario = new List<SlotInventario>();

    public event Action onInventarioMudou;

    public void AdicionarItem(DadosItem itemParaAdicionar, int quantidade)
    {
        //1. Verificar se o item é empilhavel
        if (itemParaAdicionar.ehEmpilhavel)
        {
            //1.1 Verifica se a mochila possui um item desse tipo
            for (int i = 0; i < inventario.Count; i++) 
            {
                if (inventario[i].dadosDoItem == itemParaAdicionar)
                {
                    inventario[i].AdicionarQuantidade(quantidade);
                    Debug.Log($"Adicionado + {quantidade} ao item {itemParaAdicionar.nomeDoItem}");

                    if (onInventarioMudou != null)
                    {
                        onInventarioMudou.Invoke();
                    }

                    return;
                }
            }
        }

        //2. Item não empilhavel ou ainda não possui um igual
        //Criando um novo slot
        SlotInventario novoSlot = new SlotInventario(itemParaAdicionar, quantidade);

        //Adicionado o slot ao inventario
        inventario.Add(novoSlot);

        Debug.Log($"Novo slot criado: {itemParaAdicionar.nomeDoItem} (x{quantidade})");

        if (onInventarioMudou != null)
        {
            onInventarioMudou.Invoke();
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

                if (onInventarioMudou != null)
                {
                    onInventarioMudou.Invoke();
                }

                return;
            }
        }
    }

    // Funções auxiliares para Crafting
    public bool TemItem(DadosItem item, int qtdNecessaria)
    {
        foreach (SlotInventario slot in inventario)
        {
            if (slot.dadosDoItem == item && slot.quantidade >= qtdNecessaria)
                return true;
        }
        return false;
    }

    public void ModificarMoedas(int valor)
    {
        moedas += valor;
        if (moedas < 0)
        {
            moedas = 0; // Não deixa ficar negativo
        }

        // Avisa a UI para atualizar o texto
        if (onInventarioMudou != null) 
        {
            onInventarioMudou.Invoke();
        };
    }

    // --- MÁGICA PARA O EDITOR ---
    // Esta função é chamada automaticamente pela Unity quando você altera
    // um valor no Inspector. Assim, podemos ver a UI mudar em tempo real!
    private void OnValidate()
    {
        // Só executa se o jogo estiver rodando para evitar erros
        if (Application.isPlaying && onInventarioMudou != null)
        {
            onInventarioMudou.Invoke();
        }
    }
}
