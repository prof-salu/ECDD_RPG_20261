using System.Collections.Generic;
using UnityEngine;

// A CLASSE MÁGICA: Empacota o Prefab e o Nível juntos!
[System.Serializable]
public class ConfigInimigo
{
    public GameObject prefab;
    public int nivel = 1;
}

public static class DadosGlobais
{
    public static List<GameObject> prefabsInimigos = new List<GameObject>();

    // NOVA LISTA: Vai armazenar os níveis extraídos dos inimigos do mapa!
    public static List<int> niveisInimigosParaArena = new List<int>();

    public static string idInimigoEmCombate;
    public static List<string> inimigosDerrotados = new List<string>();
    public static Vector2 posicaoRetornoJogador = Vector2.zero;

    // PROGRESSÃO E ECONOMIA 
    public static int hpAtualJogador = -1;
    public static int nivelJogador = 1;
    public static int xpJogador = 0;
    public static int moedasJogador = 0;

}