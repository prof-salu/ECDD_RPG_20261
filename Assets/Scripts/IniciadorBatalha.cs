using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciadorBatalha : MonoBehaviour
{
    // Esta função "captura" os dados de quem a chamou e executa a transição!
    public void DispararBatalha(GameObject player, string idUnico, List<GameObject> inimigosDesteGrupo)
    {
        // 1. Salva a posição do herói
        DadosGlobais.posicaoRetornoJogador = player.transform.position;

        // 2. Salva a identidade e a formação para a Arena
        DadosGlobais.idInimigoEmCombate = idUnico;
        DadosGlobais.prefabsInimigos = new List<GameObject>(inimigosDesteGrupo);

        // 3. Muda de cena
        SceneManager.LoadScene("CenaBatalha");
    }
}