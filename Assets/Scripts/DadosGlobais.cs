using System.Collections.Generic;
using UnityEngine;

public static class DadosGlobais
{
    public static List<GameObject> prefabsInimigos = new List<GameObject>();

    // --- NOVAS VARIÁVEIS DE PERSISTÊNCIA ---
    public static string idInimigoEmCombate;
    public static List<string> inimigosDerrotados = new List<string>(); // O Cemitério
    public static Vector2 posicaoRetornoJogador = Vector2.zero;         // O Checkpoint
}
