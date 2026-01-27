using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SistemaInventario : MonoBehaviour
{
    public List<SlotInventario> inventario = new List<SlotInventario>();

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
    }
}
