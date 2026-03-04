using System.Collections.Generic;
using UnityEngine;

public class GerenciadorBatalha : MonoBehaviour
{
    public Transform pontoHeroi;
    public Transform[] pontosInimigos;
    public GameObject prefabHeroi;

    void Start()
    {
        // 1. Cria o Herói e desliga o script de exploração
        GameObject heroi = Instantiate(prefabHeroi, pontoHeroi.position, Quaternion.identity);

        if (heroi.GetComponent<MovimentacaoExploracao>() != null)
            heroi.GetComponent<MovimentacaoExploracao>().enabled = false;

        // 2. Cria os inimigos automaticamente
        for (int i = 0; i < DadosGlobais.prefabsInimigos.Count; i++)
        {
            if (i >= pontosInimigos.Length) break;

            GameObject monstroCriado = Instantiate(DadosGlobais.prefabsInimigos[i], pontosInimigos[i].position, Quaternion.identity);

            // A INJEÇÃO DE DADOS: Pega a lista do DadosGlobais e aplica no monstro
            AtributosCombate atributos = monstroCriado.GetComponent<AtributosCombate>();
            if (atributos != null && i < DadosGlobais.niveisInimigosParaArena.Count)
            {
                atributos.nivel = DadosGlobais.niveisInimigosParaArena[i];

                // Força o monstro a recalcular a sua força imediatamente com o novo nível
                atributos.CalcularStatus();

                atributos.hpAtual = atributos.hpMaximo;
            }

        }
    }
}