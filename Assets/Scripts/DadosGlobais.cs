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
    public static int hpAtualJogador = -1;
    public static int nivelJogador = 1;
    public static int xpJogador = 0;
    public static int moedasJogador = 0;
    // NOVO: Acesso ao Inventário para consumo de itens na Arena e exibição na UI
    public static List<SlotInventario> inventarioAtual = new List<SlotInventario>();
    
    // --- SISTEMA DE MISSÕES LIVRES EM CADEIA ---
    public static Quest questDisponivel;     
    public static Quest questAtiva;          
    public static int progressoQuestAtual = 0; // Serve para monstros OU itens!
    public static bool historiaConcluida = false; 
    
    // NOVO: Bônus Permanentes comprados na Loja
    public static int bonusAtaque = 0;
    public static int bonusDefesa = 0;
    public static int precoBonusAtaque = 0;
    public static int precoBonusDefesa = 0;

}