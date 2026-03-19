using System.Collections.Generic;
using UnityEngine;

public class GerenciadorBatalha : MonoBehaviour
{
    public Transform pontoHeroi;
    public Transform[] pontosInimigos;
    public GameObject prefabHeroi;

    void Start()
    {
        // 1. Cria o Heroi e desliga o script de exploracao
        GameObject heroi = Instantiate(prefabHeroi, pontoHeroi.position, Quaternion.identity);

        if (heroi.GetComponent<MovimentacaoExploracao>() != null)
            heroi.GetComponent<MovimentacaoExploracao>().enabled = false;

        // 2. Cria os inimigos automaticamente
        for (int i = 0; i < DadosGlobais.prefabsInimigos.Count; i++)
        {
            if (i >= pontosInimigos.Length) break;

            GameObject monstroCriado = Instantiate(DadosGlobais.prefabsInimigos[i], pontosInimigos[i].position, Quaternion.identity);

            // A INJECAO DE DADOS: Pega a lista do DadosGlobais e aplica no monstro
            AtributosCombate atributos = monstroCriado.GetComponent<AtributosCombate>();
            if (atributos != null && i < DadosGlobais.niveisInimigosParaArena.Count)
            {
                atributos.nivel = DadosGlobais.niveisInimigosParaArena[i];

                // Forca o monstro a recalcular a sua forca imediatamente com o novo nivel
                atributos.CalcularStatus();

                atributos.hpAtual = atributos.hpMaximo;
            }

        }
    }
}