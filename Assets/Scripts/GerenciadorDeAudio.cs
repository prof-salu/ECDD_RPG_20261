using UnityEngine;

public class GerenciadorDeAudio : MonoBehaviour
{
    // A Instancia Unica (O Singleton)
    public static GerenciadorDeAudio instancia;

    [Header("Fontes de Áudio")]
    public AudioSource fonteMusica;
    public AudioSource fonteSFX;

    void Awake()
    {
        // Garante que so existe um Gerenciador de Audio vivo no jogo inteiro!
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Sobrevive ao trocar de cena!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funcao para tocar musicas continuas
    public void TocarMusica(AudioClip musica)
    {
        if (fonteMusica.clip == musica) return; // Não reinicia se já estiver a tocar a mesma
        fonteMusica.clip = musica;
        fonteMusica.Play();
    }

    // Funcao para tocar sons rapidos (espada, moedas, clique)
    public void TocarSFX(AudioClip clipeDoSom)
    {
        fonteSFX.PlayOneShot(clipeDoSom);
    }
}