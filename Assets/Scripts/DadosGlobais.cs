using System.Collections.Generic;
using UnityEngine;

// A CLASSE MAGICA: Empacota o Prefab e o Nivel juntos!
[System.Serializable]
public class ConfigInimigo
{
    public GameObject prefab;
    public int nivel = 1;
}

public static class DadosGlobais
{
    public static List<GameObject> prefabsInimigos = new List<GameObject>();

    // --- DADOS INIMIGOS ---
    public static List<int> niveisInimigosParaArena = new List<int>();
    public static string idInimigoEmCombate;
    public static List<string> inimigosDerrotados = new List<string>();

    // --- INVENTARIO REAL ---
    public static Vector2 posicaoRetornoJogador = Vector2.zero;
    public static int pocoesJogador = 0;
    public static int hpAtualJogador = -1;
    public static int nivelJogador = 1;
    public static int xpJogador = 0;
    public static int moedasJogador = 0;
    
    // --- SISTEMA DE MISSÕES LIVRES EM CADEIA ---
    public static Quest questDisponivel;     
    public static Quest questAtiva;          
    public static int progressoQuestAtual = 0; // Serve para monstros OU itens!
    public static bool historiaConcluida = false; 

}