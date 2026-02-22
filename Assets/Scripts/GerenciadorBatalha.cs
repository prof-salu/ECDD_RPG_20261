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
        List<GameObject> grupoPrefabs = DadosGlobais.prefabsInimigos;

        for (int i = 0; i < grupoPrefabs.Count; i++)
        {
            if (i >= pontosInimigos.Length) break;

            // Instancia diretamente o prefab sem usar IFs!
            GameObject monstroCriado = Instantiate(grupoPrefabs[i], pontosInimigos[i].position, Quaternion.identity);

            // Desliga a IA de exploração (pois agora estão na arena)
            if (monstroCriado.GetComponent<ControladorInimigo>() != null)
                monstroCriado.GetComponent<ControladorInimigo>().enabled = false;
        }
    }
}