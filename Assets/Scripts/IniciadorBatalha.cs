using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciadorBatalha : MonoBehaviour
{
    // Esta função "captura" os dados de quem a chamou e executa a transição!
    public void DispararBatalha(GameObject player, string idUnico, List<GameObject> inimigosDesteGrupo, List<int> niveis)
    {
        DadosGlobais.posicaoRetornoJogador = player.transform.position;
        DadosGlobais.idInimigoEmCombate = idUnico;

        DadosGlobais.prefabsInimigos = new List<GameObject>(inimigosDesteGrupo);
        DadosGlobais.niveisInimigosParaArena = new List<int>(niveis);

        SceneManager.LoadScene("CenaBatalha");
    }
}