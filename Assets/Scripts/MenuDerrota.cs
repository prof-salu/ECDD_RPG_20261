using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuDerrota : MonoBehaviour
{
    public void BotaoContinue()
    {
        SceneManager.LoadScene("CenaBatalha");
    }

    public void VoltarAoMenu()
    {
        // O botão "Menu Principal" leva a gente de volta para a tela de abertura do jogo!
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void BotaoSair()
    {
        Debug.Log("Fechando o jogo a partir da tela de derrota...");

        // Se estivermos testando dentro do Editor da Unity, sai do modo Play!
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                // Se for o jogo final compilado, fecha a janela do Windows/Mac
        #else
                    Application.Quit();
        #endif
    }
}