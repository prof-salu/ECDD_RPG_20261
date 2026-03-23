using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void BotaoNovoJogo()
    {
        // 1. Limpa a memória de vidas passadas!
        DadosGlobais.ResetarJogoNovo();

        // 2. Carrega a aventura
        SceneManager.LoadScene("Mundo");
    }

    public void BotaoSair()
    {
        Debug.Log("Fechando o jogo a partir do Menu Principal...");

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}